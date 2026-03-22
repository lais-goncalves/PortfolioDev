using Microsoft.Extensions.DependencyInjection;
using PortfolioDev.Application.Builders;
using PortfolioDev.Application.Interfaces.Builders;
using PortfolioDev.Application.Interfaces.Services;
using PortfolioDev.Application.Interfaces.Services.Identity;
using PortfolioDev.Application.Services;
using PortfolioDev.Application.Services.Identity;

namespace PortfolioDev.Application;

public static class DependencyInjection
{
	public static void AddDIApplication(this IServiceCollection services)
	{
		services.AddScoped<IUsuarioService, UsuariosService>();
		services.AddScoped<IProjetosService, ProjetosService>();
		services.AddScoped<IPortfoliosService, PortfoliosService>();
		services.AddScoped<IContaService, ContaService>();

		services.AddScoped<IUsuarioDtoBuilder, UsuarioDtoBuilder>();
	}
}