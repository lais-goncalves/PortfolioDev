using AutoMapper;
using Moq;
using PortfolioDev.Application.DTOs;
using PortfolioDev.Application.DTOs.Atualizacao;
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
		var portfolioDto = new PortfolioRegistroDto { Descricao = "novo portfólio - descrição" };

		ResultadoService resultado = await service.AddPortfolioAsync(usuarioId, portfolioDto);

		Assert.False(resultado.Sucesso);
		Assert.NotNull(resultado.Erro);
		Assert.Null(resultado.Dados);
	}
	
	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	public async Task UpdatePortfolioAsync_DeveAtualizarPortfolio_QuandoPortfolioExiste(int usuarioId) {
		dynamic contextoEService = await DefinirContextoEService();
		IPortfoliosService service = contextoEService.Service;

		var portfolioNovoDto = new PortfolioAtualizacaoDto { Descricao = "novo portfólio - descrição" };
		
		ResultadoService resultado = await service.UpdatePortfolioAsync(usuarioId, portfolioNovoDto);
		PortfolioDto? portfolioResultado = (PortfolioDto?) resultado.Dados;
		
		Assert.True(resultado.Sucesso);
		Assert.Equal(portfolioNovoDto.Descricao, portfolioResultado?.Descricao);
	}
	
	[Fact]
	public async Task UpdatePortfolioAsync_NaoDeveAtualizarPortfolio_QuandoUsuarioNaoExiste() {
		dynamic contextoEService = await DefinirContextoEService();
		IPortfoliosService service = contextoEService.Service;

		int usuarioId = 99;
		var portfolioNovoDto = new PortfolioAtualizacaoDto { Descricao = "novo portfólio - descrição" };
		
		ResultadoService resultado = await service.UpdatePortfolioAsync(usuarioId, portfolioNovoDto);
		
		Assert.False(resultado.Sucesso);
		Assert.NotNull(resultado.Erro);
		Assert.Null(resultado.Dados);
	}
	
	[Fact]
	public async Task UpdatePortfolioAsync_NaoDeveAtualizarPortfolio_QuandoPortfolioNaoExiste() {
		dynamic contextoEService = await DefinirContextoEService();
		IPortfoliosService service = contextoEService.Service;

		int usuarioId = 3;
		var portfolioNovoDto = new PortfolioAtualizacaoDto { Descricao = "novo portfólio - descrição" };
		
		ResultadoService resultado = await service.UpdatePortfolioAsync(usuarioId, portfolioNovoDto);
		
		Assert.False(resultado.Sucesso);
		Assert.NotNull(resultado.Erro);
		Assert.Null(resultado.Dados);
	}
	
		
	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	public async Task DeletePortfolioAsync_DeveDeletarPortfolio_QuandoPortfolioExiste(int portfolioId) {
		dynamic contextoEService = await DefinirContextoEService();
		IPortfoliosService service = contextoEService.Service;

		ResultadoService resultado = await service.DeletePortfolioAsync(portfolioId);
		
		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
	}
	
	[Fact]
	public async Task BuscarPortfoliosAsync_DeveRetornarPortfolios_QuandoPortfoliosExistem() {
		dynamic contextoEService = await DefinirContextoEService();
		IPortfoliosService service = contextoEService.Service;
		
		ResultadoService resultado = await service.BuscarPortfoliosAsync();
		
		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.NotEmpty((PortfolioDto[]?) resultado.Dados ?? []);
	}
	
	[Fact]
	public async Task BuscarPortfoliosAsync_DeveRetornarVazio_QuandoPortfoliosNaoExistem() {
		dynamic contextoEService = await DefinirContextoEService(false);
		IPortfoliosService service = contextoEService.Service;
		
		ResultadoService resultado = await service.BuscarPortfoliosAsync();
		
		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.Empty((PortfolioDto[]?) resultado.Dados ?? []);
	}
	
	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	public async Task BuscarPortfolioPorIdAsync_DeveRetornarPortfolio_QuandoPortfolioExiste(int portfolioId) {
		dynamic contextoEService = await DefinirContextoEService();
		IPortfoliosService service = contextoEService.Service;
		
		ResultadoService resultado = await service.BuscarPortfolioPorIdAsync(portfolioId);
		
		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.NotNull(resultado.Dados);
	}
	
	[Fact]
	public async Task BuscarPortfolioPorIdAsync_DeveRetornarNulo_QuandoPortfolioNaoExiste() {
		dynamic contextoEService = await DefinirContextoEService(false);
		IPortfoliosService service = contextoEService.Service;
		
		ResultadoService resultado = await service.BuscarPortfolioPorIdAsync(99);
		
		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.Null(resultado.Dados);
	}
	#endregion DML
}