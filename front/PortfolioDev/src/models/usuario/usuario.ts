export class Usuario {
  public id: number;
  public nomeCompleto: string;
  public email: string;
  public userName: string;
  public password: string;
  public portfolioId: number | null;

  constructor(
    id: number,
    nomeCompleto: string,
    email: string,
    userName: string,
    password: string,
    portfolioId: number | null,
  ) {
    this.id = id;
    this.nomeCompleto = nomeCompleto;
    this.email = email;
    this.userName = userName;
    this.password = password;
    this.portfolioId = portfolioId;
  }
}
