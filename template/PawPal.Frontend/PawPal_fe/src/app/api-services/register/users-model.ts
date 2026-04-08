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

export interface GetByUsernameQueryDto{
    firstName: string;
    lastName: string;
    email: string;
    username: string;
}