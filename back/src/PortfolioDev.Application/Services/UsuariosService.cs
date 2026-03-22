using AutoMapper;
using PortfolioDev.Application.Helpers.Erros;
using PortfolioDev.Application.Interfaces.Builders;
using PortfolioDev.Application.Interfaces.Commands;
using PortfolioDev.Application.Interfaces.Services;
using PortfolioDev.Domain.Models.Identity;

namespace PortfolioDev.Application.Services;

public class UsuariosService : IUsuarioService
{
	private readonly IMapper _mapper;
	private readonly IUsuarioDtoBuilder _usuarioDtoBuilder;
	private readonly IUsuariosCommands _usuariosCommands;

	public UsuariosService(
		IUsuariosCommands usuariosCommands,
		IUsuarioDtoBuilder usuarioDtoBuilder,
		IMapper mapper
	)
	{
		_usuariosCommands = usuariosCommands;
		_usuarioDtoBuilder = usuarioDtoBuilder;
		_mapper = mapper;
	}

	public async Task<ResultadoService> BuscarUsuariosAsync()
	{
		try
		{
			Usuario[] usuarios = await _usuariosCommands.BuscarUsuariosAsync();
			return ResultadoService.Ok(await _usuarioDtoBuilder.CriarVariosAsync(usuarios));
		}

		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> BuscarUsuarioPorIdAsync(int id)
	{
		try
		{
			Usuario? usuario = await _usuariosCommands.BuscarUsuarioPorIdAsync(id);
			if (usuario == null) return ResultadoService.Ok();

			return ResultadoService.Ok(await _usuarioDtoBuilder.CriarAsync(usuario));
		}

		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> BuscarUsuarioPorUserNameAsync(string userName)
	{
		try
		{
			Usuario? usuario = await _usuariosCommands.BuscarUsuarioPorUserNameAsync(userName);
			if (usuario == null) return ResultadoService.Ok();

			return ResultadoService.Ok(await _usuarioDtoBuilder.CriarAsync(usuario));
		}

		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}
}