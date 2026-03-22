using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioDev.Application.Interfaces.Commands;
using PortfolioDev.Application.Interfaces.Contexts;
using PortfolioDev.Domain.Models.Identity;
using PortfolioDev.Infrastructure.Commands;
using PortfolioDev.Infrastructure.Contexts;
using PortfolioDev.Infrastructure.DbContexts;

namespace PortfolioDev.Infrastructure;

public static class DependencyInjection
{
	public static void AddDIInfrastructure(this IServiceCollection services)
	{
		services.AddScoped<IUsuariosCommands, UsuariosCommands>();
		services.AddScoped<IProjetosCommands, ProjetosCommands>();
		services.AddScoped<IPortfoliosCommands, PortfoliosCommands>();

		services.AddScoped<IHttpUserContext, HttpUserContext>();
	}

	public static void AddDIDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<PlataformaDevsContext>
			(options => { options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")); });
	}

	public static void AddIdentityCore(this IServiceCollection services)
	{
		services.AddIdentityCore<Usuario>
			(
				options =>
				{
					options.Password.RequireDigit = false;
					options.Password.RequireLowercase = false;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireUppercase = false;
					options.Password.RequiredLength = 3;
					options.Password.RequiredUniqueChars = 0;

					// options.User.AllowedUserNameCharacters =
					// 	"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
					options.User.RequireUniqueEmail = true;
				}
			)
			.AddUserManager<UserManager<Usuario>>()
			.AddSignInManager()
			.AddRoles<IdentityRole<int>>()
			.AddEntityFrameworkStores<PlataformaDevsContext>();
	}
}