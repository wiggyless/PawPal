import { AllergyQueryDto, ListAllergyQueryDto } from '../alergies/allergy-model';
import { DisabilitiesDto, ListDisabilitiesQueryDto } from '../disabilities/disability-model';

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

export interface AddAnimalHealthHistory {
  animalId: number;
  parasiteFree: boolean;
  vaccinated: boolean;
  spayedOrNeutered: boolean;
  dietaryRestrictions: string;
  animalAllergies: Array<string>;
  animalDisabilities: Array<string>;
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
