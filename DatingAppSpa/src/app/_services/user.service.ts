
import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { environment } from './../../environments/environment';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { Observable } from 'rxjs/Observable';
import { User } from '../_models/Users';
import { AuthHttp } from 'angular2-jwt';

@Injectable()
export class UserService {

	baseUrl = environment.apiUrl;

	constructor (private authHttp: AuthHttp) { }

	getUsers(): Observable<User[]> {
		return this.authHttp
			.get(this.baseUrl + 'users')
			.map(response => <User[]>response.json())
			.catch(this.handleError);
	}

	getUser(id: number): Observable<User> {
		return this.authHttp
			.get(this.baseUrl + 'users/' + id)
			.map(response => <User>response.json())
			.catch(this.handleError);
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





// import { Injectable } from '@angular/core';
// import { Http, RequestOptions, Headers } from '@angular/http';
// import { environment } from './../../environments/environment';
// import 'rxjs/add/operator/map';
// import 'rxjs/add/operator/catch';
// import 'rxjs/add/observable/throw';
// import { Observable } from 'rxjs/Observable';
// import { User } from '../_models/Users';
// import { AuthHttp } from 'angular2-jwt';

// @Injectable()
// export class UserService {

// 	basseUrl = environment.apiUrl;

// 	constructor (private http: Http) { }

// 	getUsers(): Observable<User[]> {
// 		return this.http
// 			.get(this.basseUrl + 'users', this.jwt())
// 			.map(response => <User[]>response.json())
// 			.catch(this.handleError);
// 	}

// 	private jwt(): any {
// 		const token = localStorage.getItem('token');

// 		if (token) {
// 			const headers = new Headers({ 'Authorization': 'Bearer' + token });
// 			headers.append('Content-Type', 'application-json');

// 			return new RequestOptions({ headers: headers });
// 		}
// 	}

// 	private handleError(error: any) {
// 		const applicationError = error.headers.get('Application-Error');

// 		if (applicationError) {
// 			return Observable.throw(applicationError);
// 		}

// 		const serverError = error.json();

// 		let modelStateErrors = '';

// 		if (serverError) {
// 			for (const key in serverError) {
// 				if (serverError[key]) {
// 					modelStateErrors += serverError[key] + '\n';
// 				}
// 			}
// 		}

// 		return Observable.throw(
// 			modelStateErrors || 'Server Error'
// 		);
// 	}
// }
