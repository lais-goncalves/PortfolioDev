using PortfolioDev.Domain.Models;

namespace PortfolioDev.Application.Interfaces.Commands;

public interface IProjetosCommands
{
	public Task<bool> AddAsync(Projeto projeto);
	public Task<bool> UpdateAsync(Projeto projeto);
	public Task<bool> DeleteAsync(int id);
	public Task<Projeto[]> BuscarProjetosAsync();
	public Task<Projeto?> BuscarProjetoPorIdAsync(int id);
	public Task<Projeto[]> BuscarProjetosPorIdPortfolioAsync(int portfolioId);
	public Task<Projeto[]> BuscarProjetosPorIdUsuarioAsync(int id);
	public Task<Projeto[]> BuscarProjetosPorUserNameUsuarioAsync(string userName);
	public Task<bool> ProjetoPertenceAoUsuarioAsync(int projetoId, int usuarioId);
}