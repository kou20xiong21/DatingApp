import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
  values: any;

  // tslint:disable-next-line: space-before-function-paren
  constructor (private http: HttpClient) { }

  ngOnInit(): void {
    this.getValues();
  }

  getValues(): any {
    this.http.get('http://localhost:56436/api/values').subscribe(response =>
      this.values = response);
  }

}
