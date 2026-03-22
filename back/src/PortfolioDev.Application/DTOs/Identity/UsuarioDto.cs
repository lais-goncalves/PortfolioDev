namespace PortfolioDev.Application.DTOs.Identity;

public struct UsuarioDto
{
	public int Id { get; set; }
	public string NomeCompleto { get; set; }
	public string UserName { get; set; }
	public int? PortfolioId { get; set; }
	public List<string> Cargos { get; set; }

	// public PortfolioDTO Portfolio { get; set; }
}