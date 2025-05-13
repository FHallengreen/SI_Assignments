using Microsoft.EntityFrameworkCore;
using webhook.Models;

namespace webhook.Data;

public class AppDbContext : DbContext
{
    public DbSet<Webhook> Webhooks { get; set; }
    public DbSet<Payment> Payments { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}