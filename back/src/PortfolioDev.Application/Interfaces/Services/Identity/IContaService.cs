using Microsoft.AspNetCore.Http;
using PortfolioDev.Application.DTOs.Identity;
using PortfolioDev.Application.DTOs.Registro.Identity;
using PortfolioDev.Application.Helpers.Erros;

namespace PortfolioDev.Application.Interfaces.Services.Identity;

public interface IContaService
{
	public Task<ResultadoService> BuscarUsuarioLogado();
	public Task<ResultadoService> Login(UsuarioLoginDto usuarioLoginDto, HttpContext httpContext);
	public Task<ResultadoService> Logout(HttpContext httpContext);
	public Task<ResultadoService> CriarConta(UsuarioRegistroDto usuarioDto, bool permitirAdmin = false);
}