using System.Text;
using URLShortener.Models;
using URLShortener.Respositories;

namespace URLShortener.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly IShortenedUrlRepository _repository;

        // Custom domain name when you create short url 
        private readonly string _domain = "http://sample-site/";

        public UrlShortenerService(IShortenedUrlRepository repository)
        {
            this._repository = repository;
        }

        public async Task<ShortenedUrlResponse> ShortenUrl(ShortenedUrlRequest request)
        {
            var result = new ShortenedUrlResponse();

            // Check originalURL 
            if (!string.IsNullOrEmpty(request.OriginalUrl))
            {
                // Validate and process the URL
                if (!IsUrlValid(request.OriginalUrl))
                {
                    throw new ArgumentException("Invalid URL.");
                }

                // Check this data stored in the database
                var recordFromDb = await _repository.GetByOriginalUrl(request.OriginalUrl);

                if (recordFromDb != null)
                {
                    // Get shortened URL and keep it in that variable
                    result = new ShortenedUrlResponse
                    {
                        OriginalUrl = recordFromDb.OriginalUrl,
                        ShortUrl = recordFromDb.ShortUrl,
                        Message = "This original url already exist in the database , here is the created short url"
                    };

                    return result;
                }
                else
                {
                    // Generate a unique short URL
                    var createdRandomStr = GenerateShortUrl(6);

                    // Create a new shortened URL
                    var shortenedUrl = new ShortenedUrl
                    {
                        OriginalUrl = request.OriginalUrl,
                        ShortUrl = _domain + createdRandomStr,
                        CreatedAt = DateTime.UtcNow
                    };

                    // Save the shortened URL to the database
                    var data = await _repository.Create(shortenedUrl);

                    if (data != null)
                    {
                        result = new ShortenedUrlResponse
                        {
                            OriginalUrl = data.OriginalUrl,
                            ShortUrl = data.ShortUrl,
                            Message = "Short url has been created for this original url."
                        };

                        return result;
                    }
                }
            }
            throw new ArgumentException("Original url can not be null");
        }

        private string GenerateShortUrl(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(0, chars.Length);
                stringBuilder.Append(chars[index]);
            }

            var createdRandomStr = stringBuilder.ToString();

            return createdRandomStr;
        }

        public bool IsUrlValid(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
                   (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                return false;
            }

            return true;
        }
    }
}
