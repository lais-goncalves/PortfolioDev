import {Projeto} from '../projeto/projeto';

export class Portfolio {
  public id: number;
  public descricao: string;
  public criadoEm?: Date;
  public projetos: Projeto[] = [];

  constructor(
    id: number,
    descricao: string,
    criadoEm?: Date
  ) {
    this.id = id;
    this.descricao = descricao;
    this.criadoEm = criadoEm;
  }
}
