export interface ListAnimalPostsDto {
  postID: number;
  animalID: number;
  cityID: number;
  categoryID: number;
  genderID: number;
  name: string;
  age: number;
  breed: string;
  userId: number;
  photoURL: string;
  dateAdded: Date;
}
export interface AnimalPostByIdQuery {
  postID: number;
  animalID: number;
  name: string;
  cityID: number;
  userId: number;
  photoURL: string;
}
