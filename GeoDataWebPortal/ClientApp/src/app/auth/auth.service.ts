import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, from, of } from 'rxjs';
import { map, tap, switchMap } from 'rxjs/operators';
import { User } from './user.model';
import { SettingsService } from '../portal-settings/settings.service';

export interface AuthResponseData {
  access_token: string;
  refresh_Token: string;
  expires_in: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _user = new BehaviorSubject<User>(null);

  constructor(private http: HttpClient, private settingsService: SettingsService) { }

  public get currentUser(): BehaviorSubject<User> {
    if (!this._user) {
      this._user = new BehaviorSubject<User>(null);
    }
    return this._user;
  }

  public get userIsAuthenticated(): Observable<boolean> {
    // if (this.autoLogin()) {
    //   return of(true)
    // } else {
    //   return of(false);
    // }

    // this._user.asObservable().pipe(
    //   map(user => {
    //     if (user && !user.tokenExpirationDate || user.tokenExpirationDate <= new Date()) {
    //       return !!user.token;
    //     } else {
    //       return false;
    //     }
    //   })
    // );

    return this._user.asObservable().pipe(
      map(user => {
        // if (user && !user.tokenExpirationDate || user.tokenExpirationDate <= new Date()) {
        if (user) {
          return !!user.token;
        } else {
          return false;
        }
      })
    );
  }

  public get userId(): Observable<string> {
    return this._user.asObservable().pipe(
      map(user => {
        if (user) {
          return user.id;
        } else {
          return null;
        }
      })
    );
  }

  public get token(): Observable<string> {
    return this._user.asObservable().pipe(
      map(user => {
        if (user) {
          return user.token;
        } else {
          return null;
        }
      })
    );
  }

  signup(email: string, password: string): Observable<AuthResponseData> {
    //todo: need to add a user creation endpoint on api
    return new Observable<AuthResponseData>();
  }

  login(loginID: string, password: string): Observable<AuthResponseData> {
    return this.settingsService.getSettings().pipe(
      switchMap(
        config => {
          const authurl = config.AuthUrl + '/connect/token';

          const formData = new FormData();
          formData.append('client_id', 'geomgrui');
          formData.append('client_secret', config.AuthClientSecret);
          formData.append('grant_type', 'password');
          formData.append('username', loginID);
          formData.append('password', password);

          return this.http.post<AuthResponseData>(authurl, formData)
            .pipe(tap(authResponse => {
              this.setUserData(authResponse, loginID);
            }));
        }
      )
    );
  }

  logout() {
    localStorage.removeItem('authData')
    this._user.next(null);
  }

  private setUserData(userData: AuthResponseData, loginID: string) {
    const expirationTime = new Date(
      new Date().getTime() + +userData.expires_in * 1000
    );

    this._user.next(
      new User(
        loginID,
        'not set. future use',
        userData.access_token,
        expirationTime
      )
    );

    this.storeAuthData(
      loginID,
      userData.access_token,
      expirationTime.toISOString()
    );

  }

  private storeAuthData(
    userId: string,
    token: string,
    tokenExpirationDate: string
  ) {
    const data = JSON.stringify({
      userId: userId,
      token: token,
      tokenExpirationDate: tokenExpirationDate
    });

    localStorage.setItem('authData', data);    
  }

  public autoLogin(): Observable<boolean> {
    const authData: string = localStorage.getItem('authData');
    let storedUser: User = null;

    if (authData != null) {

      const parsedData = JSON.parse(authData) as {
        token: string;
        tokenExpirationDate: string;
        userId: string;
      };
      const expirationTime = new Date(parsedData.tokenExpirationDate);
      if (expirationTime <= new Date()) {        
        //todo: refresh token attempt
      } else {
        storedUser = new User(
          parsedData.userId,
          'not set',
          parsedData.token,
          expirationTime
        );
      }

      this._user.next(storedUser);
    }
    return of(!!storedUser);
  }
}
