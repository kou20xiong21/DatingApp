import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class AuthService {
	baseUrl = 'http://localhost:56436/api/auth/';

	userToken: any;

	constructor (private http: Http) { }

	login(model: any) {

		return this.http.post(this.baseUrl + 'login', model, this.requestOptions()).map((response) => {

			const user = response.json();

			if (user) {
				localStorage.setItem('token', user.tokenString);
				this.userToken = user.tokenString;
			}
		});
	}

	register(model: any) {
		return this.http.post(this.baseUrl + 'register', model, this.requestOptions())
	}

	private requestOptions(): any {
		const headers = new Headers({ 'Contnent-type': 'application/json' });
		const options = new RequestOptions({ headers: headers });
	}
}
