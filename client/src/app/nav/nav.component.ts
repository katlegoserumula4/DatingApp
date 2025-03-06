import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_service/account.service';
import { Observable } from 'rxjs';
import { User } from '../_models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})

// class that manages login n logout behavior
export class NavComponent  implements OnInit {
  model: any = {};
  loggedIn: boolean = false;
  // currentUser$!: Observable<User | null>;

  //constructor receives an instance through DI, responsible for handling login n logout logic
  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
    // this.currentUser$ = this.accountService.currentUser$;
  }

  //loggin user in
  login(){
    this.accountService.login(this.model).subscribe(response =>{
      console.log(response);
     
    }, error =>{
      console.log(error);
    })
  }

  logout(){
    this.accountService.logout();
    // this.loggedIn = false;
  }


}
