import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode = false;
  // values: any;

  constructor (private http: Http) { }

  ngOnInit() {
    // this.getValues();
  }

  registerToggle() {
    this.registerMode = true;
  }

  CancelRegisterMode(registerMode: boolean) {
    this.registerMode = registerMode;
  }
}






  // getValues() {
  //   this.http.get('http://localhost:56436/api/values').subscribe(response => {
  //     this.values = response.json();
  //   })
  // }