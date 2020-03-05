import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { LoadingController, AlertController, ToastController } from '@ionic/angular';
import { Observable, Subscription } from 'rxjs';
import { AuthService, AuthResponseData } from './auth.service';
import { User } from './user.model';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.page.html',
  styleUrls: ['./auth.page.scss']
})
export class AuthPage implements OnInit, OnDestroy {
  public isLoading = false;
  public isLogin = true;
  public isAuthenticated: boolean;
  public currentUser: User;

  private isAuthSub: Subscription;
  private currentUserSub: Subscription;

  constructor(
    private authService: AuthService,
    private router: Router,
    private loadingCtrl: LoadingController,
    private alertCtrl: AlertController,
    public toastController: ToastController
  ) { }

  ngOnInit() {

    this.isAuthSub = this.authService.userIsAuthenticated.subscribe({
      next: authenticated => {
        this.isAuthenticated = authenticated;
      }
    });

    console.log("auth current user:", this.authService.currentUser )
    this.currentUserSub = this.authService.currentUser.subscribe({
      next: user => {
        console.log("user sub:", user)
        this.currentUser = user;
      }
    });

  }

  authenticate(email: string, password: string) {
    this.isLoading = true;
    this.loadingCtrl
      .create({ keyboardClose: true, message: 'Logging in...' })
      .then(loadingEl => {
        loadingEl.present();
        let authObs: Observable<AuthResponseData>;
        if (this.isLogin) {
          console.log('logging in', email, password)
          authObs = this.authService.login(email, password);
        } else {
          console.log('creating account', email, password)
          authObs = this.authService.signup(email, password);
        }
        authObs.subscribe(
          resData => {
            console.log(resData);
            this.presentToast();
            this.isLoading = false;
            loadingEl.dismiss();
            this.router.navigateByUrl('/');
          },
          errRes => {
            console.log('login error', errRes)
            loadingEl.dismiss();
            const code = errRes;
            let message = 'Authentication error. Check logs';
            this.showAlert(message);
          }
        );
      });
  }

  onSwitchAuthMode() {
    this.isLogin = !this.isLogin;
  }

  onSubmit(form: NgForm) {
    if (!form.valid) {
      return;
    }
    const email = form.value.email;
    const password = form.value.password;

    this.authenticate(email, password);
  }

  private showAlert(message: string) {
    this.alertCtrl
      .create({
        header: 'Authentication failed',
        message: message,
        buttons: ['OK']
      })
      .then(alertEl => alertEl.present());
  }

  async presentToast() {
    const toast = await this.toastController.create({
      message: 'Authentication successful',
      duration: 1000
    });
    toast.present();
  }

  public logoutCurrentUser() {
    this.authService.logout();
  }

  ngOnDestroy(): void {
    this.isAuthSub.unsubscribe();
    this.currentUserSub.unsubscribe();
  }

}
