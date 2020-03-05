import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { map, tap, switchMap } from 'rxjs/operators';
import { User } from './user.model';
import { SettingsService } from '../portal-settings/settings.service';
import { environment } from 'src/environments/environment';

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
    if (!this._user){
      console.log("newing up subject user")
      this._user = new BehaviorSubject<User>(null);      
    }
    return this._user;
  }

  public get userIsAuthenticated(): Observable<boolean> {
    return this._user.asObservable().pipe(
      map(user => {
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
    //todo: add user creation endpoint on api
    // use api url from environment
    // from response create new AuthResponseData 
    return new Observable<AuthResponseData>();
  }

  login(loginID: string, password: string): Observable<AuthResponseData> {
    const formData = new FormData();
    formData.append('client_id', 'geomgrui');
    formData.append('client_secret', environment.authClientSecret);
    formData.append('grant_type', 'password');
    formData.append('username', loginID);
    formData.append('password', password);

    console.log("in login", formData)

    return this.settingsService.getURLSettings().pipe(
      switchMap(
        stuff => {
          const authurl = stuff.AuthAPI + '/connect/token';
          return this.http.post<AuthResponseData>(authurl, formData)
            .pipe(tap(authResponse => {
              this.setUserData(authResponse, loginID);
            }));
        }
      )
    );

  }

  logout() {
    this._user.next(null);
  }

  private setUserData(userData: AuthResponseData, loginID:string) {
    console.log("in setuserdata:", userData, "login", loginID)
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
    //Plugins.Storage.set({ key: 'authData', value: data });
  }

  public autoLogin() {
    // return from(Plugins.Storage.get({ key: 'authData' })).pipe(
    //   map(storedData => {
    //     if (!storedData || !storedData.value) {
    //       return null;
    //     }
    //     const parsedData = JSON.parse(storedData.value) as {
    //       token: string;
    //       tokenExpirationDate: string;
    //       userId: string;          
    //     };
    //     const expirationTime = new Date(parsedData.tokenExpirationDate);
    //     if (expirationTime <= new Date()) {
    //       return null;
    //     }
    //     const user = new User(
    //       parsedData.userId,
    //       'not set',
    //       parsedData.token,
    //       expirationTime
    //     );
    //     return user;
    //   }),
    //   tap(user => {
    //     if (user) {
    //       this._user.next(user);
    //     }
    //   }),
    //   map(user => {
    //     return !!user;
    //   })
    // );
  }


}
