namespace URLShortener.Models
{
    public class ShortenedUrlResponse
    {
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public string Message { get; set; }
    }
}
