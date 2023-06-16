using Microsoft.EntityFrameworkCore;
using URLShortener.Data;
using URLShortener.Models;

namespace URLShortener.Respositories
{
    public class ShortenedUrlRepository : IShortenedUrlRepository
    {
        private readonly AppDbContext _dbContext;

        public ShortenedUrlRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ShortenedUrl> GetByOriginalUrl(string originalUrl)
        {
            var data = await _dbContext.Set<ShortenedUrl>()
                .Where(u => u.OriginalUrl.Contains(originalUrl)).FirstOrDefaultAsync();
            return data;
        }

        public async Task<ShortenedUrl> Create(ShortenedUrl shortenedUrl)
        {
            _dbContext.Set<ShortenedUrl>().Add(shortenedUrl);
            await _dbContext.SaveChangesAsync();
            return shortenedUrl;
        }
    }
}
