using Microsoft.AspNetCore.Authentication.Cookies;
using PortfolioDev.Domain.Models.Identity;

namespace PortfolioDev.Presentation.Config;

public static class DependencyInjection
{
	private static readonly string cors = "politicaCors";

	public static void AddDICors(this IServiceCollection services)
	{
		services.AddCors
		(
			options =>
			{
				options.AddPolicy
				(
					cors,
					politica =>
					{
						politica.WithOrigins("http://localhost:4200")
							.AllowAnyHeader()
							.AllowAnyMethod()
							.AllowCredentials();

						politica.WithOrigins("https://localhost:4200")
							.AllowAnyHeader()
							.AllowAnyMethod()
							.AllowCredentials();
					}
				);
			}
		);
	}

	public static void UseDICors(this WebApplication app) { app.UseCors(cors); }

	public static void AddDICookies(this IServiceCollection services, WebApplicationBuilder builder)
	{
		services
			.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie
			(
				options =>
				{
					options.ExpireTimeSpan = TimeSpan.FromMinutes(120);

					if (builder.Environment.IsDevelopment())
					{
						options.Cookie.SameSite = SameSiteMode.Lax;
						options.Cookie.SecurePolicy = CookieSecurePolicy.None;
					}
					else
					{
						options.Cookie.SameSite = SameSiteMode.None;
						options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
					}
				}
			);

		services.ConfigureApplicationCookie
		(
			o =>
			{
				o.Events = new CookieAuthenticationEvents
				{
					OnRedirectToLogin = ctx =>
					{
						if (ctx.Request.Path.StartsWithSegments("/") && ctx.Response.StatusCode == 200)
							ctx.Response.StatusCode = 401;

						return Task.CompletedTask;
					},
					OnRedirectToAccessDenied = ctx =>
					{
						if (ctx.Request.Path.StartsWithSegments("/") && ctx.Response.StatusCode == 200)
							ctx.Response.StatusCode = 403;

						return Task.CompletedTask;
					}
				};
			}
		);
	}

	public static void AddDIPolicies(this IServiceCollection services)
	{
		services
			.AddAuthorization()
			.AddAuthorizationBuilder()
			.AddPolicy
			(
				"Admin",
				policy => { policy.RequireRole(nameof(Cargo.Admin)); }
			)
			.AddPolicy
			(
				"Dev",
				policy => { policy.RequireRole(nameof(Cargo.Dev)); }
			);
	}
}