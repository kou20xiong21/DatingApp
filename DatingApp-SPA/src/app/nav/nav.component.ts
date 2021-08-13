import { Component, OnInit } from '@angular/core';
import { AuthService } from './../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};

  // tslint:disable-next-line: space-before-function-paren
  constructor (private authservice: AuthService) { }

  ngOnInit(): void {
  }

  login(): any {
    // console.log(this.model);

    this.authservice.login(this.model).subcribe((data: any) => {
      console.log('logged in successfully');
    }, (error: any) => {
      console.log('failed to login');
    });

  }
}
