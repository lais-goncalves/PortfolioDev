import {Component, OnInit, TemplateRef} from '@angular/core';
import {Observable} from 'rxjs';
import {Projeto} from './projeto/projeto';
import {Projeto as ProjetoModel} from '../../../../models/projeto/projeto';
import {AsyncPipe} from '@angular/common';
import {ToastrService} from 'ngx-toastr';
import {NgxSpinnerService} from 'ngx-spinner';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap/modal';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {PortfolioService} from '../../../../services/portfolio/portfolio-service';
import {ProjetoApiService} from '../../../../services/projeto/projeto-api/projeto-api-service';
import {ProjetoService} from '../../../../services/projeto/projeto-service';

@Component({
  selector: 'app-projetos',
  imports: [
    Projeto,
    AsyncPipe,
    ReactiveFormsModule
  ],
  templateUrl: './projetos.html',
  styleUrl: './projetos.scss',
})
export class Projetos implements OnInit {
  public formNovoProjeto: FormGroup = new FormGroup({});

  constructor(
    private portfolioService: PortfolioService,
    private projetoService: ProjetoService,
    private projetoApiService: ProjetoApiService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private modal: NgbModal
  ) {
  }

  get jaCarregouProjetos$(): Observable<boolean> {
    return this.projetoService.jaCarregouProjetos$;
  }

  get pertenceAoUsuario$(): Observable<boolean> {
    return this.portfolioService.pertenceAoUsuario$;
  }

  get projetos$(): Observable<ProjetoModel[]> {
    return this.projetoService.projetos$;
  }

  ngOnInit() {
    this.spinner.show();
    this.projetoService.inicializar();
    this.esconderCarregamento();
    this.definirForm();
  }

  public abrirModalNovoProjeto(content: TemplateRef<any>) {
    this.modal.open(content, {centered: true});
  }

  public adicionarProjeto(): void {
    if (this.formNovoProjeto.invalid) {
      return;
    }

    this.spinner.show();
    const projeto: ProjetoModel = this.formNovoProjeto.value as ProjetoModel;

    this
      .projetoApiService
      .adicionarProjeto(projeto)
      .subscribe({
        next: (res) => {
          this.projetoService.recarregar();
          this.modal.dismissAll();
          this.formNovoProjeto.reset();
          this.spinner.hide();
          this.toastr.success(
            "Projeto adicionado com sucesso.",
            "Sucesso!"
          );
        },
        error: err => {
          this.spinner.hide();
          this.toastr.error(
            "Ocorreu um erro ao tentar adicionar projeto. Tente novamente.",
            "Erro"
          );
        }
      });
  }

  private esconderCarregamento(): void {
    this.jaCarregouProjetos$.subscribe({
      next: jaCarregou => {
        if (jaCarregou) this.spinner.hide();
      },
      error: () => this.spinner.hide()
    });
  }

  private definirForm() {
    this.formNovoProjeto = this.formBuilder.group({
      nome: ['', Validators.required],
      descricao: [''],
      // dataCriacao: ['']
    });
  }
}
