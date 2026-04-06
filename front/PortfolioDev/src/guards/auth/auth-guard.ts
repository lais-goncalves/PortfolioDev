import {CanActivate, Router} from '@angular/router';
import {Injectable} from '@angular/core';
import {map, Observable, take} from 'rxjs';
import {Usuario} from '../../models/usuario/usuario';
import {AuthService} from '../../services/auth/auth-service';

@Injectable({providedIn: 'root'})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {
  }

  canActivate(): Observable<boolean> {
    return this.authService.usuario$.pipe(
      take(1),
      map((usuario: Usuario | null) => {
        if (!!usuario && !!usuario.userName) return true;

        this.router.navigate(['/auth/login']);
        return false;
      })
    );
  }
}
