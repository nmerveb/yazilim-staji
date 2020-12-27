import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { JwtHelper, tokenNotExpired } from 'angular2-jwt';
import { RegisterUser } from '../components/models/registerUser';
import { LoginUser } from '../components/models/loginUser';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(@Inject('apiUrl') private apiUrl, private httpClient:HttpClient, private router :Router) { }

    userToken:any;
    decodedToken:any;
    jwtHelper:JwtHelper=new JwtHelper();
    TOKEN_KEY="token";
    
    login(loginUser:LoginUser){
      //Backenddeki login aksiyonuna veri göndermek için(headerda, application json formatında)
      let headers = new HttpHeaders();
      headers=headers.append("Content-Type","application/json");
      //Login olan kullanıcı bilgisini alır.
       this.httpClient.post(this.apiUrl + "auth/login", loginUser, { headers: headers })
      .subscribe(data => {
        this.userToken = data;
        this.decodedToken = this.jwtHelper.decodeToken(data.toString());
        console.log(this.decodedToken.nameid);
        this.save(data,this.decodedToken.nameid);
        this.router.navigateByUrl("/home");
      });
    
    
      }
      register(registerUser:RegisterUser){
        let headers = new HttpHeaders();
        headers=headers.append("Content-Type","application/json");
        this.httpClient.post(this.apiUrl+"auth/register",registerUser,{headers:headers}).subscribe(data=>{
          alert("Kayıt işlemi gerçekleşti. Giriş yapmak için login sayfasına gidiniz.");
        }, (err) => { alert("Kullanıcı adı kullanılmaktadır"); })
      }
    
      //Login işlemi gerçekleştiğinde backend'in gönderdiği token'ı tutar.
      save(token,userId){
        localStorage.setItem(this.TOKEN_KEY,token);
        localStorage.setItem("userId",userId);
      }

    
      //Çıkış
      logOut(){
        localStorage.clear();
      }
    
      //Hala sistemde mi?
      loggedIn(){
        //jwt kütüphanesinin özelliği
        return tokenNotExpired(this.TOKEN_KEY);
      }
    
      get token(){
        return localStorage.getItem(this.TOKEN_KEY);
      }
    
      //Mevcut kullanıcıyı gönderir.
      getCurrentUserId(){
        return this.jwtHelper.decodeToken(this.token).nameId;
      }
}
