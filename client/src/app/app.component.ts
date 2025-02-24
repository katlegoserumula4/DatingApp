import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
// import { error } from 'console';


//Connection establishment to the other files here
//Decorators
@Component({
  selector: 'app-root', //links to html.index
  templateUrl: './app.component.html', //links to app.component.html which is the display of fetch data
  styleUrls: ['./app.component.css']
})

//a class that defines the main component of the app
//Connect to the backend, to fecth the list of users through the getUsers method, 
// once data is retrieved it is stored in the users variable.

export class AppComponent implements OnInit {
  title = 'Katlego Serumula';
  users: any; //property meant to store data, any kind of data, can hold data fetched from server


  //constructor for AppComponent with parameter http, HttpClient is used to make Http request requests, fecth data from API or server
  constructor(private http: HttpClient){

  }
  //a place to put code to run when the component is set up, fetching data
  ngOnInit() {
    this.getUsers();
  }
  
  //method for making an HTTP GET request to fetch data from API
  //"this" keyword grant an access to anything in the class
  getUsers(){
    this.http.get('http://localhost:5001/api/users').subscribe(response =>{   //send a GET request to specified URL, API endpoint 
      this.users = response; //Data is stored in the variable users
    }, error => {
      console.error('Error fetching users:', error);
      alert('Failed to load users. Please try again later.');
    })
  }
}
