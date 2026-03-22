using Microsoft.EntityFrameworkCore;
using PortfolioDev.Application.Interfaces.Commands;
using PortfolioDev.Domain.Models.Identity;
using PortfolioDev.Infrastructure.DbContexts;

namespace PortfolioDev.Infrastructure.Commands;

public class UsuariosCommands : IUsuariosCommands
{
	private readonly PlataformaDevsContext _contexto;

	public UsuariosCommands(PlataformaDevsContext contexto) { _contexto = contexto; }

	public async Task<Usuario[]> BuscarUsuariosAsync()
	{
		IQueryable<Usuario> query = _contexto.Usuarios.IgnoreAutoIncludes();
		query = query.OrderBy(u => u.Id);

		return await query.ToArrayAsync();
	}

	public async Task<Usuario?> BuscarUsuarioPorIdAsync(int id)
	{
		IQueryable<Usuario> query = _contexto.Usuarios.IgnoreAutoIncludes();

		Usuario? usuario = await query
			.Where(u => u.Id == id)
			.FirstOrDefaultAsync(u => u.Id == id);
		return usuario;
	}

	public async Task<Usuario?> BuscarUsuarioPorEmailAsync(string email)
	{
		IQueryable<Usuario> query = _contexto.Usuarios.IgnoreAutoIncludes();

		Usuario? usuario = await query
			.Where(u => u.Email == email)
			.FirstOrDefaultAsync(u => u.Email == email);
		return usuario;
	}

	public async Task<Usuario?> BuscarUsuarioPorUserNameAsync(string apelido)
	{
		IQueryable<Usuario> query = _contexto.Usuarios.IgnoreAutoIncludes();
		Usuario? usuario = await query.FirstOrDefaultAsync(u => u.UserName == apelido);

		return usuario;
	}

	public async Task<int?> BuscarPortfolioIdDoUsuarioAsync(int usuarioId)
	{
		IQueryable<Usuario> queryUsuario = _contexto
			.Usuarios
			.IgnoreAutoIncludes();

		bool usuarioExiste = queryUsuario.Any(u => u.Id == usuarioId);
		if (!usuarioExiste) return null;

		return await _contexto.Portfolios
			.Where(p => p.UsuarioId == usuarioId)
			.Select(p => (int?)p.Id)
			.FirstOrDefaultAsync();
	}

	public async Task<Dictionary<int, int?>> BuscarPortfolioIdsPorUsuariosAsync(List<int> usuarioIds)
	{
		return await _contexto.Portfolios
			.Where(p => usuarioIds.Contains(p.UsuarioId))
			.ToDictionaryAsync(p => p.UsuarioId, p => (int?)p.Id);
	}

	public async Task<Dictionary<int, IList<string>>> BuscarCargosPorUsuariosAsync(IList<int> usuarioIds)
	{
		return await _contexto.UserRoles
			.Where(u => usuarioIds.Contains(u.UserId))
			.Join
			(
				_contexto.Roles,
				u => u.RoleId,
				r => r.Id,
				(u, r) => new { u.UserId, r.Name }
			)

			// Agrupa os resultados por UserId
			// Cada grupo (IGrouping) terá: Key = UserId, elementos = cargos daquele usuário

			// Exemplo de grupo retornado:
			// { Key = 1,
			// [ { UserId: 1, Name: "Admin" }, { UserId: 1, Name: "Dev" } ] }
			.GroupBy(x => x.UserId)

			// Converte os grupos para um Dictionary de forma assíncrona
			// - Chave   (g.Key)    → UserId do grupo
			// - Valor   (g.Select) → lista com o Name de cada cargo do grupo
			// O cast para IList<string> é necessário para bater com o tipo de retorno
			.ToDictionaryAsync
			(
				g => g.Key,
				g => (IList<string>)g.Select(x => x.Name).ToList()
			);
	}
}