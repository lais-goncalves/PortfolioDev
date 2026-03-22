namespace PortfolioDev.Domain.Models.Identity;

public enum Cargo
{
	Admin,
	Dev,
	Recrutador
}

public static class ExtensaoCargo
{
	public static IEnumerable<Cargo> ExcetoAdmin(this IEnumerable<Cargo> cargos)
	{
		return cargos.Where(c => c != Cargo.Admin);
	}

	public static IEnumerable<string> ToEnumString(this IEnumerable<Cargo> cargos)
	{
		return cargos.Select(c => c.ToString());
	}
}