using PortfolioDev.Application.DTOs.Identity;
using PortfolioDev.Application.Helpers.Erros;

namespace PortfolioDev.Application.Interfaces.Services;

public interface IUsuarioService
{
	public Task<ResultadoService> BuscarUsuariosAsync();
	public Task<ResultadoService> BuscarUsuarioPorIdAsync(int id);
	public Task<ResultadoService> BuscarUsuarioPorUserNameAsync(string userName);
}