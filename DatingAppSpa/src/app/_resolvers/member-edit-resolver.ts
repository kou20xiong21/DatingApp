import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from '../_models/Users';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable } from 'rxjs/';
import { AuthService } from './../_services/auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<User> {

	constructor (private userService: UserService, private alertifyService: AlertifyService,
		private router: Router, private authService: AuthService) {
	}

	resolve(route: ActivatedRouteSnapshot): Observable<User> {
		return this.userService.getUser(this.authService.decodedToken.nameid).catch(error => {
			this.alertifyService.error('Problem retrieving data');
			this.router.navigate(['/members']);
			return Observable.of(null);
		});
	}
}
