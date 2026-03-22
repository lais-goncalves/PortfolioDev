import { Injectable } from '@angular/core';
import {HttpService} from '../http/http-service';
import {map, Observable} from 'rxjs';
import {Usuario} from '../../models/usuario/usuario';

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  constructor(
    private httpService: HttpService,
  ) {}

  public buscarPorId(id: number): Observable<Usuario> {
    return this
      .httpService
      .enviarGET(
        `/api/Usuarios/Id/${id}`,
        { withCredentials: true }
      )
      .pipe(map((res: any) => {
        return res as Usuario;
      }));
  }

  public buscarPorUserName(userName: string): Observable<Usuario> {
    return this
      .httpService
      .enviarGET(
        `/api/Usuarios/UserName/${userName}`,
        { withCredentials: true }
      )
      .pipe(map((res: any) => {
        return res as Usuario;
      }));
  }
}
