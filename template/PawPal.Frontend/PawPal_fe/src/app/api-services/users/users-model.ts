export interface CreateUserCommand{
        firstName: string;
    lastName: string;
    birthDate: Date;
    username: string;
    email: string;
    password: string;
    roleID: number;
    city: string | number;
    profilePictureURL?: string | null;

}
export interface GetUserByIdDto {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  dateTime: string;
  city: string;
  cityID: number;
  cantonAbbrevation: string;
  username: string;
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

export interface GetByUsernameQueryDto{
    firstName: string;
    lastName: string;
    email: string;
    username: string;
}

export interface GetByEmailQueryDto{
    firstName: string;
    lastName: string;
    email: string;
    username: string;
}