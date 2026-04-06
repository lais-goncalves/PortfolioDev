namespace PortfolioDev.Domain.Models;

public class Projeto
{
	public int Id { get; set; }
	public string Nome { get; set; }
	public string Descricao { get; set; }
	
	public List<Linguagem> Linguagens { get; set; }
	public List<Framework> Frameworks { get; set; }
	public List<Ferramenta> Ferramentas { get; set; }
	
	public int PortfolioId { get; set; }
	public Portfolio Portfolio { get; set; }
}