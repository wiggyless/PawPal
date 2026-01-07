export interface GetAnimalsHealthByIdDto {
  animalHealthHistoryId: number;
  animalId: number;
  vaccinated: boolean;
  parasiteFree: boolean;
  spayedOrNeutered: boolean;
  dietaryRestrictions: string;
  animalAllergies: Array<string>;
  animalDisabilities: Array<string>;
}
