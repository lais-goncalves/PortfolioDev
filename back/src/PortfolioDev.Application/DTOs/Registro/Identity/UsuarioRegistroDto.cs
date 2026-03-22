using PortfolioDev.Domain.Models.Identity;

namespace PortfolioDev.Application.DTOs.Registro.Identity;

public struct UsuarioRegistroDto
{
	public string NomeCompleto { get; set; }
	public string UserName { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public List<Cargo> cargos { get; set; }
}