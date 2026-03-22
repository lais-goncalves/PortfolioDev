import {Component, OnInit} from '@angular/core';
import {Header} from './header/header';
import {Projetos} from './projetos/projetos';
import {Portfolio as PortfolioModel} from '../../../models/portfolio/portfolio';
import {Observable} from 'rxjs';
import {AsyncPipe} from '@angular/common';
import {Secao} from './secao/secao';
import {ActivatedRoute} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {NgxSpinnerService} from 'ngx-spinner';
import {PortfolioApiService} from '../../../services/portfolio/portfolio-api/portfolio-api-service';
import {PortfolioService} from '../../../services/portfolio/portfolio-service';

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
  constructor(
    private portfolioApiService: PortfolioApiService,
    private portfolioService: PortfolioService,
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
    this.spinner.show();

    this.buscarRouteParams((params): void => {
      this.portfolioService.inicializar(params);
      this.esconderCarregamento();
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

  private esconderCarregamento(): void {
    this.jaCarregouPortfolio$.subscribe({
      next: jaCarregou => {
        if (jaCarregou) this.spinner.hide();
      },
      error: () => this.spinner.hide()
    });
  }

  private buscarRouteParams(callback: ({}: any) => void): void {
    this
      .activatedRoute
      .params
      .subscribe((params) => callback(params));
  }
}
