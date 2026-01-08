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
