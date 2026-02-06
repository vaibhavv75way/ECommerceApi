using Microsoft.EntityFrameworkCore;
using EcommerceApi.Domain.Entities;

namespace EcommerceApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
}