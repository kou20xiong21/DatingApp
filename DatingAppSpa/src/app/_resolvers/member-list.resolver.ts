import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from '../_models/Users';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
// mport 'rxjs/add/operator/catch/of';
import { Observable } from 'rxjs/';

@Injectable()
export class MemberListResolver implements Resolve<User[]>{

	constructor (private userService: UserService, private alertifyService: AlertifyService,
		private router: Router) {
	}

	resolve(router: ActivatedRouteSnapshot): Observable<User[]> {
		return this.userService.getUsers().catch(error => {
			this.alertifyService.error('Problem retrieving data');
			this.router.navigate(['/home']);
			return Observable.of(null);
		});
	}
}