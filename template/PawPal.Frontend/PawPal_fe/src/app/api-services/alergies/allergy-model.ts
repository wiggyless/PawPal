export interface ListAllergyQueryDto {
  allergyID: number;
  name: string;
}

export class AllergyQueryDto {
  name: string = '';
  constructor(allName: string) {
    this.name = allName;
  }
}
