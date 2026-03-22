export class Projeto {
  public id: number | null;
  public nome: string | null;
  public descricao?: string | null;

  constructor(
    id: number | null,
    nome: string | null,
    descricao?: string | null
  ) {
    this.id = id;
    this.nome = nome;
    this.descricao = descricao;
  }
}
