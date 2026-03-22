using PortfolioDev.Application.DTOs.Identity;
using PortfolioDev.Domain.Models.Identity;

namespace PortfolioDev.Application.Interfaces.Builders;

public interface IUsuarioDtoBuilder
{
	Task<UsuarioDto> CriarAsync(Usuario usuario);
	Task<UsuarioDto[]> CriarVariosAsync(List<Usuario> usuarios);
	Task<UsuarioDto[]> CriarVariosAsync(IEnumerable<Usuario> usuarios);
}