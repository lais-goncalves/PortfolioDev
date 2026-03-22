import {inject, Injectable, PLATFORM_ID, REQUEST} from '@angular/core';
import {HttpService} from '../http/http-service';
import {Login} from '../../models/usuario/login/login';
import {Registro} from '../../models/usuario/registro/registro';
import {BehaviorSubject, filter, map, Observable, tap} from 'rxjs';
import {Usuario} from '../../models/usuario/usuario';
import {isPlatformServer} from '@angular/common';

export const NAO_CARREGADO = Symbol('NAO_CARREGADO');
export type EstadoUsuario = Usuario | null | typeof NAO_CARREGADO;

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  public jaCarregou: boolean = false;
  private usuarioEstadoSubject = new BehaviorSubject<EstadoUsuario>(NAO_CARREGADO);
  private usuarioEstado$: Observable<EstadoUsuario> = this.usuarioEstadoSubject.asObservable();

  private platformId = inject(PLATFORM_ID);
  private request = isPlatformServer(this.platformId)
    ? inject(REQUEST, {optional: true})
    : null;

  constructor(private httpService: HttpService) {
    this.carregarUsuarioAtual();
  }

  get usuario$(): Observable<Usuario | null> {
    return this.usuarioEstado$.pipe(
      filter(u => u !== NAO_CARREGADO),
      tap(u => console.log("usuário: ", u?.userName)),
      map(u => u as Usuario | null)
    );
  }

  get estaLogado$(): Observable<boolean> {
    return this
      .usuario$
      .pipe(
        map((u: EstadoUsuario) => {
            const usuario = u as Usuario;
            return u != null && usuario.userName != null
          }
        ));
  }

  carregarUsuarioAtual() {
    this.jaCarregou = false;

    // TODO: entender como isso funciona
    const options: any = {withCredentials: true};

    if (isPlatformServer(this.platformId) && this.request) {
      const cookie = this.request.headers.get('cookie');
      if (cookie) {
        options.headers = {Cookie: cookie};
      }
    }

    this.httpService.enviarGET('/Autenticar/Usuario', options).subscribe({
      next: (data: any) => {
        console.log("dados brutos da API:", data, typeof data);
        console.log("usuário carregado com sucesso")
        this.usuarioEstadoSubject.next(data as Usuario);
        this.jaCarregou = true;
      },
      error: (e) => {
        console.log("houve um erro ao tentar carregar usuário")
        this.usuarioEstadoSubject.next(null);
        this.jaCarregou = true;
      }
    });
  }

  public logar(
    dadosLogin: Login,
    callbackSucesso: any = () => {
    },
    callbackFalhou: any = () => {
    },
    callbackComplete: any = () => {
    },
  ): void {
    this.httpService
      .enviarPOST(
        "/Autenticar/Login",
        dadosLogin,
        {withCredentials: true}
      )
      .subscribe({
        next: (res) => {
          this.carregarUsuarioAtual();
          callbackSucesso(res)
        },
        error: e => callbackFalhou(e.response),
        complete: () => callbackComplete()
      });
  }

  public deslogar(
    callbackSucesso: any = () => {
    },
    callbackFalhou: any = () => {
    },
    callbackComplete: any = () => {
    }
  ): void {
    this.httpService
      .enviarGET(
        "/Autenticar/Logout",
        {withCredentials: true}
      )
      .subscribe({
        next: (res) => {
          this.carregarUsuarioAtual();
          callbackSucesso(res)
        },
        error: e => callbackFalhou(e.response),
        complete: () => callbackComplete()
      });
  }

  public registrar(
    dadosRegistro: Registro,
    callbackSucesso: any = () => {
    },
    callbackFalhou: any = () => {
    },
    callbackComplete: any = () => {
    }
  ): void {
    this.httpService
      .enviarPOST(
        "/Autenticar/Registrar",
        dadosRegistro,
        {withCredentials: true}
      )
      .subscribe({
        next: (res) => callbackSucesso(res),
        error: e => callbackFalhou(e.response),
        complete: () => callbackComplete()
      });
  }
}
