import { Component, inject, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { CitiesService } from '../../../../api-services/cities/cities.service';
@Component({
  selector: 'app-register-component',
  standalone: false,
  templateUrl: './register-component.html',
  styleUrl: './register-component.scss'
})
export class RegisterComponent implements OnInit {

  private _formBuilder = inject(FormBuilder);
  private cityService = inject(CitiesService);
  cityList: any;
  dateControl = new FormControl(new Date());
   basicInfo = this._formBuilder.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required]
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

}