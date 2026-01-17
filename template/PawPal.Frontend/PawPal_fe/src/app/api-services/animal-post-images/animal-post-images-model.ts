export interface GetPostImageById {
  id: number;
  postID: number;
  postImages: Array<string>;
}
export interface AddNewPostImages {
  postId: number;
  postImages: Array<File>;
}
export interface UpdateNewPostImages {
  postID: number;
  postImages: File;
}
export interface GetImagePostBlob {
  postID: number;
  postImages: string[];
}
