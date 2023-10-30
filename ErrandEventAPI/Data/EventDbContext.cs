using Microsoft.EntityFrameworkCore;
using ErrandEventAPI.Models;
namespace ErrandEventAPI.Data
{
    
    public class EventDbContext: DbContext
    {
        protected readonly IConfiguration Configuration;
        public EventDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<Event> Events { get; set; }
    }
}
