export interface ListAllergyQueryDto {
  allergyID: number;
  name: string;
}

export class AllergyQueryDto {
  allergyName: string = '';
  constructor(allName: string) {
    this.allergyName = allName;
  }
}
