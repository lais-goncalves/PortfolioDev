using AutoMapper;
using PortfolioDev.Application.DTOs;
using PortfolioDev.Application.DTOs.Atualizacao;
using PortfolioDev.Application.DTOs.Registro;
using PortfolioDev.Application.Helpers.Erros;
using PortfolioDev.Application.Interfaces.Commands;
using PortfolioDev.Application.Interfaces.Contexts;
using PortfolioDev.Application.Interfaces.Services;
using PortfolioDev.Domain.Models;

namespace PortfolioDev.Application.Services;

public class PortfoliosService : IPortfoliosService
{
	private readonly IHttpUserContext _httpUserContext;
	private readonly IMapper _mapper;
	private readonly IPortfoliosCommands _portfoliosCommands;
	private readonly IUsuariosCommands _usuariosCommands;

	public PortfoliosService(
		IPortfoliosCommands portfoliosCommands,
		IUsuariosCommands usuariosCommands,
		IMapper mapper,
		IHttpUserContext httpUserContext
	)
	{
		_portfoliosCommands = portfoliosCommands;
		_usuariosCommands = usuariosCommands;
		_mapper = mapper;
		_httpUserContext = httpUserContext;
	}

	#region Utils
	private async Task<ResultadoService> UsuarioPodeModificarPortfolio(int usuarioId, int portfolioId)
    	{
    		// TODO: criar codigo de erro para falta de permissao
    		int? id = await _usuariosCommands.BuscarPortfolioIdDoUsuarioAsync(usuarioId);
    		if (id == null)
    			return ResultadoService
    				.Falhou("Portfólio não existente.");
    
    		bool pertenceAoUsuario = await _portfoliosCommands
    			.PortfolioPertenceAoUsuarioAsync((int)id, usuarioId);
    
    		if (!pertenceAoUsuario)
    			return ResultadoService
    				.Falhou("Você não tem permissão para modificar este portfólio.");
    
    		return ResultadoService.Ok();
    	}
	#endregion Utils
	
	
	#region DML
	public async Task<ResultadoService> AddPortfolioComAuthAsync(PortfolioRegistroDto portfolioDTO)
	{
		try
		{
			int usuarioId = _httpUserContext.Id;
			return await AddPortfolioAsync(usuarioId, portfolioDTO);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> AddPortfolioAsync(int usuarioId, PortfolioRegistroDto portfolioDTO)
	{
		try
		{
			var portfolio = _mapper.Map<Portfolio>(portfolioDTO);
			portfolio.UsuarioId = usuarioId;

			bool adicionou = await _portfoliosCommands.AddAsync(portfolio);
			if (!adicionou)
				return ResultadoService.Falhou
				(
					"Ocorreu um erro ao tentar registrar portfólio.",
					CodigoErro.REGISTRO_NAO_EFETUADO
				);

			ResultadoService resultadoPortfolio = await BuscarPortfolioPorIdAsync(portfolio.Id);
			if (!resultadoPortfolio.Sucesso || resultadoPortfolio.Dados == null)
				return ResultadoService.Falhou
				(
					"Ocorreu um erro ao tentar registrar portfólio.",
					CodigoErro.REGISTRO_NAO_EFETUADO
				);

			return resultadoPortfolio;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> UpdatePortfolioComAuthAsync(PortfolioAtualizacaoDto portfolioDTO)
	{
		try
		{
			int usuarioId = _httpUserContext.Id;

			ResultadoService resultadoPodeEditar = await UsuarioPodeModificarPortfolio(usuarioId, portfolioDTO.Id);
			if (!resultadoPodeEditar.Sucesso) return resultadoPodeEditar;

			return await UpdatePortfolioAsync(usuarioId, portfolioDTO);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> UpdatePortfolioAsync(int usuarioId, PortfolioAtualizacaoDto portfolioDTO)
	{
		try
		{
			Portfolio? portfolioExistente = await _portfoliosCommands
				.BuscarPortfolioPorIdUsuarioAsync(usuarioId);

			if (portfolioExistente == null) throw new Exception("Portfólio não existente.");

			Portfolio portfolioASerAtualizado = _mapper.Map(portfolioDTO, portfolioExistente);
			bool atualizou = await _portfoliosCommands.UpdateAsync(portfolioASerAtualizado);
			if (!atualizou) throw new Exception("Houve um erro ao tentar atualizar portfólio.");

			ResultadoService resultadoPortfolio = await BuscarPortfolioPorIdAsync(portfolioExistente.Id);
			if (!resultadoPortfolio.Sucesso || resultadoPortfolio.Dados == null)
				return ResultadoService.Falhou
				(
					"Ocorreu um erro ao tentar atualizar portfólio.",
					CodigoErro.UPDATE_NAO_EFETUADO
				);

			return resultadoPortfolio;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> DeletePortfolioComAuthAsync()
	{
		try
		{
			int usuarioId = _httpUserContext.Id;

			int? portfolioId = await _usuariosCommands.BuscarPortfolioIdDoUsuarioAsync(usuarioId);
			if (portfolioId == null)
				return ResultadoService
					.Falhou("Portfólio não existente.");

			ResultadoService resultadoPodeEditar = await UsuarioPodeModificarPortfolio(usuarioId, (int)portfolioId);
			if (!resultadoPodeEditar.Sucesso) return resultadoPodeEditar;

			ResultadoService resultado = await DeletePortfolioAsync((int)portfolioId);
			if (!resultado.Sucesso)
				return ResultadoService
					.Falhou("Ocorreu um erro ao tentar deletar portfólio.");

			return new ResultadoService();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> DeletePortfolioAsync(int id)
	{
		try
		{
			bool deletou = await _portfoliosCommands.DeleteAsync(id);
			if (!deletou)
				return ResultadoService.Falhou
				(
					"Ocorreu um erro ao tentar deletar portfólio.",
					CodigoErro.DELETE_NAO_EFETUADO
				);

			return ResultadoService.Ok();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}
	#endregion DML
	
	
	#region Buscas
	public async Task<ResultadoService> BuscarPortfoliosAsync(bool? incluirProjetos = false)
	{
		try
		{
			Portfolio[] portfolios = await _portfoliosCommands.BuscarPortfoliosAsync(incluirProjetos);
			PortfolioDto[] portfoliosDTOs = _mapper.Map<PortfolioDto[]>(portfolios);

			return ResultadoService.Ok(portfoliosDTOs);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> BuscarPortfolioPorIdAsync(int id, bool? incluirProjetos = false)
	{
		try
		{
			Portfolio? portfolio = await _portfoliosCommands.BuscarPortfolioPorIdAsync(id, incluirProjetos);
			if (portfolio == null) return ResultadoService.Ok();

			var portfolioDTO = _mapper.Map<PortfolioDto?>(portfolio);

			return ResultadoService.Ok(portfolioDTO);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> BuscarPortfolioPorUserNameUsuarioAsync(
		string userName,
		bool? incluirProjetos = false
	)
	{
		try
		{
			Portfolio? portfolio = await _portfoliosCommands
				.BuscarPortfolioPorUserNameUsuarioAsync
				(
					userName,
					incluirProjetos
				);

			if (portfolio == null) return ResultadoService.Ok();
			var portfolioDTOs = _mapper.Map<PortfolioDto>(portfolio);

			return ResultadoService.Ok(portfolioDTOs);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> BuscarPortfolioDoUsuarioAsync(bool? incluirProjetos = false)
	{
		try
		{
			int usuarioId = _httpUserContext.Id;
			if (usuarioId <= 0) return ResultadoService.Falhou("Usuário não existente.");

			Portfolio? portfolio = await _portfoliosCommands.BuscarPortfolioPorIdUsuarioAsync
				(usuarioId, incluirProjetos);
			if (portfolio == null) return ResultadoService.Ok();

			var portfolioDTOs = _mapper.Map<PortfolioDto>(portfolio);

			return ResultadoService.Ok(portfolioDTOs);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}
	#endregion Buscas
}