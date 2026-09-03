import { SafeUrl } from '@angular/platform-browser';
import { BasePagedQuery } from '../../core/models/paging/base-paged-query';

export interface CreateUserCommand {
  firstName: string;
  lastName: string;
  birthDate: Date;
  username: string;
  email: string;
  password: string;
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
  photoURL: string;
  disabled?: boolean;
}
// Public, redacted view of a user — no email/birthdate/canton/disabled status — for anyone
// viewing someone else's profile (catalog post authorship, public profile page, etc).
export interface GetPublicUserProfileDto {
  id: number;
  firstName: string;
  lastName: string;
  username: string;
  city: string;
  cityID: number;
  aboutMe?: string;
  photoURL: string;
}
export interface GetUserList {
  id: number;
  firstName: string;
  lastName: string;
  username: string;
  email: string;
  photoURL: SafeUrl | null;
}
export class GetUserQuery extends BasePagedQuery {
  searchFirstName?: string;
  searchLastName?: string;
  searchEmail?: string;
  searchUsername?: string;
  disabled?: boolean;
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
  username: string;
  exists: boolean;
}

export interface GetByEmailQueryDto {
  email: string;
  exists: boolean;
}
export interface UpdateUserPassword {
  email: string;
  newPassword: string;
  passwordRecovery: boolean;
  currentPassword?: string;
  answers?: Record<number, string>;
}

export interface RequestEmailChangeCommand {
  newEmail: string;
}
