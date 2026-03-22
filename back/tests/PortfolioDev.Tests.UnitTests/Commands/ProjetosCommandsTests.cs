using PortfolioDev.Domain.Models;
using PortfolioDev.Infrastructure.Commands;
using PortfolioDev.Infrastructure.DbContexts;
using PortfolioDev.Tests.UnitTests.Fixtures;

namespace PortfolioDev.Tests.UnitTests.Commands;

public class ProjetosCommandsTests : IClassFixture<DbContextFixture>
{
	private readonly DbContextFixture _fixture;

	public ProjetosCommandsTests(DbContextFixture fixture) { _fixture = fixture; }


	#region DML
	[Fact]
	public async Task AddAsync_DeveCriarId_QuandoProjetoAdicionado()
	{
		PlataformaDevsContext contexto = await _fixture.CriarContexto();

		var projeto = new Projeto
		{
			Id = 5,
			Nome = "Projeto 5",
			Descricao = "Projeto 5",
			PortfolioId = 1
		};

		var commands = new ProjetosCommands(contexto);
		bool adicionado = await commands.AddAsync(projeto);

		Assert.True(adicionado);
	}

	[Fact]
	public async Task UpdateAsync_DeveAtualizarProjeto_QuandoProjetoExiste()
	{
		PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new ProjetosCommands(contexto);

		Projeto? projeto = await commands.BuscarProjetoPorIdAsync(1);
		Assert.NotNull(projeto);

		var descDepoisAtualizacao = "depois do update";
		projeto.Descricao = descDepoisAtualizacao;

		bool atualizou = await commands.UpdateAsync(projeto);
		Assert.True(atualizou);

		Projeto? projetoAtualizado = await commands.BuscarProjetoPorIdAsync(1);
		Assert.NotNull(projetoAtualizado);
		Assert.Equal(1, projetoAtualizado.Id);
		Assert.Equal(descDepoisAtualizacao, projetoAtualizado.Descricao);
	}

	[Fact]
	public async Task UpdateAsync_NaoDeveAtualizarProjeto_QuandoProjetoNaoExiste()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new ProjetosCommands(contexto);

		var projeto = new Projeto { Id = 99, Descricao = "teste" };

		await Assert.ThrowsAnyAsync<Exception>(async () => await commands.UpdateAsync(projeto));
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	public async Task DeleteAsync_DeveDeletarProjeto_QuandoProjetoExiste(int id)
	{
		PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new ProjetosCommands(contexto);

		bool deletado = await commands.DeleteAsync(id);
		Assert.True(deletado);

		Projeto? projetoDeletado = await commands.BuscarProjetoPorIdAsync(id);
		Assert.Null(projetoDeletado);
	}
	#endregion DML


	#region Buscas
	[Fact]
	public async Task BuscarProjetosAsync_DeveRetornarProjetos_QuandoProjetosExistem()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new ProjetosCommands(contexto);

		Projeto[] projetos = await commands.BuscarProjetosAsync();

		Assert.True(projetos.Length > 0);
	}

	[Fact]
	public async Task BuscarProjetosAsync_DeveRetornarVazio_QuandoProjetosNaoExistem()
	{
		await using PlataformaDevsContext contexto = await _fixture.CriarContexto(false);
		var commands = new ProjetosCommands(contexto);

		Projeto[] projetos = await commands.BuscarProjetosAsync();

		Assert.False(projetos.Length > 0);
		Assert.Equal([], projetos);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	public async Task BuscarProjetoPorIdAsync_DeveRetornarProjeto_QuandoProjetoExiste(int id)
	{
		PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new ProjetosCommands(contexto);

		Projeto? projeto = await commands.BuscarProjetoPorIdAsync(id);

		Assert.NotNull(projeto);
		Assert.Equal(id, projeto.Id);
	}

	[Fact]
	public async Task BuscarProjetoPorIdAsync_DeveRetornarNull_QuandoProjetoNaoExiste()
	{
		PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new ProjetosCommands(contexto);

		Projeto? projeto = await commands.BuscarProjetoPorIdAsync(99);

		Assert.Null(projeto);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	public async Task BuscarProjetosPorIdPortfolioAsync_DeveRetornarProjeto_QuandoProjetoExiste(int portfolioId)
	{
		PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new ProjetosCommands(contexto);

		Projeto[] projetos = await commands.BuscarProjetosPorIdPortfolioAsync(portfolioId);

		Assert.NotEmpty(projetos);
	}

	[Fact]
	public async Task BuscarProjetosPorIdPortfolioAsync_DeveRetornarNull_QuandoProjetoNaoExiste()
	{
		PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new ProjetosCommands(contexto);

		Projeto[] projeto = await commands.BuscarProjetosPorIdPortfolioAsync(99);

		Assert.Empty(projeto);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	public async Task BuscarProjetosPorIdPortfolioAsync_DeveRetornarVazio_QuandoNaoTemProjetos(int portfolioId)
	{
		PlataformaDevsContext contexto = await _fixture.CriarContexto(false);
		var commands = new ProjetosCommands(contexto);

		Projeto[] projeto = await commands.BuscarProjetosPorIdPortfolioAsync(portfolioId);

		Assert.Empty(projeto);
	}

	[Theory]
	[InlineData(1, 1)]
	[InlineData(3, 2)]
	public async Task ProjetoPertenceAoUsuarioAsync_DeveRetornarTrue_CasoProjetoPertencaAoUsuario(
		int projetoId,
		int usuarioId
	)
	{
		PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new ProjetosCommands(contexto);

		bool pertence = await commands.ProjetoPertenceAoUsuarioAsync(projetoId, usuarioId);

		Assert.True(pertence);
	}

	[Theory]
	[InlineData(1, 2)]
	[InlineData(3, 1)]
	public async Task ProjetoPertenceAoUsuarioAsync_DeveRetornarFalse_CasoProjetoNaoPertencaAoUsuario(
		int projetoId,
		int usuarioId
	)
	{
		PlataformaDevsContext contexto = await _fixture.CriarContexto();
		var commands = new ProjetosCommands(contexto);

		bool pertence = await commands.ProjetoPertenceAoUsuarioAsync(projetoId, usuarioId);

		Assert.False(pertence);
	}
	#endregion Buscas
}