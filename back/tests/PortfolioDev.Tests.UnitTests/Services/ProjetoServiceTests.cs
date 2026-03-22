using AutoMapper;
using Moq;
using PortfolioDev.Application.DTOs;
using PortfolioDev.Application.DTOs.Atualizacao;
using PortfolioDev.Application.DTOs.Registro;
using PortfolioDev.Application.Helpers.Erros;
using PortfolioDev.Application.Interfaces.Commands;
using PortfolioDev.Application.Interfaces.Contexts;
using PortfolioDev.Application.Interfaces.Services;
using PortfolioDev.Application.Services;
using PortfolioDev.Domain.Models;
using PortfolioDev.Infrastructure.Commands;
using PortfolioDev.Infrastructure.DbContexts;
using PortfolioDev.Tests.UnitTests.Fixtures;

namespace PortfolioDev.Tests.UnitTests.Services;

public class ProjetoServiceTests : IClassFixture<DbContextFixture>
{
	private readonly DbContextFixture _fixture;
	private readonly Mock<IHttpUserContext> _httpUserContextMock;
	private readonly Mock<IMapper> _mapperMock;

	public ProjetoServiceTests(DbContextFixture fixture)
	{
		_fixture = fixture;

		_mapperMock = new Mock<IMapper>();
		_httpUserContextMock = new Mock<IHttpUserContext>();
	}

	private async Task<dynamic> DefinirContextoEService(bool? comDados = true)
	{
		PlataformaDevsContext contexto = await _fixture.CriarContexto(comDados);
		IPortfoliosCommands portfoliosCommands = new PortfoliosCommands(contexto);
		IProjetosCommands projetosCommands = new ProjetosCommands(contexto);
		IProjetosService projetosService = new ProjetosService
		(
			portfoliosCommands,
			projetosCommands,
			_mapperMock.Object,
			_httpUserContextMock.Object
		);

		return new
		{
			Service = projetosService,
			Contexto = contexto,
			PortfoliosCommands = portfoliosCommands,
			ProjetosCommands = projetosCommands
		};
	}


	#region DML
	[Fact]
	public async Task AddProjetoAsync_DeveCriarProjeto_QuandoUsuarioEPortfolioExistem()
	{
		dynamic contextoEService = await DefinirContextoEService();
		IProjetosService service = contextoEService.Service;

		var projeto = new Projeto { Nome = "novo projeto", Descricao = "novo projeto - descrição" };
		var projetoDto = new ProjetoRegistroDto { Nome = "novo projeto", Descricao = "novo projeto - descrição" };

		_mapperMock
			.Setup(x => x.Map<Projeto>(projetoDto))
			.Returns(projeto);

		_mapperMock
			.Setup(x => x.Map<ProjetoDto?>(projeto))
			.Returns(new ProjetoDto { Nome = projetoDto.Nome, Descricao = projetoDto.Descricao });

		ResultadoService resultado = await service.AddProjetoAsync(1, projetoDto);

		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.NotNull(resultado.Dados);
	}

	[Fact]
	public async Task AddProjetoAsync_NaoDeveCriarProjeto_QuandoUsuarioNaoExiste()
	{
		dynamic contextoEService = await DefinirContextoEService();
		IProjetosService service = contextoEService.Service;

		var projeto = new Projeto { Nome = "novo projeto", Descricao = "novo projeto - descrição" };
		var projetoDto = new ProjetoRegistroDto { Nome = "novo projeto", Descricao = "novo projeto - descrição" };

		_mapperMock
			.Setup(x => x.Map<Projeto>(projetoDto))
			.Returns(projeto);

		_mapperMock
			.Setup(x => x.Map<ProjetoDto?>(projeto))
			.Returns(new ProjetoDto { Nome = projetoDto.Nome, Descricao = projetoDto.Descricao });

		ResultadoService resultado = await service.AddProjetoAsync(99, projetoDto);

		Assert.False(resultado.Sucesso);
		Assert.NotNull(resultado.Erro);
	}

	[Fact]
	public async Task UpdateProjetoAsync_DeveAtualizarProjeto_QuandoUsuarioEPortfolioExistem()
	{
		dynamic contextoEService = await DefinirContextoEService();
		IProjetosService service = contextoEService.Service;

		var projetoNovo = new Projeto
		{
			Id = 1,
			Nome = "projeto atualizado",
			Descricao = "projeto atualizado - descrição"
		};
		var projetoNovoDto = new ProjetoAtualizacaoDto
		{
			Id = projetoNovo.Id,
			Nome = projetoNovo.Nome,
			Descricao = projetoNovo.Descricao
		};

		_mapperMock
			.Setup(x => x.Map<Projeto>(It.IsAny<ProjetoAtualizacaoDto>()))
			.Returns(projetoNovo);

		_mapperMock
			.Setup(x => x.Map<ProjetoDto?>(It.IsAny<Projeto>()))
			.Returns(new ProjetoDto { Nome = projetoNovo.Nome, Descricao = projetoNovo.Descricao });

		_mapperMock
			.Setup(x => x.Map(It.IsAny<ProjetoAtualizacaoDto>(), It.IsAny<Projeto>()))
			.Callback<ProjetoAtualizacaoDto, Projeto>
			(
				(dto, projeto) =>
				{
					projeto.Nome = dto.Nome;
					projeto.Descricao = dto.Descricao;
				}
			)
			.Returns<ProjetoAtualizacaoDto, Projeto>((dto, projeto) => projeto);

		ResultadoService resultado = await service.UpdateProjetoAsync(1, projetoNovoDto);

		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.Equal(projetoNovo.Descricao, ((ProjetoDto?)resultado.Dados)?.Descricao);
	}

	[Fact]
	public async Task UpdateProjetoAsync_NaoDeveAtualizarProjeto_QuandoUsuarioNaoExiste()
	{
		dynamic contextoEService = await DefinirContextoEService();
		IProjetosService service = contextoEService.Service;

		var projetoNovo = new Projeto
		{
			Id = 1,
			Nome = "projeto atualizado",
			Descricao = "projeto atualizado - descrição"
		};
		var projetoNovoDto = new ProjetoAtualizacaoDto
		{
			Id = projetoNovo.Id,
			Nome = projetoNovo.Nome,
			Descricao = projetoNovo.Descricao
		};

		_mapperMock
			.Setup(x => x.Map<Projeto>(It.IsAny<ProjetoAtualizacaoDto>()))
			.Returns(projetoNovo);

		_mapperMock
			.Setup(x => x.Map<ProjetoDto?>(It.IsAny<Projeto>()))
			.Returns(new ProjetoDto { Nome = projetoNovo.Nome, Descricao = projetoNovo.Descricao });

		ResultadoService resultado = await service.UpdateProjetoAsync(99, projetoNovoDto);

		Assert.False(resultado.Sucesso);
		Assert.NotNull(resultado.Erro);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	public async Task DeleteProjetoAsync_DeveRetornarTrue_QuandoProjetoExiste(int id)
	{
		dynamic contextoEService = await DefinirContextoEService();
		IProjetosService service = contextoEService.Service;

		ResultadoService resultado = await service.DeleteProjetoAsync(id);

		Assert.True(resultado.Sucesso);
	}
	#endregion DML


	#region Buscas
	[Fact]
	public async Task BuscarProjetosAsync_DeveRetornarProjetos_QuandoProjetosExistem()
	{
		dynamic contextoEService = await DefinirContextoEService();
		IProjetosService service = contextoEService.Service;
		IProjetosCommands commands = contextoEService.ProjetosCommands;

		Projeto[] projetos = await commands.BuscarProjetosAsync();
		ProjetoDto[] projetoDtos = projetos.Select
			(
				p =>
				{
					return new ProjetoDto
					{
						Id = p.Id,
						Descricao = p.Descricao,
						Nome = p.Nome
					};
				}
			)
			.ToArray();

		_mapperMock
			.Setup(x => x.Map<ProjetoDto[]>(It.IsAny<Projeto[]>()))
			.Returns(projetoDtos);

		ResultadoService resultado = await service.BuscarProjetosAsync();

		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.NotEmpty((ProjetoDto[])resultado.Dados);
	}

	[Fact]
	public async Task BuscarProjetosAsync_DeveRetornarVazio_QuandoProjetosNaoExistem()
	{
		dynamic contextoEService = await DefinirContextoEService(false);
		IProjetosService service = contextoEService.Service;
		IProjetosCommands commands = contextoEService.ProjetosCommands;

		Projeto[] projetos = await commands.BuscarProjetosAsync();
		ProjetoDto[] projetoDtos = projetos.Select
			(
				p =>
				{
					return new ProjetoDto
					{
						Id = p.Id,
						Descricao = p.Descricao,
						Nome = p.Nome
					};
				}
			)
			.ToArray();

		_mapperMock
			.Setup(x => x.Map<ProjetoDto[]>(It.IsAny<Projeto[]>()))
			.Returns(projetoDtos);

		ResultadoService resultado = await service.BuscarProjetosAsync();

		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.Empty((ProjetoDto[])resultado.Dados);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	public async Task BuscarProjetosPorIdUsuarioAsync_DeveRetornarProjetos_QuandoUsuarioExiste(int usuarioId)
	{
		dynamic contextoEService = await DefinirContextoEService();
		IProjetosService service = contextoEService.Service;
		IProjetosCommands commands = contextoEService.ProjetosCommands;

		Projeto[] projetos = await commands.BuscarProjetosPorIdUsuarioAsync(usuarioId);
		ProjetoDto[] projetoDtos = projetos.Select
			(
				p =>
				{
					return new ProjetoDto
					{
						Id = p.Id,
						Descricao = p.Descricao,
						Nome = p.Nome
					};
				}
			)
			.ToArray();

		_mapperMock
			.Setup(x => x.Map<ProjetoDto[]>(It.IsAny<Projeto[]>()))
			.Returns(projetoDtos);

		ResultadoService resultado = await service.BuscarProjetosPorIdUsuarioAsync(usuarioId);

		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.NotEmpty((ProjetoDto[])resultado.Dados);
	}

	[Fact]
	public async Task BuscarProjetosPorIdUsuarioAsync_NaoDeveRetornarProjetos_QuandoUsuarioNaoExiste()
	{
		dynamic contextoEService = await DefinirContextoEService();
		IProjetosService service = contextoEService.Service;
		IProjetosCommands commands = contextoEService.ProjetosCommands;

		Projeto[] projetos = await commands.BuscarProjetosPorIdUsuarioAsync(99);
		ProjetoDto[] projetoDtos = projetos.Select
			(
				p =>
				{
					return new ProjetoDto
					{
						Id = p.Id,
						Descricao = p.Descricao,
						Nome = p.Nome
					};
				}
			)
			.ToArray();

		_mapperMock
			.Setup(x => x.Map<ProjetoDto[]>(It.IsAny<Projeto[]>()))
			.Returns(projetoDtos);

		ResultadoService resultado = await service.BuscarProjetosPorIdUsuarioAsync(99);

		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.Empty((ProjetoDto[])resultado.Dados);
	}

	[Fact]
	public async Task BuscarProjetosPorIdUsuarioAsync_NaoDeveRetornarProjetos_QuandoProjetosNaoExistem()
	{
		dynamic contextoEService = await DefinirContextoEService();
		IProjetosService service = contextoEService.Service;
		IProjetosCommands commands = contextoEService.ProjetosCommands;

		Projeto[] projetos = await commands.BuscarProjetosPorIdUsuarioAsync(3);
		ProjetoDto[] projetoDtos = projetos.Select
			(
				p =>
				{
					return new ProjetoDto
					{
						Id = p.Id,
						Descricao = p.Descricao,
						Nome = p.Nome
					};
				}
			)
			.ToArray();

		_mapperMock
			.Setup(x => x.Map<ProjetoDto[]>(It.IsAny<Projeto[]>()))
			.Returns(projetoDtos);

		ResultadoService resultado = await service.BuscarProjetosPorIdUsuarioAsync(3);

		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.Empty((ProjetoDto[])resultado.Dados);
	}

	[Theory]
	[InlineData("usuario1")]
	[InlineData("usuario2")]
	public async Task BuscarProjetosPorUserNameUsuarioAsync_DeveRetornarProjetos_QuandoUsuarioExiste(string userName)
	{
		dynamic contextoEService = await DefinirContextoEService();
		IProjetosService service = contextoEService.Service;
		IProjetosCommands commands = contextoEService.ProjetosCommands;

		Projeto[] projetos = await commands.BuscarProjetosPorUserNameUsuarioAsync(userName);
		ProjetoDto[] projetoDtos = projetos.Select
			(
				p =>
				{
					return new ProjetoDto
					{
						Id = p.Id,
						Descricao = p.Descricao,
						Nome = p.Nome
					};
				}
			)
			.ToArray();

		_mapperMock
			.Setup(x => x.Map<ProjetoDto[]>(It.IsAny<Projeto[]>()))
			.Returns(projetoDtos);

		ResultadoService resultado = await service.BuscarProjetosPorUserNameUsuarioAsync(userName);

		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.NotEmpty((ProjetoDto[])resultado.Dados);
	}

	[Fact]
	public async Task BuscarProjetosPorUserNameUsuarioAsync_NaoDeveRetornarProjetos_QuandoUsuarioNaoExiste()
	{
		dynamic contextoEService = await DefinirContextoEService();
		IProjetosService service = contextoEService.Service;
		IProjetosCommands commands = contextoEService.ProjetosCommands;

		Projeto[] projetos = await commands.BuscarProjetosPorUserNameUsuarioAsync("teste");
		ProjetoDto[] projetoDtos = projetos.Select
			(
				p =>
				{
					return new ProjetoDto
					{
						Id = p.Id,
						Descricao = p.Descricao,
						Nome = p.Nome
					};
				}
			)
			.ToArray();

		_mapperMock
			.Setup(x => x.Map<ProjetoDto[]>(It.IsAny<Projeto[]>()))
			.Returns(projetoDtos);

		ResultadoService resultado = await service.BuscarProjetosPorUserNameUsuarioAsync("teste");

		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.Empty((ProjetoDto[])resultado.Dados);
	}

	[Fact]
	public async Task BuscarProjetosPorUserNameUsuarioAsync_NaoDeveRetornarProjetos_QuandoProjetosNaoExistem()
	{
		dynamic contextoEService = await DefinirContextoEService();
		IProjetosService service = contextoEService.Service;
		IProjetosCommands commands = contextoEService.ProjetosCommands;

		Projeto[] projetos = await commands.BuscarProjetosPorUserNameUsuarioAsync("usuario3");
		ProjetoDto[] projetoDtos = projetos.Select
			(
				p =>
				{
					return new ProjetoDto
					{
						Id = p.Id,
						Descricao = p.Descricao,
						Nome = p.Nome
					};
				}
			)
			.ToArray();

		_mapperMock
			.Setup(x => x.Map<ProjetoDto[]>(It.IsAny<Projeto[]>()))
			.Returns(projetoDtos);

		ResultadoService resultado = await service.BuscarProjetosPorUserNameUsuarioAsync("usuario3");

		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.Empty((ProjetoDto[])resultado.Dados);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	public async Task BuscarProjetosPorIdPortfolioAsync_DeveRetornarProjetos_QuandoPortfolioExiste(int portfolioId)
	{
		dynamic contextoEService = await DefinirContextoEService();
		IProjetosService service = contextoEService.Service;
		IProjetosCommands commands = contextoEService.ProjetosCommands;

		Projeto[] projetos = await commands.BuscarProjetosPorIdPortfolioAsync(portfolioId);
		ProjetoDto[] projetoDtos = projetos.Select
			(
				p =>
				{
					return new ProjetoDto
					{
						Id = p.Id,
						Descricao = p.Descricao,
						Nome = p.Nome
					};
				}
			)
			.ToArray();

		_mapperMock
			.Setup(x => x.Map<ProjetoDto[]>(It.IsAny<Projeto[]>()))
			.Returns(projetoDtos);

		ResultadoService resultado = await service.BuscarProjetosPorIdPortfolioAsync(portfolioId);

		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.NotEmpty((ProjetoDto[])resultado.Dados);
	}

	[Fact]
	public async Task BuscarProjetosPorIdPortfolioAsync_NaoDeveRetornarProjetos_QuandoPortfolioNaoExiste()
	{
		dynamic contextoEService = await DefinirContextoEService();
		IProjetosService service = contextoEService.Service;
		IProjetosCommands commands = contextoEService.ProjetosCommands;

		Projeto[] projetos = await commands.BuscarProjetosPorIdPortfolioAsync(99);
		ProjetoDto[] projetoDtos = projetos.Select
			(
				p =>
				{
					return new ProjetoDto
					{
						Id = p.Id,
						Descricao = p.Descricao,
						Nome = p.Nome
					};
				}
			)
			.ToArray();

		_mapperMock
			.Setup(x => x.Map<ProjetoDto[]>(It.IsAny<Projeto[]>()))
			.Returns(projetoDtos);

		ResultadoService resultado = await service.BuscarProjetosPorIdPortfolioAsync(99);

		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.Empty((ProjetoDto[])resultado.Dados);
	}

	[Fact]
	public async Task BuscarProjetosPorIdPortfolioAsync_NaoDeveRetornarProjetos_QuandoProjetosNaoExistem()
	{
		dynamic contextoEService = await DefinirContextoEService(false);
		IProjetosService service = contextoEService.Service;
		IProjetosCommands commands = contextoEService.ProjetosCommands;

		Projeto[] projetos = await commands.BuscarProjetosPorIdPortfolioAsync(99);
		ProjetoDto[] projetoDtos = projetos.Select
			(
				p =>
				{
					return new ProjetoDto
					{
						Id = p.Id,
						Descricao = p.Descricao,
						Nome = p.Nome
					};
				}
			)
			.ToArray();

		_mapperMock
			.Setup(x => x.Map<ProjetoDto[]>(It.IsAny<Projeto[]>()))
			.Returns(projetoDtos);

		ResultadoService resultado = await service.BuscarProjetosPorIdPortfolioAsync(99);

		Assert.True(resultado.Sucesso);
		Assert.Null(resultado.Erro);
		Assert.Empty((ProjetoDto[])resultado.Dados);
	}
	#endregion Buscas
}