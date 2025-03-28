import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {map} from 'rxjs/operators';
import { User } from '../_models/user';
import {ReplaySubject} from 'rxjs';


@Injectable({
  providedIn: 'root' //this makes the service app wide
})
export class AccountService {

  baseUrl ='https://localhost:5001/api/';

  //observable to store the current user
  private currentUserSource = new ReplaySubject<User | null>(1);
  
  currentUser$ = this.currentUserSource.asObservable();

  //inject http service 
  constructor(private http: HttpClient) { }

  login(model: any){
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }


  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

}
