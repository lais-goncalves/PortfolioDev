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

public class ProjetosService : IProjetosService
{
	private readonly IHttpUserContext _httpUserContext;
	private readonly IMapper _mapper;
	private readonly IPortfoliosCommands _portfoliosCommands;
	private readonly IProjetosCommands _projetosCommands;

	public ProjetosService(
		IPortfoliosCommands portfoliosCommands,
		IProjetosCommands projetosCommands,
		IMapper mapper,
		IHttpUserContext httpUserContext
	)
	{
		_portfoliosCommands = portfoliosCommands;
		_projetosCommands = projetosCommands;
		_mapper = mapper;
		_httpUserContext = httpUserContext;
	}
	
	#region Utils
	private async Task<ResultadoService> UsuarioPodeModificarProjeto(int usuarioId, int projetoId)
	{
		bool pertenceAoUsuario = await _projetosCommands
			.ProjetoPertenceAoUsuarioAsync(projetoId, usuarioId);
    
		if (!pertenceAoUsuario)
			return ResultadoService
				.Falhou("Você não tem permissão para modificar este projeto.");
    
		return ResultadoService.Ok();
	}
	#endregion Utils

	#region DML
	public async Task<ResultadoService> AddProjetoComAuthAsync(ProjetoRegistroDto projetoDTO)
	{
		try
		{
			int usuarioId = _httpUserContext.Id;
			return await AddProjetoAsync(usuarioId, projetoDTO);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> AddProjetoAsync(int portfolioId, ProjetoRegistroDto projetoDTO)
	{
		try
		{
			Portfolio? portfolio = await _portfoliosCommands.BuscarPortfolioPorIdUsuarioAsync(portfolioId);
			if (portfolio == null)
				return ResultadoService.Falhou
				(
					"Portfólio não encontrado.",
					CodigoErro.ITEM_NAO_ENCONTRADO
				);

			var projeto = _mapper.Map<Projeto>(projetoDTO);
			projeto.PortfolioId = portfolio.Id;

			bool adicionou = await _projetosCommands.AddAsync(projeto);
			if (!adicionou)
				return ResultadoService.Falhou
				(
					"Ocorreu um erro ao tentar registrar projeto.",
					CodigoErro.REGISTRO_NAO_EFETUADO
				);

			ResultadoService resultadoProjeto = await BuscarProjetoPorIdAsync(projeto.Id);
			if (!resultadoProjeto.Sucesso || resultadoProjeto.Dados == null)
				return ResultadoService.Falhou
				(
					"Ocorreu um erro ao tentar registrar portfólio.",
					CodigoErro.REGISTRO_NAO_EFETUADO
				);

			return resultadoProjeto;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> UpdateProjetoComAuthAsync(ProjetoAtualizacaoDto projetoDTO)
	{
		try
		{
			int usuarioId = _httpUserContext.Id;

			ResultadoService resultadoPodeEditar = await UsuarioPodeModificarProjeto(usuarioId, projetoDTO.Id);
			if (!resultadoPodeEditar.Sucesso) return resultadoPodeEditar;

			return await UpdateProjetoAsync(usuarioId, projetoDTO);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> UpdateProjetoAsync(int usuarioId, ProjetoAtualizacaoDto projetoDTO)
	{
		try
		{
			int id = projetoDTO.Id;
			Projeto? projetoExistente = await _projetosCommands.BuscarProjetoPorIdAsync(id);
			if (projetoExistente == null)
				return ResultadoService.Falhou
				(
					"Projeto não encontrado.",
					CodigoErro.ITEM_NAO_ENCONTRADO
				);

			Portfolio? portfolio = await _portfoliosCommands.BuscarPortfolioPorIdUsuarioAsync(usuarioId);
			if (portfolio == null)
				return ResultadoService.Falhou
				(
					"Projeto não encontrado.",
					CodigoErro.ITEM_NAO_ENCONTRADO
				);

			Projeto projetoAtualizado = _mapper.Map(projetoDTO, projetoExistente);
			projetoAtualizado.PortfolioId = portfolio.Id;
			projetoAtualizado.Id = id;

			bool atualizou = await _projetosCommands.UpdateAsync(projetoAtualizado);
			if (!atualizou)
				return ResultadoService.Falhou
				(
					"Ocorreu um erro ao tentar atualizar projeto.",
					CodigoErro.REGISTRO_NAO_EFETUADO
				);

			ResultadoService resultadoProjeto = await BuscarProjetoPorIdAsync(id);
			if (!resultadoProjeto.Sucesso || resultadoProjeto.Dados == null)
				return ResultadoService.Falhou
				(
					"Ocorreu um erro ao tentar registrar portfólio.",
					CodigoErro.REGISTRO_NAO_EFETUADO
				);

			return resultadoProjeto;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> DeleteProjetoComAuthAsync(int id)
     	{
     		try
     		{
     			int usuarioId = _httpUserContext.Id;

				ResultadoService resultadoPodeEditar = await UsuarioPodeModificarProjeto(usuarioId, id);
				if (!resultadoPodeEditar.Sucesso) return resultadoPodeEditar;
				
     			ResultadoService resultadoDelete = await DeleteProjetoAsync(id);
     			return resultadoDelete;
     		}
     		catch (Exception e)
     		{
     			Console.WriteLine(e);
     			return ResultadoService.Falhou(e);
     		}
     	}
	
	public async Task<ResultadoService> DeleteProjetoAsync(int id)
	{
		try
		{
			bool deletado = await _projetosCommands.DeleteAsync(id);
			if (!deletado) return ResultadoService.Falhou("Ocorreu um erro ao tentar deletar projeto.");

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
	public async Task<ResultadoService> BuscarProjetosAsync()
	{
		try
		{
			Projeto[] projetos = await _projetosCommands.BuscarProjetosAsync();
			ProjetoDto[] projetosDTOs = _mapper.Map<ProjetoDto[]>(projetos);

			return ResultadoService.Ok(projetosDTOs);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> BuscarProjetoPorIdAsync(int id)
	{
		try
		{
			Projeto? projeto = await _projetosCommands.BuscarProjetoPorIdAsync(id);
			if (projeto == null) return ResultadoService.Ok();

			var projetoDTO = _mapper.Map<ProjetoDto?>(projeto);

			return ResultadoService.Ok(projetoDTO);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> BuscarProjetosPorIdUsuarioAsync(int id)
	{
		try
		{
			Projeto[] projetos = await _projetosCommands.BuscarProjetosPorIdUsuarioAsync(id);
			ProjetoDto[] projetosDTO = _mapper.Map<ProjetoDto[]>(projetos);

			return ResultadoService.Ok(projetosDTO);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> BuscarProjetosPorUserNameUsuarioAsync(string userName)
	{
		try
		{
			Projeto[] projetos = await _projetosCommands.BuscarProjetosPorUserNameUsuarioAsync(userName);
			ProjetoDto[] projetosDTO = _mapper.Map<ProjetoDto[]>(projetos);

			return ResultadoService.Ok(projetosDTO);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> BuscarProjetosDoUsuarioAsync()
	{
		try
		{
			int usuarioId = _httpUserContext.Id;
			if (usuarioId <= 0) return ResultadoService.Falhou("Usuário não existente.");

			Projeto[] projetos = await _projetosCommands.BuscarProjetosPorIdUsuarioAsync(usuarioId);
			ProjetoDto[] projetosDTO = _mapper.Map<ProjetoDto[]>(projetos);

			return ResultadoService.Ok(projetosDTO);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> BuscarProjetosPorIdPortfolioAsync(int id)
	{
		try
		{
			Projeto[] projetos = await _projetosCommands.BuscarProjetosPorIdPortfolioAsync(id);
			ProjetoDto[] projetosDTO = _mapper.Map<ProjetoDto[]>(projetos);

			return ResultadoService.Ok(projetosDTO);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}
	#endregion Buscas
}