import { Component, OnInit } from '@angular/core';
import { MenuController } from '@ionic/angular';
import { AuthService } from './auth/auth.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-portal',
  templateUrl: './portal.page.html',
  styleUrls: ['./portal.page.scss']
})
export class PortalPage implements OnInit {
  
  public appPages: any;
  public loginStatusColor = 'dark';
  private authSub: Subscription;

  constructor(
    private menu: MenuController,
    private authService: AuthService,
    private router: Router) {

    this.authSub = authService.userIsAuthenticated.subscribe({
      next: x => {
        this.setLoginStatusColor(x);
      }
    });

    this.appPages = this.getAppPages();
  }

  ngOnInit() { }

  public openPortalMenu() {
    this.menu.enable(true, 'main1');
    this.menu.open('main1');
  }

  public openAuth() {
    this.router.navigateByUrl('/portal/auth');
  }

  public openSearch() {

  }

  private setLoginStatusColor(isloggedin: boolean) {
    if (isloggedin) {
      this.loginStatusColor = 'secondary';
    } else {
      this.loginStatusColor = 'warning';
    };
  }

  //todo: make a class for this type
  private getAppPages(): any {
    return [
      {
        title: 'Main',
        url: '/portal/home',
        icon: 'home'
      },
      {
        title: 'Search',
        url: '/portal/search',
        icon: 'search'
      },
      {
        title: 'Auth',
        url: '/portal/auth',
        icon: 'contact'
      },
      {
        title: 'Geo Data Manager',
        url: '/portal/geo-data-manager',
        icon: 'map'
      },
      {
        title: 'Settings',
        url: '/portal/portal-settings',
        icon: 'cog'
      }
    ];
  }

}
