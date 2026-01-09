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
