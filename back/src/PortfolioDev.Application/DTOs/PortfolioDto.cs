namespace PortfolioDev.Application.DTOs;

public struct PortfolioDto
{
	public int Id { get; set; }
	public string? Descricao { get; set; }
	public string CriadoEm { get; set; }

	public List<ProjetoDto> Projetos { get; set; }
}