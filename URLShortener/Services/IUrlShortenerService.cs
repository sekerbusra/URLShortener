using System.Threading.Tasks;
using URLShortener.Migrations;
using URLShortener.Models;

namespace URLShortener.Services
{
    public interface IUrlShortenerService
    {
        Task<ShortenedUrlResponse> ShortenUrl(ShortenedUrlRequest request);
    }
}
