using Microsoft.EntityFrameworkCore;
using PortfolioDev.Application.Interfaces.Commands;
using PortfolioDev.Domain.Models;
using PortfolioDev.Domain.Models.Identity;
using PortfolioDev.Infrastructure.DbContexts;

namespace PortfolioDev.Infrastructure.Commands;

public class PortfoliosCommands : IPortfoliosCommands
{
	private readonly PlataformaDevsContext _contexto;

	public PortfoliosCommands(
		PlataformaDevsContext contexto
	)
	{
		_contexto = contexto;
	}

	public async Task<bool> AddAsync(Portfolio portfolio)
	{
		Usuario? usuarioDono = await _contexto
			.Usuarios
			.AsNoTracking()
			.FirstOrDefaultAsync(u => u.Id == portfolio.UsuarioId);

		if (usuarioDono == null) throw new Exception("Usuário não existente.");

		bool jaExistePortfolioCadastrado = await _contexto
			.Portfolios
			.AsNoTracking()
			.AnyAsync(p => p.UsuarioId == portfolio.UsuarioId);

		if (jaExistePortfolioCadastrado) throw new Exception("Já existe um portfólio cadastrado para este usuário.");

		portfolio.CriadoEm = DateTime.UtcNow;

		_contexto.Add(portfolio);
		int resultado = await _contexto.SaveChangesAsync();

		return resultado > 0;
	}

	public async Task<bool> UpdateAsync(Portfolio portfolio)
	{
		Portfolio? portfolioExistente = await _contexto
			.Portfolios
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Id == portfolio.Id);

		if (portfolioExistente == null) throw new Exception("Portfólio não existente.");

		portfolio.Projetos = [];
		portfolio.UsuarioId = portfolioExistente.UsuarioId;

		_contexto.Update(portfolio);
		int resultado = await _contexto.SaveChangesAsync();

		return resultado > 0;
	}

	public async Task<Portfolio[]> BuscarPortfoliosAsync(bool? incluirProjetos = false)
	{
		IQueryable<Portfolio> query = _contexto
			.Portfolios
			.AsNoTracking()
			.IgnoreAutoIncludes();

		if (incluirProjetos == true)
			query = query
				.Include(p => p.Projetos);

		Portfolio[] portfolios = await query
			.OrderBy(p => p.Id)
			.ToArrayAsync();

		return portfolios;
	}

	public async Task<Portfolio?> BuscarPortfolioPorIdAsync(int id, bool? incluirProjetos = false)
	{
		IQueryable<Portfolio> query = _contexto
			.Portfolios
			.AsNoTracking()
			.IgnoreAutoIncludes();

		if (incluirProjetos == true)
			query = query
				.Include(p => p.Projetos);

		Portfolio? portfolio = await query.FirstOrDefaultAsync(p => p.Id == id);

		return portfolio;
	}

	public async Task<Portfolio?> BuscarPortfolioPorUserNameUsuarioAsync(string userName, bool? incluirProjetos = false)
	{
		IQueryable<Portfolio> query = _contexto
			.Portfolios
			.AsNoTracking()
			.IgnoreAutoIncludes()
			.Include(p => p.Usuario);

		if (incluirProjetos == true)
			query = query
				.Include(p => p.Projetos);

		Portfolio? portfolio = await query
			.FirstOrDefaultAsync(p => p.Usuario.UserName == userName);

		return portfolio;
	}

	public async Task<Portfolio?> BuscarPortfolioPorIdUsuarioAsync(int usuarioId, bool? incluirProjetos = false)
	{
		IQueryable<Portfolio> query = _contexto
			.Portfolios
			.AsNoTracking()
			.IgnoreAutoIncludes();

		if (incluirProjetos == true)
			query = query
				.Include(p => p.Projetos);

		Portfolio? portfolio = await query
			.FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);

		return portfolio;
	}

	public async Task<bool> PortfolioPertenceAoUsuarioAsync(
		int portfolioId,
		int usuarioId
	)
	{
		IQueryable<Portfolio> query = _contexto
			.Portfolios
			.IgnoreAutoIncludes();

		bool pertence = query.Any(p => p.Id == portfolioId && p.UsuarioId == usuarioId);

		return pertence;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		Portfolio? portfolioExistente = await _contexto
			.Portfolios
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Id == id);

		if (portfolioExistente == null) throw new Exception("Portfólio não existente.");

		_contexto.Remove(portfolioExistente);
		int resultado = await _contexto.SaveChangesAsync();

		return resultado > 0;
	}
}