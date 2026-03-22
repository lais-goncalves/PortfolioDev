using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortfolioDev.Domain.Models;
using PortfolioDev.Domain.Models.Identity;

namespace PortfolioDev.Infrastructure.DbContexts;

public class PlataformaDevsContext
	: IdentityDbContext<
		Usuario,
		IdentityRole<int>,
		int
	>
{
	public PlataformaDevsContext(DbContextOptions opcoes) : base(opcoes) { }
	public DbSet<Usuario> Usuarios { get; set; }
	public DbSet<Portfolio> Portfolios { get; set; }
	public DbSet<Projeto> Projetos { get; set; }
	
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.EnableSensitiveDataLogging();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Portfolio>()
			.HasOne<Usuario>(p => p.Usuario)
			.WithOne(u => u.Portfolio)
			.HasForeignKey<Portfolio>(p => p.UsuarioId)
			.OnDelete(DeleteBehavior.NoAction);

		modelBuilder.Entity<Portfolio>()
			.HasMany(p => p.Projetos)
			.WithOne(p => p.Portfolio)
			.HasForeignKey(p => p.PortfolioId);
	}
}