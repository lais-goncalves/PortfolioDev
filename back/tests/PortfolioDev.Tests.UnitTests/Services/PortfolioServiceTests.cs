using AutoMapper;
using Moq;
using PortfolioDev.Application.DTOs;
using PortfolioDev.Application.DTOs.Registro;
using PortfolioDev.Application.Helpers.Erros;
using PortfolioDev.Application.Helpers.Mapper;
using PortfolioDev.Application.Interfaces.Commands;
using PortfolioDev.Application.Interfaces.Contexts;
using PortfolioDev.Application.Interfaces.Services;
using PortfolioDev.Application.Services;
using PortfolioDev.Domain.Models;
using PortfolioDev.Infrastructure.Commands;
using PortfolioDev.Infrastructure.DbContexts;
using PortfolioDev.Tests.UnitTests.Fixtures;

namespace PortfolioDev.Tests.UnitTests.Services;

public class PortfolioServiceTests : IClassFixture<DbContextFixture>
{
	private readonly DbContextFixture _fixture;
	private readonly Mock<IHttpUserContext> _httpUserContextMock;
	private readonly IMapper _mapper;

	public PortfolioServiceTests(DbContextFixture fixture)
	{
		_fixture = fixture;

		_mapper = DefinirMapper();
		_httpUserContextMock = new Mock<IHttpUserContext>();
	}

	private IMapper DefinirMapper()
	{
		return new MapperConfiguration(cfg => {
			cfg.AddProfile<PlataformaDevsProfile>();
		}).CreateMapper();
	}

	private async Task<dynamic> DefinirContextoEService(bool? comDados = true)
	{
		PlataformaDevsContext contexto = await _fixture.CriarContexto(comDados);
		IPortfoliosCommands portfoliosCommands = new PortfoliosCommands(contexto);
		IUsuariosCommands usuariosCommands = new UsuariosCommands(contexto);
		IPortfoliosService portfoliosService = new PortfoliosService
		(
			portfoliosCommands,
			usuariosCommands,
			_mapper,
			_httpUserContextMock.Object
		);

		return new
		{
			Service = portfoliosService,
			Contexto = contexto,
			PortfoliosCommands = portfoliosCommands,
			UsuariosCommands = usuariosCommands
		};
	}

	#region DML
	[Fact]
	public async Task AddPortfolioAsync_DeveCriarPortfolio_QuandoUsuarioExisteENaoTemPortfolio()
	{
		dynamic contextoEService = await DefinirContextoEService();
		IPortfoliosService service = contextoEService.Service;

		int usuarioId = 3;
		var portfolio = new Portfolio { Descricao = "novo portfólio - descrição" };
		var portfolioDto = new PortfolioRegistroDto { Descricao = "novo portfólio - descrição" };

		ResultadoService resultado = await service.AddPortfolioAsync(usuarioId, portfolioDto);

		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.NotNull(resultado.Dados);
	}
	
	// TODO: criar codigo de duplicidade
	
	[Fact]
	public async Task AddPortfolioAsync_NaoDeveCriarPortfolio_QuandoUsuarioExisteETemPortfolio()
	{
		dynamic contextoEService = await DefinirContextoEService();
		IPortfoliosService service = contextoEService.Service;

		int usuarioId = 1;
		var portfolio = new Portfolio { Descricao = "novo portfólio - descrição" };
		var portfolioDto = new PortfolioRegistroDto { Descricao = "novo portfólio - descrição" };

		ResultadoService resultado = await service.AddPortfolioAsync(usuarioId, portfolioDto);

		Assert.False(resultado.Sucesso);
		Assert.NotNull(resultado.Erro);
		Assert.Null(resultado.Dados);
	}
	
	[Fact]
	public async Task AddPortfolioAsync_NaoDeveCriarPortfolio_QuandoUsuarioNaoExiste()
	{
		dynamic contextoEService = await DefinirContextoEService();
		IPortfoliosService service = contextoEService.Service;

		int usuarioId = 99;
		var portfolio = new Portfolio { Descricao = "novo portfólio - descrição" };
		var portfolioDto = new PortfolioRegistroDto { Descricao = "novo portfólio - descrição" };

		ResultadoService resultado = await service.AddPortfolioAsync(usuarioId, portfolioDto);

		Assert.False(resultado.Sucesso);
		Assert.NotNull(resultado.Erro);
		Assert.Null(resultado.Dados);
	}
	
	[Fact]
	public async Task UpdatePortfolioAsync_DeveAtualizarPortfolio_QuandoPortfolioExiste() {
		
	}
	#endregion DML
}