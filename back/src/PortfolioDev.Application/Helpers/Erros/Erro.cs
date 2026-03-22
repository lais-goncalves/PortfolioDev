namespace PortfolioDev.Application.Helpers.Erros;

public struct Erro
{
	public string? Mensagem { get; set; }
	public CodigoErro? CodigoErro { get; set; }

	public Erro(string? mensagem = null, CodigoErro? codigoErro = null)
	{
		Mensagem = mensagem;
		CodigoErro = codigoErro;
	}

	public Erro(Exception e, CodigoErro? codigoErro = null)
	{
		Mensagem = e.Message;
		CodigoErro = codigoErro ?? Erros.CodigoErro.OUTRO;
	}
}