import {Component, OnInit} from '@angular/core';
import {FormArray, FormBuilder, FormControl, ReactiveFormsModule, Validators} from '@angular/forms';
import {NgClass} from '@angular/common';
import {AuthService} from '../../../../services/auth/auth-service';
import {Registro as RegistroDto} from '../../../../models/usuario/registro/registro';
import {ToastrService} from 'ngx-toastr';
import {Form} from '../../../shared/form/form';
import {NgxSpinnerService} from 'ngx-spinner';

@Component({
  selector: 'app-registro',
  imports: [
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './registro.html',
  styleUrl: './registro.scss',
})
export class Registro extends Form implements OnInit {
  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    super();
  }

  get cargos(): any {
    return [
      {nome: 'Admin', valor: 'admin'},
      {nome: 'Dev', valor: 'dev'}
    ];
  }

  ngOnInit() {
    this.definirForm();
  }

  public definirCargosSelecionados(): void {
    var valores = this.form.value as RegistroDto;
    console.log(valores)
  }

  public registrarUsuario(): void {
    this.definirCargosSelecionados();

    if (this.form.invalid) {
      return;
    }

    // this.spinner.show();
    //
    // var valores = this.form.value as RegistroDto;
    // valores.cargos = ['Dev']; // TODO: arrumar
    //
    // this.authService.registrar(
    //   valores,
    //   (): void => {
    //     this.spinner.hide();
    //
    //     this.toastr.success(
    //       "Usuário registrado com sucesso!",
    //       "Sucesso"
    //     )
    //   },
    //   (): void => {
    //     this.spinner.hide();
    //
    //     this.toastr.error(
    //       "Não foi possível registrar usuário. Tente novamente",
    //       "Erro"
    //     )
    //   },
    // );
  }

  protected definirForm(): void {
    const cargos: any = [
      {name: 'Admin', value: 'admin'},
      {name: 'Dev', value: 'dev'}
    ];

    this.form = this.formBuilder.group({
      nomeCompleto: ['', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(60)
      ]],
      userName: ['', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20)
      ]],
      email: ['', [
        Validators.required,
        Validators.email
      ]],
      password: ['', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20)
      ]],
      cargos: new FormArray(this.cargos.map(() => new FormControl(false)))
    });
  }
}
