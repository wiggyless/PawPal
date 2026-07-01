export interface UserImageQuery {
  userID: number;
}

export interface UserImageDto {
  imageData: Blob;
}

export interface UserImageCommand {
  userID: number;
  imageData?: FormData;
}
export interface GetUserImageById {
  id: number;
  userID: number;
  photoURL: string;
}
