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

        public async Task<ShortenedUrl> GetByShortUrl(string shortUrl)
        {
            var data = await _dbContext.Set<ShortenedUrl>().Where(op => op.ShortUrl.Contains(shortUrl))
                .FirstOrDefaultAsync();
            return data;
        }

        public async Task<ShortenedUrl> GetById(int id)
        {
            var data = await _dbContext.Set<ShortenedUrl>().Where(op => op.Id == id)
                .FirstOrDefaultAsync();
            if (data != null)
            {
                return data;
            }
            throw new ArgumentException("data not found");
        }

        public async Task<ShortenedUrl> Update(ShortenedUrl shortenedUrl)
        {

            var persistent = await GetById(shortenedUrl.Id);
            if (persistent != null)
            {
                _dbContext.Entry(persistent).CurrentValues.SetValues(shortenedUrl);
                await _dbContext.SaveChangesAsync();
            }
            else
                throw new ArgumentException("Could not updated");

            return shortenedUrl;
        }
    }
}
