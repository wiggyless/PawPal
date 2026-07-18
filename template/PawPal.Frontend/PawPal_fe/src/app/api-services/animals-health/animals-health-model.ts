import { AllergyQueryDto } from '../allergies/allergy-model';
import { DisabilitiesDto } from '../disabilities/disability-model';

export interface GetAnimalsHealthByIdDto {
  animalHealthHistoryId: number;
  animalId: number;
  parasiteFree: boolean;
  vaccinated: boolean;
  spayedOrNeutered: boolean;
  dietaryRestrictions: string;
  animalAllergies: Array<GetAllergyFromAnimalDto>;
  animalDisabilities: Array<GetDisabilityFromAnimalDto>;
}

export interface GetAllergyFromAnimalDto {
  allergyName: string;
  allergyDescription: string;
}

export interface GetDisabilityFromAnimalDto {
  disabilityName: string;
  disabilityDescription: string;
}
export interface AddAnimalHealthHistory {
  animalId: number;
  parasiteFree: boolean;
  vaccinated: boolean;
  spayedOrNeutered: boolean;
  dietaryRestrictions: string;
  allergies: Array<string>;
  disabilities: Array<string>;
}

export interface UpdateHealthHistory {
  animalId: number;
  vaccinated: boolean;
  spayedOrNeutered: boolean;
  parasiteFree: boolean;
  dietaryRestrictions: string;
  allergies: Array<AllergyQueryDto>;
  disabilities: Array<DisabilitiesDto>;
}
