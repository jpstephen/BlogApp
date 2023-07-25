using Microsoft.EntityFrameworkCore;
using BlogApp.Models;

namespace BlogApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Article> AllArticles { get; set; }

    }
}
