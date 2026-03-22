import {Component, Input} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {Usuario} from '../../../models/usuario/usuario';

@Component({
  selector: 'app-auth',
  imports: [
    RouterOutlet
  ],
  templateUrl: './auth.html',
  styleUrl: './auth.scss',
})
export class Auth {}
