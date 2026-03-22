using Microsoft.EntityFrameworkCore;
using PortfolioDev.Domain.Models;
using PortfolioDev.Domain.Models.Identity;
using PortfolioDev.Infrastructure.DbContexts;

namespace PortfolioDev.Tests.UnitTests.Fixtures;

public class DbContextFixture : IAsyncLifetime
{
	public async Task InitializeAsync() { }

	public Task DisposeAsync() { return Task.CompletedTask; }

	public async Task<PlataformaDevsContext> CriarContexto(bool? comDados = true)
	{
		DbContextOptions<PlataformaDevsContext> options = new DbContextOptionsBuilder<PlataformaDevsContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options;

		var contexto = new PlataformaDevsContext(options);
		if (comDados == true) await SeedDadosBaseAsync(contexto);

		return new PlataformaDevsContext(options);
	}

	public async Task SeedDadosBaseAsync(PlataformaDevsContext contexto)
	{
		var usuario1 = new Usuario
		{
			Id = 1,
			NomeCompleto = "Usuario 1",
			UserName = "usuario1",
			Email = "teste1@teste"
		};
		var usuario2 = new Usuario
		{
			Id = 2,
			NomeCompleto = "Usuario 2",
			UserName = "usuario2",
			Email = "teste2@teste"
		};
		var usuario3 = new Usuario
		{
			Id = 3,
			NomeCompleto = "Usuario 3",
			UserName = "usuario3",
			Email = "teste3@teste"
		};

		var portfolio1 = new Portfolio
		{
			Id = 1,
			Descricao = "Portfolio 1",
			UsuarioId = 1
		};
		var portfolio2 = new Portfolio
		{
			Id = 2,
			Descricao = "Portfolio 2",
			UsuarioId = 2
		};

		var projeto1 = new Projeto
		{
			Id = 1,
			Nome = "Projeto 1",
			Descricao = "Projeto 1",
			PortfolioId = 1
		};
		var projeto2 = new Projeto
		{
			Id = 2,
			Nome = "Projeto 2",
			Descricao = "Projeto 2",
			PortfolioId = 1
		};
		var projeto3 = new Projeto
		{
			Id = 3,
			Nome = "Projeto 3",
			Descricao = "Projeto 3",
			PortfolioId = 2
		};
		var projeto4 = new Projeto
		{
			Id = 4,
			Nome = "Projeto 4",
			Descricao = "Projeto 4",
			PortfolioId = 2
		};

		await contexto.AddRangeAsync
		(
			usuario1,
			usuario2,
			usuario3,
			portfolio1,
			portfolio2,
			projeto1,
			projeto2,
			projeto3,
			projeto4
		);
		await contexto.SaveChangesAsync();
	}
}