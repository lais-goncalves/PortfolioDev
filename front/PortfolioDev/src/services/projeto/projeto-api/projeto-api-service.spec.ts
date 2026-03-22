import { TestBed } from '@angular/core/testing';

import { ProjetoApiService } from './projeto-api-service';

describe('ProjetoApiService', () => {
  let service: ProjetoApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProjetoApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
