import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from '../../_models/Users';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from './../../_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from './../../_services/user.service';
import { AuthService } from './../../_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;

  user: User;


  constructor (private route: ActivatedRoute, private alertifyService: AlertifyService,
    private userService: UserService, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
  }

  onUpdateUser() {
    this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {
      this.alertifyService.success('Profile updated successfully!');
      this.editForm.reset(this.user);
    }, error => {
      this.alertifyService.error(error);
    });
  }
}
