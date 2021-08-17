import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AuthService } from './../_services/auth.service';
import { AlertifyService } from './../_services/alertify.service';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor (private authService: AuthService, private router: Router, private alertifyService: AlertifyService) {
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    if (this.authService.loggedIn()) {
      return true;
    }

    this.alertifyService.error('You need to login to have access!');
    this.router.navigate(['/home']);

    return false;
  }
}
