import { BasePagedQuery } from '../../core/models/paging/base-paged-query';

export class GetLikedPostListQuery {
  userId?: number;
  postIdList?: number[];
}
export interface GetLikedPostList {
  userId?: number;
  postList?: number[];
}
export interface LikePost {
  userID: number;
  postID: number;
}
export interface DeleteLikedPost {
  userId: number;
  postId: number;
}
