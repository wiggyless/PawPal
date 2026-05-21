export interface CommentQuery {
  postID: number;
}

export interface CommentDto {
  commentID: number;
  userID: number;
  content: string;
  datePosted: Date;
  username: string;
}

export interface CreateCommentCommand {
  userID: number;
  postID: number;
  content: string;
}
