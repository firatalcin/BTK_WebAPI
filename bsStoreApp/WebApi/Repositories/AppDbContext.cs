using Microsoft.EntityFrameworkCore;
using WebApi.Model;

namespace WebApi.Repositories;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Book> Books { get; set; }
}