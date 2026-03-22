import {Component, OnInit} from '@angular/core';
import {AbstractControl, FormGroup} from '@angular/forms';

@Component({
  selector: 'app-form',
  imports: [],
  templateUrl: './form.html',
  styleUrl: './form.scss',
})
export class Form {
  public form: FormGroup = new FormGroup({});

  get controls(): any {
    return this.form.controls;
  }

  public verificarControleInvalido(nomeControl: string): boolean {
    var control: AbstractControl = this.form.controls[nomeControl];
    return !!(control.errors && control.touched);
  }
}
