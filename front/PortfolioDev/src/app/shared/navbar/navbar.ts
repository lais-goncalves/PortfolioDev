import {Component} from '@angular/core';
import {Router, RouterLink, RouterLinkActive} from '@angular/router';
import {AuthService} from '../../../services/auth/auth-service';
import {Observable} from 'rxjs';
import {Usuario} from '../../../models/usuario/usuario';
import {AsyncPipe} from '@angular/common';
import {NgbDropdown, NgbDropdownItem, NgbDropdownMenu, NgbDropdownToggle} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-navbar',
  imports: [
    RouterLink,
    RouterLinkActive,
    AsyncPipe,
    NgbDropdown,
    NgbDropdownToggle,
    NgbDropdownMenu,
    NgbDropdownItem
  ],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {
  }

  get usuario$(): Observable<Usuario | null> {
    return this.authService.usuario$;
  }

  get estaLogado$(): Observable<boolean> {
    return this.authService.estaLogado$;
  }

  public logout(): void {
    this.authService.deslogar(() => {
      this.router.navigate(['/auth/login']);
    });
  }
}
