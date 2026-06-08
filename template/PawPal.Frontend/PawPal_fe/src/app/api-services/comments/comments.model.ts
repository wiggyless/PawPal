import { SafeUrl } from '@angular/platform-browser';
import { BasePagedQuery } from '../../core/models/paging/base-paged-query';

export interface CommentQuery extends BasePagedQuery {
  postID: number;
}

export interface CommentDto {
  commentID: number;
  userID: number;
  content: string;
  datePosted: Date;
  username: string;
  imageData: SafeUrl | null;
}

export interface CreateCommentCommand {
  userID: number;
  postID: number;
  content: string;
}
