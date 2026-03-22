using PortfolioDev.Application.DTOs.Atualizacao;
using PortfolioDev.Application.DTOs.Registro;
using PortfolioDev.Application.Helpers.Erros;

namespace PortfolioDev.Application.Interfaces.Services;

public interface IProjetosService
{
	public Task<ResultadoService> AddProjetoComAuthAsync(ProjetoRegistroDto projetoDTO);
	public Task<ResultadoService> AddProjetoAsync(int usuarioId, ProjetoRegistroDto projetoDTO);
	public Task<ResultadoService> UpdateProjetoComAuthAsync(ProjetoAtualizacaoDto projetoDTO);
	public Task<ResultadoService> UpdateProjetoAsync(int usuarioId, ProjetoAtualizacaoDto projetoDTO);
	public Task<ResultadoService> DeleteProjetoComAuthAsync(int id);
	public Task<ResultadoService> DeleteProjetoAsync(int id);
	
	public Task<ResultadoService> BuscarProjetosAsync();
	public Task<ResultadoService> BuscarProjetoPorIdAsync(int id);
	public Task<ResultadoService> BuscarProjetosPorIdPortfolioAsync(int id);
	public Task<ResultadoService> BuscarProjetosPorIdUsuarioAsync(int id);
	public Task<ResultadoService> BuscarProjetosPorUserNameUsuarioAsync(string userName);
	public Task<ResultadoService> BuscarProjetosDoUsuarioAsync();
}