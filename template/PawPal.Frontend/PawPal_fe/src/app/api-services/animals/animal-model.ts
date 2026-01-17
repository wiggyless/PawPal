export interface ListAnimalsDto {
  name: string;
  breed: string;
  gender: string;
  category: string;
}
export interface GetAnimalByIdDto {
  id: number;
  name: string;
  breed: string;
  gender: string;
  age: number;
  category: string;
  hasPapers: boolean;
  childFriendly: boolean;
}
export interface AddAnimalDto {
  name: string;
  breed: string;
  genderId: number;
  age: number;
  hasPapers: boolean;
  childFriendly: boolean;
  categoryId: number;
}
export interface UpdateAnimalDto {
  name: string;
  breed: string;
  gender: string;
  age: number;
  hasPapers: true;
  childFriendly: true;
  category: string;
  categoryID: number;
}
