import {Routes} from '@angular/router';
import {Auth} from './components/auth/auth';
import {Portfolio} from './components/portfolio/portfolio';
import {Login} from './components/auth/login/login';
import {Registro} from './components/auth/registro/registro';
import {AuthGuard} from '../guards/auth/auth-guard';
import {LoginGuard} from '../guards/login/login-guard';
import {Home} from './components/home/home';

export const routes: Routes = [
  {path: '', redirectTo: 'home', pathMatch: 'full'},
  {path: 'home', component: Home},
  {path: 'portfolio/:username', component: Portfolio, canActivate: [AuthGuard]},
  {path: 'auth', redirectTo: 'auth/login', pathMatch: 'full'},
  {
    path: 'auth', component: Auth, canActivate: [LoginGuard], children: [
      {path: 'login', component: Login},
      {path: 'registrar', component: Registro},
      {path: '', redirectTo: 'login', pathMatch: 'full'}
    ]
  },
];
