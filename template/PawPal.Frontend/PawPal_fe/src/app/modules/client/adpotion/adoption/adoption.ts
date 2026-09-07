import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AdoptionDialog } from './adoption-dialog/adoption-dialog/adoption-dialog';
import { Location } from '@angular/common';
import { AnimalRequestService } from '../../../../api-services/animals-adoption/animals-adoption-service';
import { CreateAdoptionRequirement } from '../../../../api-services/animals-requirements/animals-requirements-model';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DialoguePopupService } from '../../../../api-services/dialogue-popup/dialogue-popup.service';

@Component({
  selector: 'app-adoption',
  standalone: false,
  templateUrl: './adoption.html',
  styleUrl: './adoption.scss',
})
export class Adoption implements OnInit {
  dialog = inject(DialoguePopupService);
  location = inject(Location);
  requestService = inject(AnimalRequestService);
  currentUserService = inject(CurrentUserService);
  route = inject(ActivatedRoute);
  step1FormGroup = new FormGroup({
    houseType: new FormControl('', [Validators.required]),
    adress: new FormControl('', [Validators.required]),
    numOfFloor: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
    numOfFamily: new FormControl(0, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(1)],
    }),
    children: new FormControl(false),
    olderPeople: new FormControl(false),
    otherPets: new FormControl(false),
    yardAvail: new FormControl(false),
    yardInfo: new FormControl({ value: '', disabled: true }),
  });
  step2FormGroup = new FormGroup({
    pastExp: new FormControl(false),
    yourExp: new FormControl({ value: '', disabled: true }),
    petInHouse: new FormControl('', [Validators.required]),
    familyAvail: new FormControl('', [Validators.required]),
    gift: new FormControl(false),
    placeToLive: new FormControl('', [Validators.required]),
    financialSupport: new FormControl(0, [Validators.required, Validators.min(1)]),
    allergies: new FormControl(false),
    angerIssues: new FormControl(false),
    readyToBack: new FormControl(false),
  });
  step3FormGroup = new FormGroup({
    placeDesc: new FormControl('', [Validators.required]),
    comment: new FormControl(''),
    iAmReady: new FormControl(false, Validators.requiredTrue),
  });
  router = inject(Router);
  postID: number = 0;
  isSubmitting = false;
  ngOnInit(): void {
    this.postID = this.route.snapshot.queryParams['postID'];
  }

  sendRequest() {
    if (this.step1FormGroup.invalid || this.step2FormGroup.invalid || this.step3FormGroup.invalid) {
      this.step1FormGroup.markAllAsTouched();
      this.step2FormGroup.markAllAsTouched();
      this.step3FormGroup.markAllAsTouched();
      return;
    }
    if (!this.postID) {
      this.dialog.error(
        'Error',
        'This adoption form is not linked to a post. Please open it from the animal you want to adopt.',
        'OK',
      );
      return;
    }
    if (this.isSubmitting) {
      return;
    }
    this.isSubmitting = true;

    const payload: CreateAdoptionRequirement = {
      houseType: this.step1FormGroup.controls['houseType'].value as string,
      address: this.step1FormGroup.controls['adress'].value as string,
      floorNumber: this.step1FormGroup.controls['numOfFloor'].value as number,
      peopleCount: this.step1FormGroup.controls['numOfFamily'].value as number,
      childrenAround: this.step1FormGroup.controls['children'].value as boolean,
      elderlyAround: this.step1FormGroup.controls['olderPeople'].value as boolean,
      otherPetsAround: this.step1FormGroup.controls['otherPets'].value as boolean,
      yardAvailable: this.step1FormGroup.controls['yardAvail'].value as boolean,
      yardDetails: this.step1FormGroup.controls['yardAvail'].value
        ? (this.step1FormGroup.controls['yardInfo'].value as string)
        : '',

      petExp: this.step2FormGroup.controls['pastExp'].value as boolean,
      expDetails: this.step2FormGroup.controls['pastExp'].value
        ? (this.step2FormGroup.controls['yourExp'].value as string)
        : '',
      peopleAva: this.step2FormGroup.controls['familyAvail'].value as string,
      isGift: this.step2FormGroup.controls['gift'].value as boolean,
      planedStay: this.step2FormGroup.controls['placeToLive'].value as string,
      sumMoney: this.step2FormGroup.controls['financialSupport'].value as number,
      allergy: this.step2FormGroup.controls['allergies'].value as boolean,
      aggressiveness: this.step2FormGroup.controls['angerIssues'].value as boolean,
      takeBack: this.step2FormGroup.controls['readyToBack'].value as boolean,

      houseDetials: this.step3FormGroup.controls['placeDesc'].value as string,
      finalComment: this.step3FormGroup.controls['comment'].value as string,
      postID: this.postID,
    };

    this.requestService.addRequestWithRequirement(payload).subscribe({
      next: () => {
        this.dialog.success(
          'Request Sent',
          'Your adoption request has been sent successfully. Please wait for further updates.',
          'OK',
        );
        this.router.navigate(['']);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.dialog.error('Error', err?.error?.message ?? 'Could not send your request.', 'OK');
      },
    });
  }
  get yard() {
    return this.step1FormGroup.value.yardAvail;
  }

  validateNextStep() {
    if (this.step1FormGroup.invalid) {
      this.step1FormGroup.markAllAsTouched();
      return;
    }
    if (this.step2FormGroup.invalid) {
      this.step2FormGroup.markAllAsTouched();
      return;
    }
    if (this.step3FormGroup.invalid) {
      this.step3FormGroup.markAllAsTouched();
      return;
    }
  }

  addToInput(value: number, name: string) {
    if (name == 'numOfFloor') {
      const currentVal = this.step1FormGroup.get('numOfFloor')?.value || 0;
      if (currentVal != 0 || value != -1) {
        this.step1FormGroup.patchValue({
          numOfFloor: currentVal + value,
        });
      }
    } else if (name == 'numOfFamily') {
      const currentVal = this.step1FormGroup.get('numOfFamily')?.value || 0;
      if (currentVal != 0 || value != -1) {
        this.step1FormGroup.patchValue({
          numOfFamily: currentVal + value,
        });
      }
    }
  }
  toggleYard() {
    let checked = this.step1FormGroup.value.yardAvail;
    if (checked) {
      this.step1FormGroup.get('yardInfo')?.enable();
    } else {
      this.step1FormGroup.get('yardInfo')?.disable();
    }
  }
  toggleExp() {
    let checked = this.step2FormGroup.value.pastExp;
    if (checked) {
      this.step2FormGroup.get('yourExp')?.enable();
    } else {
      this.step2FormGroup.get('yourExp')?.disable();
    }
  }
  getMeBack() {
    this.location.back();
  }
}
