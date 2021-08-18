import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from '../_models/Users';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable } from 'rxjs/';

@Injectable()
export class MemberDetailResolver implements Resolve<User>{

	constructor (private userService: UserService, private alertifyService: AlertifyService,
		private router: Router) {
	}

	resolve(router: ActivatedRouteSnapshot): Observable<User> {
		return this.userService.getUser(router.params['id']).catch(error => {
			this.alertifyService.error('Problem retrieving data');
			this.router.navigate(['/members']);
			return Observable.of(null);
		});
	}
}