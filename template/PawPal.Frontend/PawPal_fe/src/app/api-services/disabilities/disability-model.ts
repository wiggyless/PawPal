export interface GetDisabilityByDto {
  disabilityID: number;
  name: string;
  description: string;
}

export interface ListDisabilitiesQueryDto {
  disabilityID: number;
  name: string;
}
export class DisabilitiesDto {
  name: string = '';
  constructor(disName: string) {
    this.name = disName;
  }
}
