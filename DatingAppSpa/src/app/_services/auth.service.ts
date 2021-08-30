import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { Observable } from 'rxjs/Observable';
import { JwtHelper, tokenNotExpired } from 'angular2-jwt';
import { User } from '../_models/Users';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

@Injectable()
export class AuthService {
	baseUrl = 'http://localhost:56436/api/auth/';

	userToken: any;
	decodedToken: any;
	currentUser: User;
	jwtHelper: JwtHelper = new JwtHelper();


	private photoUrl = new BehaviorSubject<string>('../../assets/user.png');
	currentPhotoUrl = this.photoUrl.asObservable();


	constructor (private http: Http) { }

	login(model: any) {
		return this.http
			.post(this.baseUrl + 'login', model, this.requestOptions())
			.map((response) => {
				const user = response.json();

				if (user && user.tokenString) {
					localStorage.setItem('token', user.tokenString);
					localStorage.setItem('user', JSON.stringify(user.user));

					this.decodedToken = this.jwtHelper.decodeToken(user.tokentString);
					this.currentUser = user.user;
					this.userToken = user.tokenString;

					if (this.currentUser !== null) {
						this.changeMemberPhoto(this.currentUser.photoUrl);
					} else {
						this.changeMemberPhoto('../../assets/user.png')
					}
				}
			}).catch(this.handleError);
	}

	changeMemberPhoto(photoUrl: string) {
		this.photoUrl.next(photoUrl);
	}

	register(user: User) {
		return this.http.post(this.baseUrl + 'register', user, this.requestOptions())
			.catch(this.handleError);
	}

	loggedIn() {
		return tokenNotExpired('token');
	}

	private requestOptions(): any {
		const headers = new Headers({ 'Contnent-type': 'application/json' });
		// const options = new RequestOptions({ headers: headers });
		return new RequestOptions({ headers: headers });
	}

	private handleError(error: any) {
		const applicationError = error.headers.get('Application-Error');

		if (applicationError) {
			return Observable.throw(applicationError);
		}

		const serverError = error.json();

		let modelStateErrors = '';

		if (serverError) {
			for (const key in serverError) {
				if (serverError[key]) {
					modelStateErrors += serverError[key] + '\n';
				}
			}
		}

		return Observable.throw(
			modelStateErrors || 'Server Error'
		);
	}
}
