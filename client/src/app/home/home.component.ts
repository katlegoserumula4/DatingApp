import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode = false;
  users: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle(){
    this.registerMode = !this.registerMode;
  }

  getUsers(){
    this.http.get('https://localhost:5001/api/users').subscribe(users =>{   //send a GET request to specified URL, API endpoint 
      this.users = users; //Data is stored in the variable users
    }, error => {
      console.error('Error fetching users:', error);
      alert('Failed to load users. Please try again later.');
    })
  }

  

}
