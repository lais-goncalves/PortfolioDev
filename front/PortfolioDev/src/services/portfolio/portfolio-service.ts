import {Injectable} from '@angular/core';
import {BehaviorSubject, filter, Observable, take} from 'rxjs';
import {Portfolio} from '../../models/portfolio/portfolio';
import {Usuario} from '../../models/usuario/usuario';
import {AuthService} from '../auth/auth-service';
import {UsuarioService} from '../usuario/usuario-service';
import {Router} from '@angular/router';
import {PortfolioApiService} from './portfolio-api/portfolio-api-service';

@Injectable({
  providedIn: 'root',
})
export class PortfolioService {
  private jaCarregouPortfolioSubject: BehaviorSubject<boolean> =
    new BehaviorSubject(false);
  public jaCarregouPortfolio$ = this
    .jaCarregouPortfolioSubject.asObservable();

  private portfolioSubject: BehaviorSubject<Portfolio | null> =
    new BehaviorSubject<Portfolio | null>(null);
  public portfolio$: Observable<Portfolio | null> =
    this.portfolioSubject.asObservable();

  private pertenceAoUsuarioSubject: BehaviorSubject<boolean> =
    new BehaviorSubject(false);
  public pertenceAoUsuario$: Observable<boolean> =
    this.pertenceAoUsuarioSubject.asObservable();

  private usuarioDonoSubject: BehaviorSubject<Usuario | null> =
    new BehaviorSubject<Usuario | null>(null);
  public usuarioDono$: Observable<Usuario | null> =
    this.usuarioDonoSubject.asObservable();

  private userNameUsuarioDonoSubject: BehaviorSubject<string | null> =
    new BehaviorSubject<string | null>(null);

  constructor(
    private authService: AuthService,
    private portfolioApiService: PortfolioApiService,
    private usuarioService: UsuarioService,
    private router: Router
  ) {
  }

  public inicializar(params: any): void {
    this.definirUserNameComBaseNosParams(params);
    this.buscarUsuarioDonoEPortfolio();
    this.definirSePertenceAoUsuarioAtual();
  }

  public recarregar(): void {
    this.definirPortfolio();
  }

  private definirUserNameComBaseNosParams(params: any): void {
    this.userNameUsuarioDonoSubject.next(params['username']);
  }

  private definirPortfolio(): void {
    if (!this.usuarioDonoSubject.getValue()?.portfolioId) {
      this.portfolioSubject.next(null);
      this.jaCarregouPortfolioSubject.next(true);
      return;
    }

    this
      .portfolioApiService
      .buscarPortfolioPorId(
        this.usuarioDonoSubject.getValue()?.portfolioId as number
      )
      .pipe(
        filter(p => {
          return !!p && !!p.id || this.jaCarregouPortfolioSubject.getValue();
        }),
        take(1)
      )
      .subscribe({
        next: portfolio => {
          this.portfolioSubject.next(portfolio);
        },
        error: error => {
          this.portfolioSubject.next(null);
        },
        complete: () => {
          this.jaCarregouPortfolioSubject.next(true);
        }
      });
  }

  private buscarUsuarioDonoEPortfolio(): void {
    this.usuarioService
      .buscarPorUserName(this.userNameUsuarioDonoSubject.getValue() ?? "")
      .pipe(
        filter(p => {
          return !!p || this.jaCarregouPortfolioSubject.getValue();
        }),
        take(1)
      )
      .subscribe({
        next: res => {
          if (!res) this.router.navigate(['/']);
          this.usuarioDonoSubject.next(res as Usuario);
          this.definirPortfolio();
        },
        error: error => {
          console.log(error);
          this.usuarioDonoSubject.next(null);
        }
      });
  }

  private userNameConfere(
    userName1: string,
    userName2: string
  ): boolean {
    return userName1 === userName2;
  }

  private definirSePertenceAoUsuarioAtual() {
    this.authService.usuario$.subscribe({
      next: (usuario: Usuario | null) => {
        const podeEditar = this.userNameConfere(
          usuario?.userName ?? "",
          this.userNameUsuarioDonoSubject.getValue() ?? ""
        );

        this.pertenceAoUsuarioSubject.next(podeEditar);
      },
      error: (error: Error) => {
        console.log(error);
        this.pertenceAoUsuarioSubject.next(false);
      }
    });
  }
}
