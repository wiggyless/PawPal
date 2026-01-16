export interface GetAnimalsHealthByIdDto {
  animalHealthHistoryId: number;
  animalId: number;
  parasiteFree: boolean;
  vaccinated: boolean;
  spayedOrNeutered: boolean;
  dietaryRestrictions: string;
  animalAllergies: Array<string>;
  animalDisabilities: Array<string>;
}

export interface AddAnimalHealthHistory{
  animalId: number;
  parasiteFree: boolean;
  vaccinated: boolean;
  spayedOrNeutered: boolean;
  dietaryRestrictions: string;
  animalAllergies: Array<string>;
  animalDisabilities: Array<string>;
}