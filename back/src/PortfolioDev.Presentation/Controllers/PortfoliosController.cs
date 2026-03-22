using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioDev.Application.DTOs;
using PortfolioDev.Application.DTOs.Atualizacao;
using PortfolioDev.Application.DTOs.Registro;
using PortfolioDev.Application.Helpers.Erros;
using PortfolioDev.Application.Interfaces.Services;

namespace PortfolioDev.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class PortfoliosController : ControllerBase
{
	private readonly IPortfoliosService _portfoliosService;

	public PortfoliosController(IPortfoliosService portfoliosService) { _portfoliosService = portfoliosService; }

	[HttpGet("Todos")]
	public async Task<IActionResult> GetTodos()
	{
		try
		{
			ResultadoService resultado = await _portfoliosService.BuscarPortfoliosAsync();
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			PortfolioDto[] portfolios = (PortfolioDto[]?)resultado.Dados ?? [];
			if (portfolios.Length <= 0) return NoContent();

			return Ok(portfolios);
		}

		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return BadRequest(new Erro(e));
		}
	}

	[HttpGet("Id/{id:int}")]
	public async Task<IActionResult> GetPorId(int id)
	{
		try
		{
			ResultadoService resultado = await _portfoliosService.BuscarPortfolioPorIdAsync(id, true);
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			var portfolio = (PortfolioDto?)resultado.Dados;
			if (portfolio == null) return NoContent();

			return Ok(portfolio);
		}

		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return BadRequest(new Erro(e));
		}
	}

	[HttpGet("Usuario")]
	public async Task<IActionResult> GetPorUsuarioAtual([FromQuery] bool? buscarProjetos = true)
	{
		try
		{
			ResultadoService resultado = await _portfoliosService.BuscarPortfolioDoUsuarioAsync(buscarProjetos ?? true);
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			var portfolio = (PortfolioDto?)resultado.Dados;
			if (portfolio == null) return NoContent();

			return Ok(portfolio);
		}

		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return BadRequest(new Erro(e));
		}
	}

	[HttpGet("Usuario/{userName}")]
	public async Task<IActionResult> GetPorUserNameUsuario(string userName, [FromQuery] bool? buscarProjetos = true)
	{
		try
		{
			ResultadoService resultado = await _portfoliosService
				.BuscarPortfolioPorUserNameUsuarioAsync
				(
					userName,
					buscarProjetos ?? true
				);

			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			var portfolio = (PortfolioDto?)resultado.Dados;
			if (portfolio == null) return NoContent();

			return Ok(portfolio);
		}

		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return BadRequest(new Erro(e));
		}
	}

	[Authorize(Policy = "Dev")]
	[HttpPost]
	public async Task<IActionResult> Post(PortfolioRegistroDto portfolioDto)
	{
		try
		{
			ResultadoService resultado = await _portfoliosService.AddPortfolioComAuthAsync(portfolioDto);
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			var portfolio = (PortfolioDto?)resultado.Dados;
			if (portfolio == null) return NoContent();

			return Ok(portfolio);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return BadRequest(new Erro(e));
		}
	}

	[Authorize(Policy = "Dev")]
	[HttpPut("Id/{id}:int")]
	public async Task<IActionResult> Put(int id, [FromBody] PortfolioAtualizacaoDto portfolioDto)
	{
		try
		{
			portfolioDto.Id = id;
			ResultadoService resultado = await _portfoliosService.UpdatePortfolioComAuthAsync(portfolioDto);
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			var portfolio = (PortfolioDto?)resultado.Dados;
			if (portfolio == null) return NoContent();

			return Ok(portfolio);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return BadRequest(new Erro(e));
		}
	}

	[Authorize(Policy = "Dev")]
	[HttpDelete]
	public async Task<IActionResult> Delete()
	{
		try
		{
			ResultadoService resultado = await _portfoliosService.DeletePortfolioComAuthAsync();
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			return Ok("Portfolio deletado com sucesso.");
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return BadRequest(new Erro(e));
		}
	}
}