using PortfolioDev.Application.DTOs.Atualizacao;
using PortfolioDev.Application.DTOs.Registro;
using PortfolioDev.Application.Helpers.Erros;

namespace PortfolioDev.Application.Interfaces.Services;

public interface IPortfoliosService
{
	public Task<ResultadoService> AddPortfolioComAuthAsync(PortfolioRegistroDto portfolioDTO);
	public Task<ResultadoService> AddPortfolioAsync(int usuarioId, PortfolioRegistroDto portfolioDTO);
	public Task<ResultadoService> UpdatePortfolioComAuthAsync(PortfolioAtualizacaoDto portfolioDTO);
	public Task<ResultadoService> UpdatePortfolioAsync(int usuarioId, PortfolioAtualizacaoDto portfolioDTO);
	public Task<ResultadoService> DeletePortfolioComAuthAsync();
	public Task<ResultadoService> DeletePortfolioAsync(int id);
	
	public Task<ResultadoService> BuscarPortfoliosAsync(bool? incluirProjetos = false);
	public Task<ResultadoService> BuscarPortfolioPorIdAsync(int id, bool? incluirProjetos = false);

	public Task<ResultadoService>
		BuscarPortfolioPorUserNameUsuarioAsync(string userName, bool? incluirProjetos = false);

	public Task<ResultadoService> BuscarPortfolioDoUsuarioAsync(bool? incluirProjetos = false);
}