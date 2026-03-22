namespace PortfolioDev.Application.DTOs.Identity;

public struct UsuarioLoginDto
{
	public string UserNameOuEmail { get; set; }
	public string Password { get; set; }
}