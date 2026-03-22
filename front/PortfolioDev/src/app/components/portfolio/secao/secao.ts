import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-secao',
  imports: [],
  templateUrl: './secao.html',
  styleUrl: './secao.scss',
})
export class Secao {
  @Input() titulo: string | null = null;
}
