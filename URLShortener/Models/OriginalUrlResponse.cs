namespace URLShortener.Models
{
    public class OriginalUrlResponse
    {
        public string ShortUrl { get; set; }
        public string OriginalUrl { get; set; }
        public string? Message { get; set; }
    }
}
