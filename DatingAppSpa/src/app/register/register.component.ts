import { Input, Output } from '@angular/core';
import { Component, OnInit, EventEmitter } from '@angular/core';
// import { EventEmitter } from 'events';
import { AuthService } from './../_services/auth.service';
import { AlertifyService } from './../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // @Input() valuesFromHome: any;
  @Output() cancelRegister = new EventEmitter();

  model: any = {};

  constructor (private authService: AuthService, private alertifyService: AlertifyService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model).subscribe(() => {
      this.alertifyService.success('Register Successfully');
    }, error => {
      this.alertifyService.error(error);
    });

  }

  cancel() {
    this.cancelRegister.emit(false);

    this.alertifyService.message('Cancel action Successfully.');
  }
}
