export class Registro {
  nomeCompleto: string;
  userName: string;
  email: string;
  password: string;
  cargos: Array<string>;

  constructor(
    nomeCompleto: string,
    userName: string,
    email: string,
    password: string,
    cargos: Array<string>
  ) {
    this.nomeCompleto = nomeCompleto;
    this.userName = userName;
    this.email = email;
    this.password = password;
    this.cargos = cargos;
  }
}
