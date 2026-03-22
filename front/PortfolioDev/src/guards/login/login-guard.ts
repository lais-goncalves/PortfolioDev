import {CanActivate, Router} from '@angular/router';
import {Injectable} from '@angular/core';
import {map, Observable, take} from 'rxjs';
import {AuthService} from '../../services/auth/auth-service';

@Injectable({providedIn: 'root'})
export class LoginGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {
  }

  canActivate(): Observable<boolean> {
    return this.authService.usuario$.pipe(
      take(1),
      map(usuario => {
        if (usuario == null) return true;
        
        this.router.navigate(['home']);
        return false;
      })
    );
  }
}
