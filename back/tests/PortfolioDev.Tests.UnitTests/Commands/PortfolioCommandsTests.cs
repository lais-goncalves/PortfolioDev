using PortfolioDev.Domain.Models;
using PortfolioDev.Infrastructure.Commands;
using PortfolioDev.Infrastructure.DbContexts;
using PortfolioDev.Tests.UnitTests.Fixtures;

namespace PortfolioDev.Tests.UnitTests.Commands;

public class PortfoliosCommandsTests : IClassFixture<DbContextFixture>
{
	private readonly DbContextFixture _fixture;

	public PortfoliosCommandsTests(DbContextFixture fixture) { _fixture = fixture; }


	#region DML
	[Fact]
	public async Task AddAsync_DeveCriarId_QuandoPortfolioAdicionado()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();

		var portfolio3 = new Portfolio
		{
			Id = 3,
			Descricao = "Portfolio 3",
			UsuarioId = 3
		};

		var commands = new PortfoliosCommands(contexto);
		bool adicionado = await commands.AddAsync(portfolio3);

		Assert.True(adicionado);
	}

	[Fact]
	public async Task AddAsync_NaoDeveAdd_QuandoUsuarioJaPossuiPortfolio()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();

		var portfolio = new Portfolio { UsuarioId = 1, Descricao = "Portfolio 1" };

		var commands = new PortfoliosCommands(contexto);
		await Assert.ThrowsAnyAsync<Exception>(async () => await commands.AddAsync(portfolio));
	}

	[Fact]
	public async Task AddAsync_NaoDeveAdd_QuandoUsuarioNaoExiste()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();

		var portfolio = new Portfolio { UsuarioId = 99, Descricao = "Portfolio 1" };

		var commands = new PortfoliosCommands(contexto);
		await Assert.ThrowsAnyAsync<Exception>(async () => { await commands.AddAsync(portfolio); });
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	public async Task UpdateAsync_DeveAtualizarPortfolio_QuandoPortfolioExiste(int id)
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new PortfoliosCommands(contexto);

		Portfolio? portfolio = await commands.BuscarPortfolioPorIdAsync(id);
		Assert.NotNull(portfolio);

		var descDepoisAtualizacao = "depois do update";
		portfolio.Descricao = descDepoisAtualizacao;

		bool atualizou = await commands.UpdateAsync(portfolio);
		Assert.True(atualizou);

		Portfolio? portfolioAtualizado = await commands.BuscarPortfolioPorIdAsync(id);
		Assert.NotNull(portfolioAtualizado);

		Assert.Equal(id, portfolioAtualizado.Id);
		Assert.Equal(descDepoisAtualizacao, portfolioAtualizado.Descricao);
		Assert.Equal(id, portfolioAtualizado.UsuarioId);
	}

	[Fact]
	public async Task UpdateAsync_NaoDeveAtualizarPortfolio_QuandoPortfolioNaoExiste()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();

		var portfolio = new Portfolio { Id = 99, Descricao = "teste" };

		var commands = new PortfoliosCommands(contexto);
		await Assert.ThrowsAnyAsync<Exception>(async () => await commands.UpdateAsync(portfolio));
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	public async Task DeleteAsync_DeveDeletarPortfolio_QuandoPortfolioExiste(int id)
	{
		PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new PortfoliosCommands(contexto);

		bool deletado = await commands.DeleteAsync(id);
		Assert.True(deletado);

		Portfolio? portfolioDeletado = await commands.BuscarPortfolioPorIdAsync(id);
		Assert.Null(portfolioDeletado);
	}
	#endregion DML


	#region Buscas
	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	public async Task BuscarPortfolioPorId_DeveRetornarPortfolio_QuandoPortfolioExiste(int id)
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new PortfoliosCommands(contexto);

		Portfolio? resultado = await commands.BuscarPortfolioPorIdAsync(id);

		Assert.NotNull(resultado);
		Assert.Equal(id, resultado.Id);
	}

	[Fact]
	public async Task BuscarPortfolioPorId_DeveRetornarNull_QuandoPortfolioNaoExiste()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new PortfoliosCommands(contexto);

		Portfolio? resultado = await commands.BuscarPortfolioPorIdAsync(99);

		Assert.Null(resultado);
	}

	[Fact]
	public async Task BuscarPortfoliosAsync_DeveRetornarPortfolios_QuandoExistirem()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new PortfoliosCommands(contexto);

		Portfolio[] portfolios = await commands.BuscarPortfoliosAsync();

		Assert.NotNull(portfolios);
		Assert.True(portfolios.Length > 0);
	}

	[Fact]
	public async Task BuscarPortfoliosAsync_DeveRetornarVazio_QuandoNaoExistirem()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new PortfoliosCommands(contexto);

		Portfolio[] portfolios = await commands.BuscarPortfoliosAsync();

		Assert.NotNull(portfolios);
		Assert.NotEmpty(portfolios);
	}

	[Theory]
	[InlineData("usuario1", "Portfolio 1")]
	[InlineData("usuario2", "Portfolio 2")]
	public async Task BuscarPortfolioPorUserNameUsuarioAsync_DeveRetornarPortfolio_QuandoUsuarioExiste(
		string userName,
		string descricao
	)
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new PortfoliosCommands(contexto);

		Portfolio? portfolio = await commands.BuscarPortfolioPorUserNameUsuarioAsync(userName);

		Assert.NotNull(portfolio);
		Assert.Equal(descricao, portfolio.Descricao);
	}

	[Theory]
	[InlineData("usuario5")]
	[InlineData("usuario6")]
	public async Task BuscarPortfolioPorUserNameUsuarioAsync_DeveRetornarNull_QuandoPortfolioNaoExiste(string userName)
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new PortfoliosCommands(contexto);

		Portfolio? portfolio = await commands.BuscarPortfolioPorUserNameUsuarioAsync(userName);

		Assert.Null(portfolio);
	}

	[Fact]
	public async Task BuscarPortfolioPorUserNameUsuarioAsync_DeveRetornarNull_QuandoUsuarioNaoExiste()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new PortfoliosCommands(contexto);

		Portfolio? portfolio = await commands.BuscarPortfolioPorUserNameUsuarioAsync("usuario3");

		Assert.Null(portfolio);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	public async Task BuscarPortfolioPorIdUsuarioAsync_DeveRetornarPortfolio_QuandoUsuarioExiste(int usuarioId)
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new PortfoliosCommands(contexto);

		Portfolio? portfolio = await commands.BuscarPortfolioPorIdUsuarioAsync(usuarioId);

		Assert.NotNull(portfolio);
		Assert.Equal(usuarioId, portfolio.UsuarioId);
	}

	[Fact]
	public async Task BuscarPortfolioPorIdUsuarioAsync_DeveRetornarNull_QuandoUsuarioNaoExiste()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new PortfoliosCommands(contexto);

		Portfolio? portfolio = await commands.BuscarPortfolioPorIdUsuarioAsync(99);

		Assert.Null(portfolio);
	}

	[Fact]
	public async Task BuscarPortfolioPorIdUsuarioAsync_DeveRetornarNull_QuandoUsuarioNaoTemPortfolio()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new PortfoliosCommands(contexto);

		Portfolio? portfolio = await commands.BuscarPortfolioPorIdUsuarioAsync(3);

		Assert.Null(portfolio);
	}

	[Theory]
	[InlineData(1, 1)]
	[InlineData(2, 2)]
	public async Task PortfolioPertenceAoUsuarioAsync_DeveRetornarTrue_CasoPortfolioPertencaAoUsuario(
		int portfolioId,
		int usuarioId
	)
	{
		PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new PortfoliosCommands(contexto);

		bool pertence = await commands.PortfolioPertenceAoUsuarioAsync(portfolioId, usuarioId);

		Assert.True(pertence);
	}

	[Theory]
	[InlineData(1, 2)]
	[InlineData(2, 1)]
	public async Task PortfolioPertenceAoUsuarioAsync_DeveRetornarFalse_CasoPortfolioNaoPertencaAoUsuario(
		int portfolioId,
		int usuarioId
	)
	{
		PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new PortfoliosCommands(contexto);

		bool pertence = await commands.PortfolioPertenceAoUsuarioAsync(portfolioId, usuarioId);

		Assert.False(pertence);
	}
	#endregion Buscas
}