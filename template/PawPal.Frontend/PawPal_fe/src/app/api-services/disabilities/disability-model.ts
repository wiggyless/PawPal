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
  disabilityName: string = '';
  constructor(disName: string) {
    this.disabilityName = disName;
  }
}
