import {Component, OnInit} from '@angular/core';
import {Header} from './header/header';
import {Projetos} from './projetos/projetos';
import {Portfolio as PortfolioModel} from '../../../models/portfolio/portfolio';
import {BehaviorSubject, filter, map, Observable, take, tap} from 'rxjs';
import {AsyncPipe} from '@angular/common';
import {Secao} from './secao/secao';
import {ActivatedRoute, Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {NgxSpinnerService} from 'ngx-spinner';
import {PortfolioApiService} from '../../../services/portfolio/portfolio-api/portfolio-api-service';
import {PortfolioService} from '../../../services/portfolio/portfolio-service';
import {AuthService} from '../../../services/auth/auth-service';
import {UsuarioService} from '../../../services/usuario/usuario-service';
import {Usuario} from '../../../models/usuario/usuario';

export const NAO_CARREGADO = Symbol("NAO_CARREGADO");
export type EstadoUsuarioDono = Usuario | null | typeof NAO_CARREGADO;

@Component({
  selector: 'app-portfolio',
  imports: [
    Header,
    Projetos,
    AsyncPipe,
    Projetos,
    Secao
  ],
  templateUrl: './portfolio.html',
  styleUrl: './portfolio.scss',
})
export class Portfolio implements OnInit {
  private usuarioCarregouSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  private usuarioDonoSubject: BehaviorSubject<EstadoUsuarioDono | null> =
    new BehaviorSubject<EstadoUsuarioDono | null>(NAO_CARREGADO);

  public usuarioDono$: Observable<Usuario | null> =
    this.usuarioDonoSubject.asObservable().pipe(
      map((usuario) => {
        if (usuario == NAO_CARREGADO) return null;
        return usuario;
      })
    );

  constructor(
    private portfolioApiService: PortfolioApiService,
    private portfolioService: PortfolioService,
    private usuarioService: UsuarioService,
    private authService: AuthService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
  }

  get portfolio$(): Observable<PortfolioModel | null> {
    return this.portfolioService.portfolio$;
  }

  get jaCarregouPortfolio$(): Observable<boolean | null> {
    return this.portfolioService.jaCarregouPortfolio$;
  }

  get pertenceAoUsuario$(): Observable<boolean | null> {
    return this.portfolioService.pertenceAoUsuario$;
  }

  ngOnInit() {
    this.buscarParamsRota((params): void => {
      this.inicializar(params);
    });

    this.esconderCarregamento();
  }

  private inicializar(params: any): void {
    this.spinner.show();

    this.usuarioDonoSubject.next(NAO_CARREGADO);
    this.usuarioCarregouSubject.next(false);

    this.buscarUsuarioDono(params.get("username"));
    this.buscarPortfolio();
  }

  private buscarParamsRota(callback: ({}: any) => void): void {
    this
      .activatedRoute
      .paramMap
      .subscribe((params) => callback(params));
  }

  private buscarUsuarioDono(userName: string): void {
    this.usuarioService
      .buscarPorUserName(userName)
      .pipe(take(1))
      .subscribe({
        next: res => {
          this.usuarioDonoSubject.next(res as Usuario);
        },
        error: error => {
          console.log(error);
          this.usuarioDonoSubject.next(null);
        },
        complete: () => this.usuarioCarregouSubject.next(true)
      });
  }

  private buscarPortfolio(): void {
    this.usuarioDonoSubject
      .pipe(
        filter((usuario) => usuario !== NAO_CARREGADO),
        take(1)
      )
      .subscribe(usuario => {
        if (!this.usuarioDonoSubject.getValue()) {
          this.router.navigate(['/home']);
          this.spinner.hide();
          return;
        }

        this.portfolioService.inicializar(usuario as Usuario);
      });
  }

  private esconderCarregamento(): void {
    this.usuarioCarregouSubject.subscribe((carregou) => {
      if (!carregou) return;

      if (!this.usuarioDonoSubject.getValue()) {
        this.spinner.hide();
        return;
      }

      this.jaCarregouPortfolio$.subscribe({
        next: carregou => {
          if (carregou) this.spinner.hide();
        },
        error: () => this.spinner.hide()
      });
    });
  }

  protected criarPortfolio(): void {
    this.spinner.show();

    this
      .portfolioApiService
      .criarPortfolio()
      .subscribe({
        next: () => {
          this.spinner.hide();
          this.toastr.success(
            "Portfólio criado com sucesso.",
            "Sucesso!"
          );
          window.location.reload();
        },
        error: (err) => {
          console.log(err);

          this.spinner.hide();
          this.toastr.error(
            "Ocorreu um erro ao tentar criar portfólio.",
            "Erro"
          );
        }
      });
  }
}
