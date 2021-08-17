import { Component, OnInit } from '@angular/core';
import { AuthService } from './../_services/auth.service';
import { AlertifyService } from './../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor (private authservice: AuthService, private alertifyService: AlertifyService, private router: Router) { }

  ngOnInit() {
  }

  login(): any {
    // console.log(this.model);

    this.authservice.login(this.model).subscribe(data => {
      this.alertifyService.success('Logged in successfully.');
    }, error => {
      this.alertifyService.error('Failed to login!');
    }, () => {
      this.router.navigate(['/members']);
    });
  }

  loggedIn() {
    return this.authservice.loggedIn();
  }

  logout() {
    this.authservice.userToken = null;
    localStorage.removeItem('token');
    this.alertifyService.warning('Logged out successfully.');
    this.router.navigate(['/home']);
  }
}
