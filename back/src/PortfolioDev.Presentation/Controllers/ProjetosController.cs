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
public class ProjetosController : ControllerBase
{
	private readonly IProjetosService _projetosService;

	public ProjetosController(IProjetosService projetosService) { _projetosService = projetosService; }

	#region DML
	[Authorize(Policy = "Dev")]
    [HttpPost]
    public async Task<IActionResult> Post(ProjetoRegistroDto projetoDTO)
    {
    	try
    	{
    		ResultadoService resultado = await _projetosService.AddProjetoComAuthAsync(projetoDTO);
    		if (!resultado.Sucesso) return BadRequest(resultado.Erro);

    		var projeto = (ProjetoDto?)resultado.Dados;
    		if (projeto == null) return NoContent();

    		return Ok(projeto);
    	}
    	catch (Exception e)
    	{
    		Console.WriteLine(e.Message);
    		return BadRequest(new Erro(e));
    	}
    }

    [Authorize(Policy = "Dev")]
    [HttpPut("Id/{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProjetoAtualizacaoDto projetoDTO)
    {
    	try
    	{
    		projetoDTO.Id = id;
    		ResultadoService resultado = await _projetosService
    			.UpdateProjetoComAuthAsync(projetoDTO);

    		if (!resultado.Sucesso) return BadRequest(resultado.Erro);

    		var projeto = (ProjetoDto?)resultado.Dados;
    		if (projeto == null) return NoContent();

    		return Ok(projeto);
    	}
    	catch (Exception e)
    	{
    		Console.WriteLine(e.Message);
    		return BadRequest(new Erro(e));
    	}
    }

	[Authorize(Policy = "Dev")]
	[HttpDelete("Id/{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{
			ResultadoService resultado = await _projetosService
				.DeleteProjetoComAuthAsync(id);

			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			return Ok();
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return BadRequest(new Erro(e));
		}
	}
	#endregion DML
	
	
	#region Buscas
	[HttpGet("Todos")]
	public async Task<IActionResult> GetTodos()
	{
		try
		{
			ResultadoService resultado = await _projetosService.BuscarProjetosAsync();
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			ProjetoDto[] projetos = (ProjetoDto[]?)resultado.Dados ?? [];
			if (projetos.Length <= 0) return NoContent();

			return Ok(projetos);
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
			ResultadoService resultado = await _projetosService.BuscarProjetoPorIdAsync(id);
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			var projeto = (ProjetoDto?)resultado.Dados;
			if (projeto == null) return NoContent();

			return Ok(projeto);
		}

		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return BadRequest(new Erro(e));
		}
	}

	[HttpGet("Portfolio/{portfolioId:int}")]
	public async Task<IActionResult> GetPorIdPortfolio(int portfolioId)
	{
		try
		{
			ResultadoService resultado = await _projetosService.BuscarProjetosPorIdPortfolioAsync(portfolioId);
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			var projetos = (ProjetoDto[]?)resultado.Dados;
			if (projetos == null) return NoContent();

			return Ok(projetos);
		}

		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return BadRequest(new Erro(e));
		}
	}

	[HttpGet("Usuario/{userName}")]
	public async Task<IActionResult> GetPorUserNameUsuario(string userName)
	{
		try
		{
			ResultadoService resultado = await _projetosService
				.BuscarProjetosPorUserNameUsuarioAsync(userName);

			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			var projetos = (ProjetoDto[]?)resultado.Dados;
			if (projetos == null) return NoContent();

			return Ok(projetos);
		}

		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return BadRequest(new Erro(e));
		}
	}

	[HttpGet("Usuario")]
	public async Task<IActionResult> GetPorUsuarioAtual()
	{
		try
		{
			ResultadoService resultado = await _projetosService.BuscarProjetosDoUsuarioAsync();
			if (!resultado.Sucesso) return BadRequest(resultado.Erro);

			var projetos = (ProjetoDto[]?)resultado.Dados;
			if (projetos == null) return NoContent();

			return Ok(projetos);
		}

		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return BadRequest(new Erro(e));
		}
	}
	#endregion Buscas
}