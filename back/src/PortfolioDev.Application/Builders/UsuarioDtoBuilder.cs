using PortfolioDev.Application.DTOs.Identity;
using PortfolioDev.Application.Interfaces.Builders;
using PortfolioDev.Application.Interfaces.Commands;
using PortfolioDev.Domain.Models.Identity;

namespace PortfolioDev.Application.Builders;

public class UsuarioDtoBuilder : IUsuarioDtoBuilder
{
	private readonly IUsuariosCommands _usuariosCommands;

	public UsuarioDtoBuilder(IUsuariosCommands usuariosCommands) { _usuariosCommands = usuariosCommands; }

	public async Task<UsuarioDto> CriarAsync(Usuario usuario)
	{
		UsuarioDto[] lista = await CriarVariosAsync([usuario]);
		return lista.First();
	}

	public async Task<UsuarioDto[]> CriarVariosAsync(List<Usuario> usuarios)
	{
		List<int> ids = usuarios.Select(u => u.Id).ToList();

		Dictionary<int, int?> portfoliosPorUsuario = await _usuariosCommands
			.BuscarPortfolioIdsPorUsuariosAsync(ids);

		Dictionary<int, IList<string>> cargosPorUsuario = await _usuariosCommands
			.BuscarCargosPorUsuariosAsync(ids);

		return usuarios.Select
			(
				u => new UsuarioDto
				{
					Id = u.Id,
					NomeCompleto = u.NomeCompleto,
					UserName = u.UserName ?? "",
					PortfolioId = portfoliosPorUsuario.GetValueOrDefault(u.Id),
					Cargos = cargosPorUsuario.GetValueOrDefault(u.Id, []).ToList()
				}
			)
			.ToArray();
	}

	public async Task<UsuarioDto[]> CriarVariosAsync(IEnumerable<Usuario> usuarios)
	{
		return await CriarVariosAsync(usuarios.ToList());
	}
}