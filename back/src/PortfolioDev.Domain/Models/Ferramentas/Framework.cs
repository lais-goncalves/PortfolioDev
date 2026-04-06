namespace PortfolioDev.Domain.Models;

public class Framework
{
	public int Id { get; set; }
	public string Nome { get; set; }
	public string Descricao { get; set; }
	public List<Linguagem> Linguagens { get; set; }
	public List<Projeto> Projetos { get; set; }
}