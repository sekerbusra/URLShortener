namespace URLShortener.Models
{
    public class CreateCustomShortUrlResponse
    {
        public string ShortUrl { get; set; }
        public string OriginalUrl { get; set; }
        public string? Message { get; set; }
    }
}
