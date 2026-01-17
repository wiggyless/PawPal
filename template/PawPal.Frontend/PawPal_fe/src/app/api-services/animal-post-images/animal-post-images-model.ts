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
export interface GetMainImagePostBlob {
  postID: number;
  mainImage: string;
}
export class GetMainImagePostBlobClass {
  postID: number;
  mainImage: string;
  constructor(id: number, img: string) {
    this.postID = id;
    this.mainImage = img;
  }
}
export class ListMainImageId {
  postID: number;
  constructor(id: number) {
    this.postID = id;
  }
}
