using Microsoft.EntityFrameworkCore;
using WebApi.Model;

namespace WebApi.Repositories;

public class AppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
}