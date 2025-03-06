import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_service/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})

// class that manages login n logout behavior
export class NavComponent  implements OnInit {
  model: any = {};
  loggedIn: boolean = false;

  //constructor receives an instance through DI, responsible for handling login n logout logic
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  //loggin user in
  login(){
    this.accountService.login(this.model).subscribe(response =>{
      console.log(response);
      this.loggedIn = true;
    }, error =>{
      console.log(error);
    })
  }

  logout(){
    this.loggedIn = false;
  }

}
