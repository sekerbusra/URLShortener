using Microsoft.AspNetCore.Mvc;
using URLShortener.Services;
using URLShortener.Models;
using System;
using System.Threading.Tasks;


namespace URLShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortenUrlController : ControllerBase
    {
        private readonly IUrlShortenerService _urlShortenerService;

        public ShortenUrlController(IUrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }

        [Route("ShortenUrl")]
        [HttpPost]
        public async Task<ActionResult<string>> ShortenUrl(ShortenedUrlRequest request)
        {
            try
            {
                var shortUrl = await _urlShortenerService.ShortenUrl(request);
                return Ok(shortUrl);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
