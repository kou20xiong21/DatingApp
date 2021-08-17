import { Component, OnInit } from '@angular/core';
import { AuthService } from './../_services/auth.service';
import { AlertifyService } from './../_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor (private authservice: AuthService, private alertifyService: AlertifyService) { }

  ngOnInit() {
  }

  login(): any {
    // console.log(this.model);

    this.authservice.login(this.model).subscribe(data => {
      this.alertifyService.success('Logged in successfully.');
    }, error => {
      this.alertifyService.error('Failed to login!');
    });
  }

  loggedIn() {
    return this.authservice.loggedIn();
  }

  logout() {
    this.authservice.userToken = null;
    localStorage.removeItem('token');
    this.alertifyService.warning('Logged out successfully.');
  }
}