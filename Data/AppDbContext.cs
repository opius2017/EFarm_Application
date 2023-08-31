using EFarm.Server.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFarm.Server.Data;

public class AppDbContext : DbContext
{
	protected readonly IConfiguration Configuration;

	public AppDbContext(IConfiguration configuration)
	{
		Configuration = configuration;
	}
	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		options.UseSqlite(Configuration.GetConnectionString("Default"));
	}
	//public AppDbContext(DbContextOptions options) : base(options) {
	//}

	public DbSet<Product> Products { get; set; }
	public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItem { get; internal set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<Product>().HasKey(p => p.Id);
		modelBuilder.Entity<Basket>().HasKey(p => p.Id);
		modelBuilder.Entity<Basket>().HasMany(x => x.Items)
			.WithOne().HasForeignKey(x=>x.BasketId).OnDelete(DeleteBehavior.Cascade);

	}
}