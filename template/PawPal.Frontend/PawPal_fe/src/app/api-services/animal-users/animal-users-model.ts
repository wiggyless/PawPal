export interface GetUserByIdDto {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  dateTime: string;
  city: string;
  cityID: number;
  cantonAbbrevation: string;
}
export interface UpdateUserCommand{
  firstName: string;
  lastName: string;
  profilePictureURL: string;
  date: string;
  cityId: number;
}