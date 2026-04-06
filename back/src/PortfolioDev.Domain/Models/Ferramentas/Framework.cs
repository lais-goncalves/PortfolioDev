namespace PortfolioDev.Domain.Models;

public class Framework
{
	public int Id { get; set; }
	public string Nome { get; set; }
	public string Descricao { get; set; }
	
	public int LinguagemId { get; set; }
	public Linguagem Linguagem { get; set; }
	
	public List<Projeto> Projetos { get; set; }
}