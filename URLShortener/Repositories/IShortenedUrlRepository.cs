﻿using URLShortener.Models;
using System.Threading.Tasks;

namespace URLShortener.Respositories
{
    public interface IShortenedUrlRepository
    {
        Task<ShortenedUrl> GetByOriginalUrl(string originalUrl);
        Task<ShortenedUrl> Create(ShortenedUrl shortenedUrl);
        Task<ShortenedUrl> GetByShortUrl(string shortUrl);
        Task<ShortenedUrl> Update(ShortenedUrl shortenedUrl);
    }
}
