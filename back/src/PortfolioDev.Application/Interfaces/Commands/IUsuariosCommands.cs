using PortfolioDev.Domain.Models.Identity;

namespace PortfolioDev.Application.Interfaces.Commands;

public interface IUsuariosCommands
{
	public Task<Usuario[]> BuscarUsuariosAsync();
	public Task<Usuario> BuscarUsuarioPorIdAsync(int id);
	public Task<Usuario?> BuscarUsuarioPorEmailAsync(string email);
	public Task<Usuario?> BuscarUsuarioPorUserNameAsync(string apelido);
	public Task<int?> BuscarPortfolioIdDoUsuarioAsync(int usuarioId);
	public Task<Dictionary<int, int?>> BuscarPortfolioIdsPorUsuariosAsync(List<int> usuarioIds);
	public Task<Dictionary<int, IList<string>>> BuscarCargosPorUsuariosAsync(IList<int> usuarioIds);
}