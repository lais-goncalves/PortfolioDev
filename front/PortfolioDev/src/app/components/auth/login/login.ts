import {Component, Input, OnInit} from '@angular/core';
import {Form} from '../../../shared/form/form';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {ToastrService} from 'ngx-toastr';
import {AuthService} from '../../../../services/auth/auth-service';
import {Login as LoginDto} from '../../../../models/usuario/login/login';
import {Usuario} from '../../../../models/usuario/usuario';
import {NgxSpinnerService} from 'ngx-spinner';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login extends Form implements OnInit {
  @Input() usuario: Usuario | null = null;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
  ) {
    super();
  }

   ngOnInit() {
    this.definirForm();
  }

  public logarUsuario(): void {
    if (this.form.invalid) return;

    var valores: LoginDto = {
      userNameOuEmail: this.form.controls['userName'].value,
      password: this.form.controls['password'].value,
    }

    this.spinner.show();

    this.authService.logar(
      valores,
      (res: any): void => {
        this.spinner.hide();

        this.toastr.success(
          "Usuário logado com sucesso!",
          "Sucesso")
      },
      (): void => {
        this.spinner.hide();

        this.toastr.error(
          "Não foi possível logar usuário. Tente novamente",
          "Erro")
      },
      () => window.location.reload()
      );
  }

  protected definirForm(): void {
    this.form = this.formBuilder.group({
      userName: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }
}
