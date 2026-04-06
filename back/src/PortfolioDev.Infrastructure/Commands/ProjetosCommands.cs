using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PortfolioDev.Application.Interfaces.Commands;
using PortfolioDev.Domain.Models;
using PortfolioDev.Infrastructure.DbContexts;

namespace PortfolioDev.Infrastructure.Commands;

public class ProjetosCommands : IProjetosCommands
{
	private readonly PlataformaDevsContext _contexto;

	public ProjetosCommands(PlataformaDevsContext contexto) { _contexto = contexto; }

	public async Task<bool> AddAsync(Projeto projeto)
	{
		if (projeto.PortfolioId == null || projeto.PortfolioId == 0) throw new Exception("Portfólio não existente.");

		Portfolio? portfolioExistente = await
			_contexto.Portfolios.AsNoTracking()
				.FirstOrDefaultAsync(p => p.Id == projeto.PortfolioId);

		if (portfolioExistente == null) throw new Exception("Portfólio não existente.");

		_contexto.Add(projeto);
		int resultado = await _contexto.SaveChangesAsync();

		return resultado > 0;
	}

	public async Task<bool> UpdateAsync(Projeto projeto)
	{
		Projeto? projetoExistente = await
			_contexto.Projetos.FirstOrDefaultAsync(p => p.Id == projeto.Id);

		if (projetoExistente == null) throw new Exception("Projeto não existente.");

		projeto.PortfolioId = projetoExistente.PortfolioId;

		_contexto.Update(projeto);
		int resultado = await _contexto.SaveChangesAsync();

		return resultado > 0;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		Projeto? projetoExistente = await
			_contexto.Projetos.FirstOrDefaultAsync(p => p.Id == id);

		if (projetoExistente == null) throw new Exception("Projeto não existente.");

		_contexto.Remove(projetoExistente);
		int resultado = await _contexto.SaveChangesAsync();

		return resultado > 0;
	}

	public async Task<Projeto[]> BuscarProjetosAsync()
	{
		IQueryable<Projeto> query =
			_contexto.Projetos.AsNoTracking().IgnoreAutoIncludes();

		query = query.OrderBy(p => p.Id);

		return await query.ToArrayAsync();
	}

	private IQueryable<Projeto> DefaultQuery()
	{
		IQueryable<Projeto> query = _contexto
			.Projetos
			.IgnoreAutoIncludes()
			.Include(p => p.Linguagens)
			.Include(p => p.Frameworks)
			.Include(p => p.Ferramentas);

		return query;
	}

	public async Task<Projeto?> BuscarProjetoPorIdAsync(int id)
	{
		IQueryable<Projeto> query = DefaultQuery();

		Projeto? projeto = await query.FirstOrDefaultAsync(p => p.Id == id);

		return projeto;
	}

	public async Task<Projeto[]> BuscarProjetosPorIdPortfolioAsync(int portfolioId)
	{
		IQueryable<Portfolio> queryPortfolio = _contexto
			.Portfolios
			.IgnoreAutoIncludes()
			.Include(p => p.Projetos);
		
		IQueryable<Projeto> queryProjetos = DefaultQuery();

		Projeto[] projetos = await queryPortfolio
			.Where(p => p.Id == portfolioId)
			.SelectMany(p => p.Projetos)
			.Include(p => p.Linguagens)
			.Include(p => p.Frameworks)
			.Include(p => p.Ferramentas)
			.ToArrayAsync();

		return projetos;
	}

	public async Task<Projeto[]> BuscarProjetosPorIdUsuarioAsync(int usuarioId)
	{
		IQueryable<Projeto> query = DefaultQuery();
			
		query = query
			.Include(p => p.Portfolio)
			.ThenInclude(p => p.Usuario);

		Projeto[] projetos = await query
			.Where(p => p.Portfolio.Usuario.Id == usuarioId)
			.ToArrayAsync();

		return projetos;
	}

	public async Task<Projeto[]> BuscarProjetosPorUserNameUsuarioAsync(string userName)
	{
		IQueryable<Projeto> query = DefaultQuery();
			
		query = query
			.Include(p => p.Portfolio)
			.ThenInclude(p => p.Usuario);

		Projeto[] projetos = await query
			.Where(p => p.Portfolio.Usuario.UserName == userName)
			.ToArrayAsync();

		return projetos;
	}

	public async Task<bool> ProjetoPertenceAoUsuarioAsync(int projetoId, int usuarioId)
	{
		IQueryable<Projeto> query = _contexto
			.Projetos
			.IgnoreAutoIncludes()
			.Include(p => p.Portfolio);

		bool pertence = query.Any
		(
			p => p.Id == projetoId
				&& p.Portfolio.UsuarioId == usuarioId
		);

		return pertence;
	}
}