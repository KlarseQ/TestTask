using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using TestTask.DBStuff;
using TestTask.DBStuff.Repositories;
using TestTask.ViewModels;
using Org.BouncyCastle.Crypto.Engines;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.AspNetCore.Http.Extensions;

namespace TestTask.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private IWebHostEnvironment _webHostEnvironment;
        private UrlReporitory _urlRepository;
        private readonly IServer _server;

        public UrlController(IWebHostEnvironment webHostEnvironment,
            UrlReporitory urlRepository,
            IServer server)
        {
            _webHostEnvironment = webHostEnvironment;
            _urlRepository = urlRepository;
            _server = server;
        }

        public string GenerateShortUrl([FromForm] string longUrl = "test", [FromForm] int length = 3)
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string chars = "!@#$%^&*~";
            //string shortUrl = _server.Features.Get<IServerAddressesFeature>().Addresses.ToList()[1];
            string shortUrl = "";
            if (longUrl.Contains("http://") || longUrl.Contains("https://") || longUrl.Contains("www."))
            {
                var hash = longUrl.GetHashCode().ToString();
                for (int i = 0; i < length / 3; i++)
                {
                    var randomSumbol = new Random();
                    int randomSumbolNumber = randomSumbol.Next(0, chars.Length - 1);
                    var randomLetter = new Random();
                    int randomLetterNumber = randomLetter.Next(0, letters.Length - 1);
                    string willAdd = $"{hash[DateTime.Now.Second / hash.Length]}{chars[randomSumbolNumber]}{letters[randomLetterNumber]}";
                    var rnd = new Random();
                    var shuffle = string.Join("", willAdd.OrderBy(x => rnd.Next()));
                    shortUrl += shuffle;
                }
            }
            else
                return "Проверьте правильность ввода. Ссылка должна начинаться на: \"http://\",\"https://\" или \"www.\"";
            return shortUrl;
        }
        public void AddInfoToDB([FromForm] string longUrl, [FromForm] string shortUrl)
        {
            if (_urlRepository.GetByLongUrl(longUrl) != null || _urlRepository.GetByShortUrl(shortUrl) != null)
                throw new Exception("Такой URL уже есть в базе");
            var info = new Url()
            {
                LongUrl = longUrl,
                ShortUrl = shortUrl,
                CreationDate = DateTime.Now.ToString("d"),
                LinkCount = 0
            };
            _urlRepository.Save(info);
        }
        public void ChangeUrlInfo([FromForm] string oldLongUrl, [FromForm] bool changeShortUrl, [FromForm] bool resetCounter, [FromForm] string newLongUrl, [FromForm] int newShortUrlLength = 3)
        {
            var currentUrl = _urlRepository.GetByLongUrl(oldLongUrl);
            if (_urlRepository.GetByLongUrl(newLongUrl) != null && oldLongUrl != newLongUrl)
            {
                throw new Exception("Адрес уже есть в базе");
            }
            if (newLongUrl != null)
                currentUrl.LongUrl = newLongUrl;
            if (changeShortUrl)
                currentUrl.ShortUrl = GenerateShortUrl(newLongUrl, newShortUrlLength);
            if (resetCounter)
                currentUrl.LinkCount = 0;
            _urlRepository.Save(currentUrl);
        }
        public List<UrlViewModel> GetAllUrlsInfo()
        {
            var allUrls = _urlRepository.GetAll().ToList();
            var listUrls = new List<UrlViewModel>();
            foreach (var url in allUrls)
            {
                listUrls.Add(new UrlViewModel
                {
                    LongUrl = url.LongUrl,
                    ShortUrl = url.ShortUrl,
                    LinkCount = url.LinkCount,
                    CreationDate = url.CreationDate
                });
            }
            return listUrls;
        }
        [HttpGet, Route("/{url}")]
        public IActionResult RedirectToUrl([FromRoute] string url)
        {
            if (_urlRepository.GetByShortUrl(url) != null)
            {
                var currentUrl = _urlRepository.GetByShortUrl(url);
                currentUrl.LinkCount++;
                _urlRepository.Save(currentUrl);
                try
                {
                    return Redirect($"{_urlRepository.GetByShortUrl(url).LongUrl}");
                }
                catch
                {
                    throw new Exception("Не корректный адрес");
                }
            }
            else
                throw new Exception("Адрес отсутствует в базе");
        }
        public void RemoveUrlFromDB([FromForm] string shortUrl) => _urlRepository.Remove(_urlRepository.GetByShortUrl(shortUrl));
    }
}

