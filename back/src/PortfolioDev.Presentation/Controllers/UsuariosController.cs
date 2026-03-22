using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioDev.Application.DTOs.Identity;
using PortfolioDev.Application.Helpers.Erros;
using PortfolioDev.Application.Interfaces.Services;

namespace PortfolioDev.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class UsuariosController : ControllerBase
{
	private readonly IUsuarioService _usuariosService;

	public UsuariosController(IUsuarioService usuariosService) { _usuariosService = usuariosService; }

	[AllowAnonymous]
	[HttpGet("Todos")]
	public async Task<IActionResult> GetTodos()
	{
		try
		{
			ResultadoService resultado = await _usuariosService.BuscarUsuariosAsync();
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			UsuarioDto[] usuarios = (UsuarioDto[])resultado.Dados ?? [];
			if (usuarios.Length <= 0) return NoContent();

			return Ok(usuarios);
		}
		catch (Exception ex)
		{
			return BadRequest($"Ocorreu um erro ao tentar buscar usuários. Tente novamente. Erro: {ex.Message}");
		}
	}

	[AllowAnonymous]
	[HttpGet("Id/{id:int}")]
	public async Task<IActionResult> GetPorId(int id)
	{
		try
		{
			ResultadoService resultado = await _usuariosService.BuscarUsuarioPorIdAsync(id);
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			var usuario = (UsuarioDto?)resultado.Dados;
			if (usuario == null) return NoContent();

			return Ok(usuario);
		}
		catch (Exception ex)
		{
			return BadRequest($"Ocorreu um erro ao tentar buscar usuário. Tente novamente. Erro: {ex.Message}");
		}
	}

	[AllowAnonymous]
	[HttpGet("UserName/{userName}")]
	public async Task<IActionResult> GetPorUserName(string userName)
	{
		try
		{
			ResultadoService resultado = await _usuariosService.BuscarUsuarioPorUserNameAsync(userName);
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			var usuario = (UsuarioDto?)resultado.Dados;
			if (usuario == null) return NoContent();

			return Ok(usuario);
		}
		catch (Exception ex)
		{
			return BadRequest($"Ocorreu um erro ao tentar buscar usuário. Tente novamente. Erro: {ex.Message}");
		}
	}
}