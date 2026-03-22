using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PortfolioDev.Domain.Models.Identity;

public class Usuario : IdentityUser<int>
{
	[StringLength(60, MinimumLength = 3)] public string NomeCompleto { get; set; }
	[NotMapped] public int PortfolioId { get; set; }
	public Portfolio Portfolio { get; set; }
	[NotMapped] public List<Cargo> Cargos { get; set; }
}