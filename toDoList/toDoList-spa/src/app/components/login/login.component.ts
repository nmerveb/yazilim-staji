import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(public authService: AuthService, private router :Router) { }
  loginUser:any = {};
  
  login(){
    this.authService.login(this.loginUser);
    window.location.reload();
    this.router.navigateByUrl("/home");
  }

  get isAuthenticated(){
    return this.authService.loggedIn();
  }

  logOut(){
   this.authService.logOut();
  }
  ngOnInit() {
  }

}
