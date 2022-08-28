using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using System.Reflection.Emit;
using static System.Net.Mime.MediaTypeNames;

namespace TestTask.DBStuff
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Url> Url { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
