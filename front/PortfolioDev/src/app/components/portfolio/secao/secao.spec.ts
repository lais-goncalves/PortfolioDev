import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Secao } from './secao';

describe('Secao', () => {
  let component: Secao;
  let fixture: ComponentFixture<Secao>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Secao]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Secao);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
