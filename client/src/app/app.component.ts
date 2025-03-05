import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';
  users: any;

  constructor(private http: HttpClient){}

  ngOnInit() {
    this.getUsers();
  }

  getUsers(){
    this.http.get('http://localhost:5000/api/users').subscribe(response =>{   //send a GET request to specified URL, API endpoint 
      this.users = response; //Data is stored in the variable users
    }, error => {
      console.error('Error fetching users:', error);
      alert('Failed to load users. Please try again later.');
    })
  }

}
