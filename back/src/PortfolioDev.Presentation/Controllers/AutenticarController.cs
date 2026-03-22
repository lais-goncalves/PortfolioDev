using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioDev.Application.DTOs.Identity;
using PortfolioDev.Application.DTOs.Registro.Identity;
using PortfolioDev.Application.Helpers.Erros;
using PortfolioDev.Application.Interfaces.Services.Identity;

namespace PortfolioDev.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class AutenticarController : ControllerBase
{
	private readonly IContaService _contaService;

	public AutenticarController(
		IContaService contaService
	)
	{
		_contaService = contaService;
	}

	[AllowAnonymous]
	[HttpGet]
	public async Task<IActionResult> Usuario()
	{
		try
		{
			ResultadoService resultado = await _contaService.BuscarUsuarioLogado();
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			if (resultado.Dados == null) return NoContent();

			var usuarioLogado = (UsuarioDto)resultado.Dados;

			return Ok(usuarioLogado);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return BadRequest(new Erro(e));
		}
	}

	[AllowAnonymous]
	[HttpGet]
	public async Task<IActionResult> Logout()
	{
		try
		{
			ResultadoService resultado = await _contaService.Logout(HttpContext);
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			return Ok();
		}

		catch (Exception e)
		{
			Console.WriteLine(e);
			return BadRequest(new Erro(e));
		}
	}

	[AllowAnonymous]
	[HttpPost]
	public async Task<IActionResult> Login(UsuarioLoginDto usuarioDto)
	{
		try
		{
			ResultadoService resultado = await _contaService.Login(usuarioDto, HttpContext);
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			if (resultado.Dados == null)
				return BadRequest
				(
					new Erro
					(
						"Usuário não encontrado.",
						CodigoErro.ITEM_NAO_ENCONTRADO
					)
				);

			var usuarioLogado = (UsuarioDto)resultado.Dados;
			return Ok(usuarioLogado);
		}

		catch (Exception e)
		{
			Console.WriteLine(e);
			return BadRequest(new Erro(e));
		}
	}

	[AllowAnonymous]
	[HttpPost]
	public async Task<IActionResult> Registrar(UsuarioRegistroDto usuarioDto)
	{
		try
		{
			ResultadoService resultado = await _contaService.CriarConta(usuarioDto);
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			if (resultado.Dados == null)
				return BadRequest
				(
					new Erro
					(
						"Usuário não encontrado.",
						CodigoErro.ITEM_NAO_ENCONTRADO
					)
				);

			var usuarioRegistrado = (UsuarioDto)resultado.Dados;
			return Ok(usuarioRegistrado);
		}

		catch (Exception e)
		{
			Console.WriteLine(e);
			return BadRequest(new Erro(e));
		}
	}
}