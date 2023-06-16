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

        public async Task<OriginalUrlResponse> GetOriginalUrlByShortUrl(OriginalUrlRequest request)
        {
            var result = new OriginalUrlResponse();

            // Check originalURL 
            if (!string.IsNullOrEmpty(request.shortUrl))
            {
                // Validate and process the URL
                if (!IsUrlValid(request.shortUrl))
                {
                    throw new ArgumentException("Invalid URL.");
                }

                // Check this data stored in the database
                var recordFromDb = await _repository.GetByShortUrl(request.shortUrl);

                if (recordFromDb != null)
                {
                    // Get shortened URL and keep it in that variable
                    result = new OriginalUrlResponse
                    {
                        OriginalUrl = recordFromDb.OriginalUrl,
                        ShortUrl = recordFromDb.ShortUrl,
                        Message = "Original url found for short url."
                    };

                    return result;
                }
                throw new ArgumentException("There is no data in database for short url");
            }
            throw new ArgumentException("Short url can not be null");
        }

        public async Task<CreateCustomShortUrlResponse> CreateCustomShortUrl(CreateCustomShortUrlRequest request)
        {
            var result = new CreateCustomShortUrlResponse();
            var shortenedUrl = new ShortenedUrl();


            if (string.IsNullOrEmpty(request.OriginalUrl))
            {
                throw new ArgumentException("Original Url can not be null");
            }
            else
            {
                if (!IsUrlValid(request.OriginalUrl))
                {
                    throw new ArgumentException("Invalid original URL.");
                }
            }

            if (string.IsNullOrEmpty(request.ShortUrl))
            {
                throw new ArgumentException("Short Url can not be null");
            }
            else
            {
                if (!IsUrlValid(request.ShortUrl))
                {
                    throw new ArgumentException("Invalid short URL.");
                }
            }

            // Check this data stored in the database
            var recordFromDb = await _repository.GetByOriginalUrl(request.OriginalUrl);

            if (recordFromDb != null)
            {

                shortenedUrl = new ShortenedUrl
                {
                    Id = recordFromDb.Id,
                    OriginalUrl = recordFromDb.OriginalUrl,
                    ShortUrl = request.ShortUrl,
                    CreatedAt = DateTime.UtcNow
                };

                // Update the shortened URL to the database
                var data = await _repository.Update(shortenedUrl);

                if (data != null)
                {
                    result = new CreateCustomShortUrlResponse
                    {
                        OriginalUrl = data.OriginalUrl,
                        ShortUrl = data.ShortUrl,
                        Message = "Short url has been updated for existing original url."
                    };
                }

            }
            else
            {
                var recoredData = await _repository.GetByShortUrl(request.ShortUrl);
                if (recoredData == null)
                {
                    shortenedUrl = new ShortenedUrl
                    {
                        OriginalUrl = request.OriginalUrl,
                        ShortUrl = request.ShortUrl,
                        CreatedAt = DateTime.UtcNow
                    };

                    // Save the shortened URL to the database
                    await _repository.Create(shortenedUrl);

                    result = new CreateCustomShortUrlResponse
                    {
                        OriginalUrl = shortenedUrl.OriginalUrl,
                        ShortUrl = shortenedUrl.ShortUrl,
                        Message = "New record has been created."
                    };
                }
                else
                    throw new ArgumentException("There is another original url exist for this short url.");
            }
            return result;
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
