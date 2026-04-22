import { BasePagedQuery } from '../../core/models/paging/base-paged-query';

export interface CreateAdoptionRequirement {
  houseType: string;
  address: string;
  floorNumber: number;
  peopleCount: number;
  childrenAround: boolean;
  elderlyAround: boolean;
  otherPetsAround: boolean;
  yardAvailable: boolean;
  yardDetails: string;
  petExp: boolean;
  expDetails: string;
  peopleAva: string;
  isGift: boolean;
  planedStay: string;
  sumMoney: number;
  allergy: boolean;
  aggressiveness: boolean;
  takeBack: boolean;
  houseDetials: string;
  finalComment: string;
}

export interface GetAdoptionRequirementQuery extends BasePagedQuery {
  searchHouseType: string;
  searchPeopleCount: string;
  searchChildrenAround: string;
  searchOtherPetsAround: string;
  searchYardAvailable: string;
}
export interface GetAdoptionRequirementList {
  id: number;
  houseType: string;
  address: string;
  floorNumber: number;
  peopleCount: number;
  childrenAround: boolean;
  elderlyAround: boolean;
  otherPetsAround: boolean;
  yardAvailable: boolean;
  yardDetails: string;
  petExp: boolean;
  expDetails: string;
  peopleAva: string;
  isGift: boolean;
  planedStay: string;
  sumMoney: number;
  allergy: boolean;
  aggressiveness: boolean;
  takeBack: boolean;
  houseDetials: string;
  finalComment: string;
}
export interface GetAdoptionRequirementsById {
  id: number;
  houseType: string;
  address: string;
  floorNumber: number;
  peopleCount: number;
  childrenAround: boolean;
  elderlyAround: boolean;
  otherPetsAround: boolean;
  yardAvailable: boolean;
  yardDetails: string;
  petExp: boolean;
  expDetails: string;
  peopleAva: string;
  isGift: boolean;
  planedStay: string;
  sumMoney: number;
  allergy: boolean;
  aggressiveness: boolean;
  takeBack: boolean;
  houseDetials: string;
  finalComment: string;
}
export interface UpdateAdoptionRequirements {
  id: number;
  houseType: string;
  address: string;
  floorNumber: number;
  peopleCount: number;
  childrenAround: boolean;
  elderlyAround: boolean;
  otherPetsAround: boolean;
  yardAvailable: boolean;
  yardDetails: string;
  petExp: boolean;
  expDetails: string;
  peopleAva: string;
  isGift: boolean;
  planedStay: string;
  sumMoney: number;
  allergy: boolean;
  aggressiveness: boolean;
  takeBack: boolean;
  houseDetials: string;
  finalComment: string;
}
