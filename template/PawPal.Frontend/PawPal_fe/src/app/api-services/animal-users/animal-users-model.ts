export interface GetUserByIdDto {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  dateTime: string;
  city: string;
  cityID: number;
  cantonAbbrevation: string;
  userName: string;
}
export interface UpdateUserCommand {
  firstName: string;
  lastName: string;
  profilePictureURL: string;
  date: string;
  cityId: number;
}

export interface DeleteUserCommand {
  id: number;
}
