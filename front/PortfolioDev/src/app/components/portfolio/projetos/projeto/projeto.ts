import {Component, Input, TemplateRef} from '@angular/core';
import {Projeto as ProjetoModel} from '../../../../../models/projeto/projeto';
import {Observable} from 'rxjs';
import {AsyncPipe} from '@angular/common';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {PortfolioService} from '../../../../../services/portfolio/portfolio-service';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap/modal';
import {ProjetoService} from '../../../../../services/projeto/projeto-service';
import {ProjetoApiService} from '../../../../../services/projeto/projeto-api/projeto-api-service';
import {NgxSpinnerService} from 'ngx-spinner';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-projeto',
  imports: [
    AsyncPipe,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './projeto.html',
  styleUrl: './projeto.scss',
})
export class Projeto {
  @Input() projeto: ProjetoModel = {} as ProjetoModel;

  protected formEdicao: FormGroup = new FormGroup({});

  constructor(
    private portfolioService: PortfolioService,
    private projetoService: ProjetoService,
    private projetoApiService: ProjetoApiService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private modal: NgbModal,
    private formBuilder: FormBuilder,
  ) {
  }

  get pertenceAoUsuario$(): Observable<boolean> {
    return this.portfolioService.pertenceAoUsuario$;
  }

  public abrirModal(content: TemplateRef<any>) {
    this.definirForm();
    this.modal.open(content, {centered: true});
  }

  public editar(): void {
    if (this.formEdicao.invalid || this.projeto.id == null) return;

    this.spinner.show();
    const projeto: ProjetoModel = this.formEdicao.value as ProjetoModel;

    this
      .projetoApiService
      .editarProjeto(this.projeto.id, projeto)
      .subscribe({
        next: (result) => {
          this.projetoService.recarregar();
          this.spinner.hide();
          this.toastr.success("Projeto editado com sucesso.", "Sucesso!");
        },
        error: error => {
          this.spinner.hide();
          this.toastr.error("Ocorreu um erro ao tentar editar projeto. Tente novamente.", "Erro");
        },
        complete: () => {
          this.modal.dismissAll();
          this.spinner.hide();
        }
      });
  }

  public excluir(): void {
    if (this.projeto.id == null) return;

    this.spinner.show();

    this
      .projetoApiService
      .deletarProjeto(this.projeto.id)
      .subscribe({
        next: () => {
          this.projetoService.recarregar();
          this.spinner.hide();
          this.toastr.success("Projeto deletado com sucesso.", "Sucesso!");
        },
        error: error => {
          this.spinner.hide();
          this.toastr.error("Ocorreu um erro ao tentar deletar projeto. Tente novamente.", "Erro");
        },
        complete: () => {
          this.modal.dismissAll();
          this.spinner.hide();
        }
      });
  }

  private definirForm(): void {
    this.formEdicao = this.formBuilder.group({
      nome: [this.projeto.nome, Validators.required],
      descricao: [this.projeto.descricao],
    });
  }
}
