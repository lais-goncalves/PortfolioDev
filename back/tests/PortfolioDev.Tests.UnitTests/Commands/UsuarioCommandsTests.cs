using PortfolioDev.Domain.Models.Identity;
using PortfolioDev.Infrastructure.Commands;
using PortfolioDev.Infrastructure.DbContexts;
using PortfolioDev.Tests.UnitTests.Fixtures;

namespace PortfolioDev.Tests.UnitTests.Commands;

public class UsuarioCommandsTests : IClassFixture<DbContextFixture>
{
	private readonly DbContextFixture _fixture;

	public UsuarioCommandsTests(DbContextFixture fixture) { _fixture = fixture; }

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	public async Task BuscarUsuarioPorIdAsync_DeveRetornarUsuario_QuandoIdExiste(int id)
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new UsuariosCommands(contexto);

		Usuario? usuario = await commands.BuscarUsuarioPorIdAsync(id);

		Assert.NotNull(usuario);
		Assert.Equal(id, usuario.Id);
	}

	[Fact]
	public async Task BuscarUsuarioPorIdAsync_DeveRetornarNull_QuandoIdNaoExiste()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new UsuariosCommands(contexto);

		Usuario? usuario = await commands.BuscarUsuarioPorIdAsync(99);

		Assert.Null(usuario);
	}

	[Theory]
	[InlineData("usuario1")]
	[InlineData("usuario2")]
	[InlineData("usuario3")]
	public async Task BuscarUsuarioPorUserNameAsync_DeveRetornarUsuario_QuandoUserNameExiste(string userName)
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new UsuariosCommands(contexto);

		Usuario? usuario = await commands.BuscarUsuarioPorUserNameAsync(userName);

		Assert.NotNull(usuario);
		Assert.Equal(userName, usuario.UserName);
	}

	[Fact]
	public async Task BuscarUsuarioPorUserNameAsync_DeveRetornarNull_QuandoUserNameNaoExiste()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new UsuariosCommands(contexto);

		Usuario? usuario = await commands.BuscarUsuarioPorUserNameAsync("teste");

		Assert.Null(usuario);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	public async Task BuscarIdPortfolioUsuarioAsync_DeveRetornarId_QuandoUsuarioEPortfolioExistem(int id)
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new UsuariosCommands(contexto);

		int? portfolioId = await commands.BuscarPortfolioIdDoUsuarioAsync(id);

		Assert.NotNull(portfolioId);
		Assert.Equal(portfolioId, id);
	}

	[Fact]
	public async Task BuscarIdPortfolioUsuarioAsync_DeveRetornarNull_QuandoUsuarioNaoExiste()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new UsuariosCommands(contexto);

		int? portfolioId = await commands.BuscarPortfolioIdDoUsuarioAsync(99);

		Assert.Null(portfolioId);
	}

	[Fact]
	public async Task BuscarIdPortfolioUsuarioAsync_DeveRetornarNull_QuandoPortfolioNaoExiste()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new UsuariosCommands(contexto);

		int? portfolioId = await commands.BuscarPortfolioIdDoUsuarioAsync(3);

		Assert.Null(portfolioId);
	}
}