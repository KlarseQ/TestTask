using Microsoft.EntityFrameworkCore;

namespace TestTask.DBStuff.Repositories
{
    public class UrlReporitory : BaseRepository<Url>
    {
        public UrlReporitory(MyDBContext context) : base(context)
        {

        }
        public Url GetByLongUrl(string longUrl)
        {
            return _dbSet.FirstOrDefault(u => u.LongUrl == longUrl);
        }
        public Url GetByShortUrl(string shortUrl)
        {
            return _dbSet.FirstOrDefault(u => u.ShortUrl == shortUrl);
        }
    }
}
