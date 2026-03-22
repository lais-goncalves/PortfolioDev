using PortfolioDev.Domain.Models;

namespace PortfolioDev.Application.Interfaces.Commands;

public interface IPortfoliosCommands
{
	public Task<bool> AddAsync(Portfolio portfolio);
	public Task<bool> UpdateAsync(Portfolio portfolio);
	public Task<bool> DeleteAsync(int id);
	public Task<Portfolio[]> BuscarPortfoliosAsync(bool? incluirProjetos = false);
	public Task<Portfolio?> BuscarPortfolioPorIdAsync(int id, bool? incluirProjetos = false);

	public Task<Portfolio?>
		BuscarPortfolioPorUserNameUsuarioAsync(string userName, bool? incluirProjetos = false);

	public Task<Portfolio?> BuscarPortfolioPorIdUsuarioAsync(int usuarioId, bool? incluirProjetos = false);

	public Task<bool> PortfolioPertenceAoUsuarioAsync(
		int portfolioId,
		int usuarioId
	);
}