using Microsoft.EntityFrameworkCore;
using ErrandUserAPI.Models;
namespace ErrandUserAPI.Data
{
    
    public class UserDbContext: DbContext
    {
        protected readonly IConfiguration Configuration;
        public UserDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<User> Users { get; set; }
    }
}
