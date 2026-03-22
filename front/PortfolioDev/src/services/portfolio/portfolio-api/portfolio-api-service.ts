import {Injectable} from '@angular/core';
import {HttpService} from '../../http/http-service';
import {map, Observable} from 'rxjs';
import {Portfolio} from '../../../models/portfolio/portfolio';

@Injectable({
  providedIn: 'root',
})
export class PortfolioApiService {
  constructor(private httpService: HttpService) {
  }

  public buscarPortfolioUsuarioAtual(
    buscarProjetos: boolean = false
  ): Observable<Portfolio | null> {
    const params = new URLSearchParams({
      buscarProjetos: buscarProjetos.toString(),
    });

    return this
      .httpService
      .enviarGET(
        `/api/Portfolios/Usuario?${params.toString()}`,
        {withCredentials: true}
      )
      .pipe(map((p: any) => {
        const portfolio = p as Portfolio;
        return !!portfolio ? portfolio : null;
      }));
  }

  public buscarPortfolioPorId(
    id: number,
    buscarProjetos: boolean = false
  ): Observable<Portfolio | null> {
    const params = new URLSearchParams({
      buscarProjetos: buscarProjetos.toString(),
    });

    return this
      .httpService
      .enviarGET(
        `/api/Portfolios/Id/${id}?${params.toString()}`,
        {withCredentials: true}
      )
      .pipe(map((p: any) => {
        const portfolio = p as Portfolio;
        return !!portfolio ? portfolio : null;
      }));
  }

  public buscarPortfolioPorUserNameUsuario(
    userName: string,
    buscarProjetos: boolean = false
  ): Observable<Portfolio | null> {
    const params = new URLSearchParams({
      buscarProjetos: buscarProjetos.toString(),
    });

    return this
      .httpService
      .enviarGET(
        `/api/Portfolios/Usuario/${userName}?${params.toString()}`,
        {withCredentials: true}
      )
      .pipe(map((p: any) => {
        const portfolio = p as Portfolio;
        return !!portfolio ? portfolio : null;
      }));
  }

  public criarPortfolio(): Observable<Portfolio | null> {
    const portfolio: Portfolio = {
      descricao: ""
    } as Portfolio;

    return this
      .httpService
      .enviarPOST(
        '/api/Portfolios',
        portfolio,
        {withCredentials: true}
      )
      .pipe(map((p: any) => {
        const portfolio = p as Portfolio;
        return !!portfolio ? portfolio : null;
      }));
  }
}
