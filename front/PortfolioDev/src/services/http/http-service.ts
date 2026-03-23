import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {take} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  public apiUrl: string;

  constructor(private httpClient: HttpClient) {
    if (environment.production) {
      this.apiUrl = environment.apiUrlProd;
    } else {
      this.apiUrl = environment.apiUrlDev;
    }
  }

  public criarUrl(url: string) {
    if (url[0] === '\\' || url[0] === '/') {
      url = url.substring(1);
    }

    return this.apiUrl + url;
  }

  public enviarGET(url: string, options: any | null = {}) {
    return this.httpClient.get(this.criarUrl(url), options).pipe(take(1));
  }

  public enviarPOST(url: string, body: any, options: any | null = {}) {
    return this.httpClient.post(this.criarUrl(url), body, options).pipe(take(1));
  }

  public enviarPUT(url: string, body: any, options: any | null = {}) {
    return this.httpClient.put(this.criarUrl(url), body, options).pipe(take(1));
  }

  public enviarDELETE(url: string, options: any | null = {}) {
    return this.httpClient.delete(this.criarUrl(url), options).pipe(take(1));
  }
}
