import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
// import 'rxjs/map/add/operator';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = 'http://localhost:56436/api/auth/';
  userToken: any;

  // tslint:disable-next-line: space-before-function-paren
  constructor (private http: HttpClient) { }

  login(model: any): any {
    // const headers = new Headers({ 'Content-type': 'application/json' });
    // // tslint:disable-next-line: object-literal-shorthand
    // const options = new RequestOptions({ headers: headers });

    return this.http.post(this.baseUrl + 'login', model).subscribe((response: any) => {
      const user = response;

      if (user) {
        localStorage.setItem('token', user.tokenString);
        this.userToken = user.tokenString;
      }
    });
  }
}
