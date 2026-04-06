namespace PortfolioDev.Domain.Models;

public class Linguagem
{
	public int Id { get; set; }
	public string Nome { get; set; }
	
	public List<Framework> Frameworks { get; set; }
	public List<Projeto> Projetos { get; set; }
}