import {Component} from '@angular/core';
import {Stacks} from '../stacks/stacks';
import {Portfolio as PortfolioModel} from '../../../../models/portfolio/portfolio';
import {Observable} from 'rxjs';
import {AsyncPipe} from '@angular/common';
import {Usuario} from '../../../../models/usuario/usuario';
import {PortfolioService} from '../../../../services/portfolio/portfolio-service';

@Component({
  selector: 'app-header',
  imports: [
    Stacks,
    AsyncPipe
  ],
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {
  constructor(
    private portfolioService: PortfolioService
  ) {
  }

  get portfolio$(): Observable<PortfolioModel | null> {
    return this.portfolioService.portfolio$;
  }

  get usuarioDono$(): Observable<Usuario | null> {
    return this.portfolioService.usuarioDono$;
  }
}
