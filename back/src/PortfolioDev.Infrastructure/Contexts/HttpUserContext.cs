using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PortfolioDev.Application.Interfaces.Contexts;
using PortfolioDev.Domain.Models.Identity;
using static System.Int32;

namespace PortfolioDev.Infrastructure.Contexts;

public class HttpUserContext : IHttpUserContext
{
	private readonly IHttpContextAccessor _contextAccessor;
	private readonly UserManager<Usuario> _userManager;

	public HttpUserContext(
		IHttpContextAccessor contextAccessor,
		UserManager<Usuario> userManager
	)
	{
		_contextAccessor = contextAccessor;
		_userManager = userManager;
	}

	private ClaimsPrincipal HttpUser => _contextAccessor.HttpContext.User;

	public int Id => BuscarId();
	public string UserName => BuscarUserName();
	public bool EhAdmin => VerificarAdmin();

	public async Task<Usuario?> ToUsuarioAsync()
	{
		string? id = HttpUser.FindFirstValue(ClaimTypes.NameIdentifier);
		if (id == null) return null;

		return await _userManager.FindByIdAsync(id);
	}

	private int BuscarId()
	{
		string? id = HttpUser.FindFirstValue(ClaimTypes.NameIdentifier);
		if (id == null) return 0;
		TryParse(id, out int intId);

		return intId;
	}

	private string BuscarUserName()
	{
		string? userName = HttpUser.FindFirstValue(ClaimTypes.Name);
		return userName ?? string.Empty;
	}

	private bool VerificarAdmin() { return HttpUser.IsInRole(nameof(Cargo.Admin)); }
}