export interface CreateUserCommand{
        firstName: string;
    lastName: string;
    birthDate: Date;
    email: string;
    password: string;
    roleID: number;
    city: string | number;
    profilePictureURL?: string | null;

}