import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor( private authService:AuthService,
    private formBuilder:FormBuilder) { }

    registerForm:FormGroup;
    registerUser:any;
  ngOnInit() {
    this.createRegisterForm();
  }
  createRegisterForm(){
    this.registerForm= this.formBuilder.group({
      userName:["",[Validators.required,Validators.minLength(4),Validators.maxLength(20)]],
      password:["",[Validators.required,Validators.minLength(4),Validators.maxLength(8)]],
      confirmPassword:["",Validators.required]        //Şifre doğrulama
    },
    {validator:this.passwordMatchValidator}            //Şifre doğrulamayla şifrenin eşitliğinin kontrolü
    )
  }
  passwordMatchValidator(g:FormGroup){
    return g.get('password').value === g.get('confirmPassword').value?null:{misMatch:true}
  }

  register(){
    if(this.registerForm.valid){
      this.registerUser = Object.assign({}, this.registerForm.value);
      this.authService.register(this.registerUser);
      this.registerForm=  this.formBuilder.group({userName:[""],password:[""],confirmPassword:[""]})
      
    }
  }
}
