namespace PortfolioDev.Application.Helpers.Erros;

public class ResultadoService
{
	public object? Dados { get; set; }
	public bool Sucesso { get; set; }
	public Erro? Erro { get; set; }

	public static ResultadoService Ok(object? dados = default)
	{
		return new ResultadoService { Dados = dados, Sucesso = true };
	}

	public static ResultadoService Falhou(Exception? erro = null)
	{
		return new ResultadoService { Erro = new Erro(erro), Sucesso = false };
	}

	public static ResultadoService Falhou(Erro? erro = null)
	{
		return new ResultadoService { Erro = erro, Sucesso = false };
	}

	public static ResultadoService Falhou(string? mensagem = null, CodigoErro? codigo = null)
	{
		return new ResultadoService { Erro = new Erro(mensagem, codigo), Sucesso = false };
	}
}