import {Injectable} from '@angular/core';
import {HttpService} from '../../http/http-service';
import {Projeto} from '../../../models/projeto/projeto';
import {map, Observable} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProjetoApiService {
  constructor(private httpService: HttpService) {
  }

  public buscarProjetosPorPortfolioId(portfolioId: number): Observable<Array<Projeto>> {
    return this
      .httpService
      .enviarGET(
        `/api/Projetos/Portfolio/${portfolioId}`,
        {withCredentials: true}
      )
      .pipe(map((p: any) => {
        const projetos = p as Projeto[];
        return !!projetos ? projetos : [];
      }));
  }

  public buscarProjetosUsuarioAtual(): Observable<Array<Projeto>> {
    return this
      .httpService
      .enviarGET(
        '/api/Projetos/Usuario',
        {withCredentials: true}
      )
      .pipe(map((p: any) => {
        const projetos = p as Projeto[];
        return !!projetos ? projetos : [];
      }));
  }

  // public buscarProjetosPorUserNameUsuario(userName: string): Observable<Array<Projeto>> {
  //   return this
  //     .httpService
  //     .enviarGET(
  //       `/api/Projetos/Usuario/${userName}`,
  //       {withCredentials: true}
  //     )
  //     .pipe(map((p: any) => {
  //       const projetos = p as Projeto[];
  //       return !!projetos ? projetos : [];
  //     }));
  // }

  public adicionarProjeto(projeto: Projeto): Observable<any> {
    return this
      .httpService
      .enviarPOST(
        '/api/Projetos',
        projeto,
        {withCredentials: true}
      );
  }

  public editarProjeto(id: number, projeto: Projeto): Observable<any | null> {
    return this
      .httpService
      .enviarPUT(
        `/api/Projetos/Id/${id}`,
        projeto,
        {withCredentials: true}
      );
  }

  public deletarProjeto(id: number): Observable<any | null> {
      return this
        .httpService
        .enviarDELETE(
          `/api/Projetos/Id/${id}`,
          {withCredentials: true}
        );
  }
}
