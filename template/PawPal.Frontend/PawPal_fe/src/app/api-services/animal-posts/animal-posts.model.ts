import { BasePagedQuery } from '../../core/models/paging/base-paged-query';

export class GetPostQuery extends BasePagedQuery {
  searchCityName?: string | null;
  searchCategoryName?: string;
  searchBreed?: string;
  searchGender?: string;
  searchDateAddedMax?: Date;
  searchDateAddedMin?: Date;
}

// List the animals posts (forgor to add Post in the interface name)
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
  photoURL: string;
  dateAdded: Date;
}

// Get a list of posts when looking by UserID
export interface listAnimalPostsByUserIdDto {
  postId: number;
  firstImage: string;
  userId: string;
  animalName: string;
  cityID: number;
  animalID: number;
  dateAdded: Date;
}

// Get Post by Id
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

// Post new ANimal
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
  photoURL: string;
  dateAdded: Date;
}