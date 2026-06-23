import { BasePagedQuery } from '../../core/models/paging/base-paged-query';

export interface CreateUserCommand {
  firstName: string;
  lastName: string;
  birthDate: Date;
  username: string;
  email: string;
  password: string;
  roleID: number;
  city: string | number;
  aboutMe: string;
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
  aboutMe?: string;
}
export interface GetUserList {
  id: number;
  firstName: string;
  lastName: string;
  username: string;
  email: string;
}
export class GetUserQuery extends BasePagedQuery {
  searchFirstName?: string;
  searchLastName?: string;
  searchEmail?: string;
}
export interface UpdateUserCommand {
  firstName: string;
  lastName: string;
  profilePictureURL: string;
  date: string;
  cityId: number;
  aboutMe?: string;
}

export interface DeleteUserCommand {
  id: number;
}

export interface GetByUsernameQueryDto {
  firstName: string;
  lastName: string;
  email: string;
  username: string;
}

export interface GetByEmailQueryDto {
  firstName: string;
  lastName: string;
  email: string;
  username: string;
}
export interface UpdateUserPassword {
  email: string;
  newPassword: string;
}
