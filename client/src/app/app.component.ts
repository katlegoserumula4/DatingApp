import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_service/account.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';
  users: any;
  
  constructor( private accountService: AccountService){}


  ngOnInit() {
    // this.getUsers();
    this.setCurrentUser();
  }

  setCurrentUser(){
    const userJson = localStorage.getItem('user');
    const user: User = userJson ? JSON.parse(userJson) : null;
    this.accountService.setCurrentUser(user);
  }



  // getUsers(){
  //   this.http.get('https://localhost:5001/api/users').subscribe(response =>{   //send a GET request to specified URL, API endpoint 
  //     this.users = response; //Data is stored in the variable users
  //   }, error => {
  //     console.error('Error fetching users:', error);
  //     alert('Failed to load users. Please try again later.');
  //   })
  // }

}
