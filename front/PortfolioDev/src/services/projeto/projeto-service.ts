import {Injectable} from '@angular/core';
import {ProjetoApiService} from './projeto-api/projeto-api-service';
import {PortfolioService} from '../portfolio/portfolio-service';
import {BehaviorSubject, Observable} from 'rxjs';
import {Projeto} from '../../models/projeto/projeto';
import {Usuario} from '../../models/usuario/usuario';

@Injectable({
  providedIn: 'root',
})
export class ProjetoService {
  private projetosSubject: BehaviorSubject<Projeto[]> = new BehaviorSubject<Projeto[]>([]);
  public projetos$: Observable<Projeto[]> = this.projetosSubject.asObservable();

  private jaCarregouProjetosSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public jaCarregouProjetos$: Observable<boolean> = this.jaCarregouProjetosSubject.asObservable();

  constructor(
    private projetoApi: ProjetoApiService,
    private portfolioService: PortfolioService,
  ) {
  }

  public pertenceAoUsuario$(): Observable<boolean> {
    return this.portfolioService.pertenceAoUsuario$;
  }

  public inicializar(): void {
    this.buscarProjetos();
  }

  public recarregar(): void {
    this.buscarProjetos();
  }

  public buscarProjetos(): void {
    this.definirProjetosComBaseNoUsuario();
  }

  public buscarUsuarioDono(
    proximo: ((usuario: Usuario | null) => void) | null,
    erro: ((erro: any) => void) | null,
    completo: (() => void) | null,
  ): void {
    this
      .portfolioService
      .usuarioDono$.subscribe({
      next: (usuario) => {
        if (!!proximo) proximo(usuario)
      },
      error: (err) => {
        if (!!erro) erro(err)
      },
      complete: () => {
        if (!!completo) completo();
      }
    })
  }

  public definirProjetosComBaseNoUsuario(): void {
    this.jaCarregouProjetosSubject.next(false);

    this.buscarUsuarioDono(
      usuario => {
        if (!usuario || usuario.portfolioId == null) return;

        this.buscarProjetosApi(usuario?.portfolioId);
      },
      erro => {
        console.log(erro);
        this.projetosSubject.next([]);
      },
      null
    )
  }

  private buscarProjetosApi(portfolioId: number): void {
    this.jaCarregouProjetosSubject.next(false);

    if (portfolioId == null || portfolioId == 0) {
      this.projetosSubject.next([]);
      return;
    }

    this
      .projetoApi
      .buscarProjetosPorPortfolioId(portfolioId)
      .subscribe({
        next: (projetos) => {
          this.projetosSubject.next(projetos);
        },
        error: err => {
          console.log(err);
          this.projetosSubject.next([]);
        },
        complete: () => {
          this.jaCarregouProjetosSubject.next(true);
        }
      });
  }
}
