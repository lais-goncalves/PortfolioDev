namespace PortfolioDev.Domain.Models;

public class Ferramenta
{
	public int Id { get; set; }
	public string Nome { get; set; }
	public string Descricao { get; set; }
	
	public List<Projeto> Projetos { get; set; }
}