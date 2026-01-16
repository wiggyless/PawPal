export interface ListAnimalPostsDto {
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
export interface AnimalPostByIdQuery {
  postID: number;
  animalID: number;
  name: string;
  cityID: number;
  userID: number;
  photoURL: string;
}
export interface listAnimalPostsByUserIdDto {
  postID: number;
  firstImage: string;
  userID: string;
  animalName: string;
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
  photoURL: string;
  dateAdded: Date;
}