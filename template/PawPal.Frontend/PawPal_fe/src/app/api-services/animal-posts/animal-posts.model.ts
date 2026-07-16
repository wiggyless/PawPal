import { BasePagedQuery } from '../../core/models/paging/base-paged-query';

export class GetPostQuery extends BasePagedQuery {
  userID?: number;
  searchCityName?: string | null;
  searchCategoryName?: string;
  searchBreed?: string;
  searchGender?: string;
  searchDateAddedMax?: Date;
  searchDateAddedMin?: Date;
  searchCantonId?: number;
  isLiked?: boolean;
  isRequest?: boolean;
}
export class GetListPostQueryByUserID extends BasePagedQuery {
  userID?: number;
}

export interface AnimalPostByIdQuery {
  postID: number;
  animalID: number;
  cityID: number;
  categoryID: any;
  userID: number;
  genderID: any;
  name: string;
  age: number;
  breed: string;
  photoURL: string;
  dateAdded: Date;
}

export interface AddAnimalPost {
  animalID: number;
  cityID: number;
  userId: number;
  status: boolean;
}

export interface ListAnimal {
  postID: number;
  animalID: number;
  cityID: number;
  categoryID: number;
  userID: number;
  genderID: number;
  name: string;
  age: number;
  breed: string;
  dateAdded: Date;
  mainImage: string;
}
export class ListPostsByRange extends BasePagedQuery {
  postIdList?: number[];
}
