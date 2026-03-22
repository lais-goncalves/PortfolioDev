import {ChangeDetectorRef, Component, OnInit, signal} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {Navbar} from './shared/navbar/navbar';
import {NgxSpinnerComponent} from 'ngx-spinner';
import {AuthService} from '../services/auth/auth-service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Navbar, NgxSpinnerComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  protected readonly title = signal('PortfolioDev');

  constructor(
    private authService: AuthService,
    private changeDetectorRef: ChangeDetectorRef,
  ) {
  }

  ngOnInit() {
    this.changeDetectorRef.detectChanges();
  }
}
