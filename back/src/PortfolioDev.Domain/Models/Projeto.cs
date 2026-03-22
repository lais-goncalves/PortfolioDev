namespace PortfolioDev.Domain.Models;

public class Projeto
{
	public int Id { get; set; }
	public string Nome { get; set; }
	public string Descricao { get; set; }
	public int PortfolioId { get; set; }
	public Portfolio Portfolio { get; set; }
}