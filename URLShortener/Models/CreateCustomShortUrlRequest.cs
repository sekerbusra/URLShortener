namespace URLShortener.Models
{
    public class CreateCustomShortUrlRequest
    {
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }

    }
}
