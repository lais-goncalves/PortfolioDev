using Microsoft.EntityFrameworkCore;
using PortfolioDev.Domain.Models.Identity;

namespace PortfolioDev.Domain.Models;

[Index(nameof(UsuarioId), IsUnique = true)]
public class Portfolio
{
	public int Id { get; set; }
	public string? Descricao { get; set; }
	public DateTime CriadoEm { get; set; }
	
	public int UsuarioId { get; set; }
	public Usuario Usuario { get; set; }
	
	public List<Projeto> Projetos { get; set; }
}