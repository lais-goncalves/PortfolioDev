using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PortfolioDev.Application.DTOs.Identity;
using PortfolioDev.Application.DTOs.Registro.Identity;
using PortfolioDev.Application.Helpers.Erros;
using PortfolioDev.Application.Interfaces.Builders;
using PortfolioDev.Application.Interfaces.Commands;
using PortfolioDev.Application.Interfaces.Contexts;
using PortfolioDev.Application.Interfaces.Services.Identity;
using PortfolioDev.Domain.Models.Identity;

namespace PortfolioDev.Application.Services.Identity;

public class ContaService : IContaService
{
	private readonly IHttpUserContext _httpUserContext;
	private readonly IMapper _mapper;
	private readonly SignInManager<Usuario> _signInManager;
	private readonly UserManager<Usuario> _userManager;
	private readonly IUsuarioDtoBuilder _usuarioDtoBuilder;
	private readonly IUsuariosCommands _usuariosCommands;

	public ContaService(
		IMapper mapper,
		IUsuariosCommands usuariosCommands,
		SignInManager<Usuario> signInManager,
		UserManager<Usuario> userManager,
		IHttpUserContext httpUserContext,
		IUsuarioDtoBuilder usuarioDtoBuilder
	)
	{
		_mapper = mapper;
		_usuariosCommands = usuariosCommands;
		_signInManager = signInManager;
		_userManager = userManager;
		_httpUserContext = httpUserContext;
		_usuarioDtoBuilder = usuarioDtoBuilder;
	}

	public async Task<ResultadoService> BuscarUsuarioLogado()
	{
		try
		{
			Usuario? usuario = await _httpUserContext.ToUsuarioAsync();
			if (usuario == null) return ResultadoService.Ok();

			return ResultadoService.Ok(await _usuarioDtoBuilder.CriarAsync(usuario));
		}

		catch (Exception e)
		{
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> Login(UsuarioLoginDto usuarioLoginDto, HttpContext httpContext)
	{
		try
		{
			Usuario? usuario = await VerificarLogin
			(
				usuarioLoginDto.UserNameOuEmail,
				usuarioLoginDto.Password
			);

			if (usuario == null)
				return ResultadoService.Falhou
				(
					"Usuário não encontrado.",
					CodigoErro.ITEM_NAO_ENCONTRADO
				);

			IList<string> roles = await _userManager.GetRolesAsync(usuario);
			List<Claim> claims = await CriarClaimsLogin(usuario, roles);
			var claimsIdentity = new ClaimsIdentity
			(
				claims,
				CookieAuthenticationDefaults.AuthenticationScheme
			);

			await httpContext.SignInAsync
			(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				new AuthenticationProperties()
			);

			return ResultadoService.Ok(await _usuarioDtoBuilder.CriarAsync(usuario));
		}

		catch (Exception e)
		{
			var erro = new Erro(e, CodigoErro.USUARIO_LOGIN_INVALIDO);
			return ResultadoService.Falhou(erro);
		}
	}

	public async Task<ResultadoService> Logout(HttpContext httpContext)
	{
		try
		{
			await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return ResultadoService.Ok();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ResultadoService.Falhou(e);
		}
	}

	public async Task<ResultadoService> CriarConta(
		UsuarioRegistroDto usuarioDto,
		bool permitirAdmin = false
	)
	{
		try
		{
			var usuario = _mapper.Map<Usuario>(usuarioDto);

			IdentityResult resultado = await _userManager.CreateAsync
			(
				usuario,
				usuarioDto.Password
			);

			if (!resultado.Succeeded && resultado.Errors.Any())
			{
				IdentityError error = resultado.Errors.First();
				return error.Code switch
				{
					"DuplicateUserName" => ResultadoService.Falhou
					(
						"O username já está sendo usado por outro usuário. Tente outro.",
						CodigoErro.USUARIO_DUPLICADO_USERNAME
					),
					"DuplicateEmail" => ResultadoService.Falhou
					(
						"O e-mail já está sendo usado por outro usuário. Tente outro.",
						CodigoErro.USUARIO_DUPLICADO_EMAIL
					),
					var _ => ResultadoService.Falhou
					(
						"Não foi possível criar conta. Tente novamente.",
						CodigoErro.OUTRO
					)
				};
			}

			Usuario? usuarioInserido = _userManager.FindByIdAsync(usuario.Id.ToString()).Result;
			if (usuarioInserido == null)
				return ResultadoService.Falhou
				(
					"Usuário não encontrado.",
					CodigoErro.ITEM_NAO_ENCONTRADO
				);

			if (permitirAdmin)
				await AddCargos(usuarioInserido, usuarioDto.cargos);

			else
				await AddCargosExcetoAdmin(usuarioInserido, usuarioDto.cargos);

			return ResultadoService.Ok(await _usuarioDtoBuilder.CriarAsync(usuario));
		}
		catch (Exception e)
		{
			return ResultadoService.Falhou(e);
		}
	}


	#region Utils
	private async Task AddCargosExcetoAdmin(Usuario usuario, IEnumerable<Cargo> cargos)
	{
		IEnumerable<Cargo> cargosValidos = cargos.ExcetoAdmin();
		IEnumerable<string> strCcargos = cargosValidos.ToEnumString();
		await _userManager.AddToRolesAsync(usuario, strCcargos);
	}

	private async Task AddCargos(Usuario usuario, IEnumerable<Cargo> cargos)
	{
		IEnumerable<string> strCcargos = cargos.ToEnumString();
		await _userManager.AddToRolesAsync(usuario, strCcargos);
	}

	private async Task<Usuario?> BuscarUsuario(string userNameOuEmail)
	{
		return await _userManager.FindByNameAsync(userNameOuEmail)
			?? await _userManager.FindByEmailAsync(userNameOuEmail);
	}

	private async Task<bool> ValidarSenha(Usuario usuario, string password)
	{
		SignInResult? resultado = await _signInManager.CheckPasswordSignInAsync
		(
			usuario,
			password,
			false
		);

		return resultado.Succeeded;
	}

	private async Task<Usuario?> VerificarLogin(string userNameOuEmail, string password)
	{
		if (string.IsNullOrEmpty(userNameOuEmail) || string.IsNullOrEmpty(password)) return null;

		Usuario? usuario = await BuscarUsuario(userNameOuEmail);
		if (usuario == null) throw new Exception("Usuário, Email e/ou Senha incorreto(s).");

		bool validado = await ValidarSenha(usuario, password);
		if (!validado) throw new Exception("Usuário, Email e/ou Senha incorreto(s).");

		return usuario;
	}

	private async Task<List<Claim>> CriarClaimsLogin(Usuario usuario, IList<string> roles)
	{
		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
			new(ClaimTypes.Name, usuario.UserName ?? ""),
			new(ClaimTypes.Email, usuario.Email ?? "")
		};

		foreach (string role in roles)
			if (Enum.TryParse(role, out Cargo cargo))
				claims.Add
				(
					new Claim
					(
						ClaimTypes.Role,
						cargo.ToString()
					)
				);

		return claims;
	}
	#endregion Utils
}