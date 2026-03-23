using System.Text.Json.Serialization;
using PortfolioDev.Application;
using PortfolioDev.Infrastructure;
using PortfolioDev.Presentation.Config;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDICors();

builder.Services.AddControllers()
	.AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.AddDICookies(builder);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDIDbContext(builder.Configuration);
builder.Services.AddIdentityCore();

builder.Services.AddDIInfrastructure();
builder.Services.AddDIApplication();

builder.Services.AddOpenApi();
builder.Services.AddDIPolicies();

WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseDICors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference();
}

app.Run();