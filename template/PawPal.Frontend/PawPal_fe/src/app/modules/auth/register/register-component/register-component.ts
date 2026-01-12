import { Component, inject, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { UserService } from '../../../../api-services/register/users-service';
import { CreateUserCommand } from '../../../../api-services/register/users-model';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { AuthFacadeService } from '../../../../core/services/auth/auth-facade.service';
import { LoginCommand } from '../../../../api-services/auth/auth-api.model';
import { CurrentUserDto } from '../../../../core/services/auth/current-user.dto';
import { NET_CLAIM_TYPES } from '../../../../core/services/auth/jwt-payload.dto';
import { Router } from '@angular/router';
@Component({
  selector: 'app-register-component',
  standalone: false,
  templateUrl: './register-component.html',
  styleUrl: './register-component.scss'
})
export class RegisterComponent implements OnInit {

  private _formBuilder = inject(FormBuilder);
  private cityService = inject(CitiesService);
  private userService = inject(UserService);
    private auth = inject(AuthFacadeService);
  private currentUser = inject(CurrentUserService);
    private router = inject(Router);


    
  cityList: any = [];
  cityId : number = 0;
  dateOfBirth : Date = new Date();
  showPassword = false;

  dateControl = new FormControl(new Date());
   basicInfo = this._formBuilder.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    cityId: ['', Validators.required],
    dateOfBirth: ['', Validators.required]
  });
  
  accountInfo = this._formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });
    ngOnInit(): void {
    this.loadCities();
}  

  loadCities() :void {
    this.cityService.listCities().subscribe((res) =>{
      this.cityList = res;
    }
    )
  }
   togglePassword():void{this.showPassword = !this.showPassword;}

   onSubmit() {
    if(this.accountInfo.invalid || this.basicInfo.invalid) return;
      const payload : CreateUserCommand={
        firstName: this.basicInfo.value.firstName ?? '',
        lastName: this.basicInfo.value.lastName ?? '',
        birthDate: new Date(this.basicInfo.value.dateOfBirth ?? ''),
        email: this.accountInfo.value.email ?? '',
        password: this.accountInfo.value.password ?? '',
        roleID: 2,
        city: this.basicInfo.value.cityId ?? 0,
        profilePictureURL: null,
      }

      //user is created here
      this.userService.createUser(payload).subscribe({
        next: (res) => {
          console.log("Registration successful, user ID:", res);
          this.auth.redirectToLogin();
        },
        error: (err) => {
          console.error('Registration error:', err);
        },
      });
   
}
}
