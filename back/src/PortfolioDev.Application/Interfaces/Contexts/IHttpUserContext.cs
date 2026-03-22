using PortfolioDev.Domain.Models.Identity;

namespace PortfolioDev.Application.Interfaces.Contexts;

public interface IHttpUserContext
{
	public int Id { get; }
	public string UserName { get; }
	public bool EhAdmin { get; }

	public Task<Usuario?> ToUsuarioAsync();
}