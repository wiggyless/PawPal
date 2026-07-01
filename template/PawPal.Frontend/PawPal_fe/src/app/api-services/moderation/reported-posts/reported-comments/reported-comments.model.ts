export enum ReportCommentEnum {
  AIGen = 0,
  Misinformation = 1,
  HateSpeech = 2,
  AnimalAbuse = 3,
  Graphic = 4,
  Stolen = 5,
  Other = 6,
}

export const REPORT_REASON_COMMENTS_LABELS: { value: ReportCommentEnum; label: string }[] = [
  { value: ReportCommentEnum.AIGen, label: 'This post was created using Generative AI' },
  { value: ReportCommentEnum.Misinformation, label: 'This post contains misinformation' },
  { value: ReportCommentEnum.HateSpeech, label: 'This post promotes hate speech' },
  {
    value: ReportCommentEnum.AnimalAbuse,
    label: 'This post shows signs of animal abuse or neglect',
  },
  { value: ReportCommentEnum.Graphic, label: 'This post contains inappropriate or graphic images' },
  { value: ReportCommentEnum.Stolen, label: 'This post is rehoming an animal that may be stolen' },
  { value: ReportCommentEnum.Other, label: 'Other' },
];
export interface CommentDto {
  id: number;
  content: string;
  datePosted: Date;
  postId: number;
  userId: number;
}

export interface CreateReportedCommentCommand {
  reason: ReportCommentEnum;
  description?: string;
  commentID: number;
  commentReportedByID: number;
  dateReported: Date;
}
export interface GetReportedCommentQuery {
  dateSentMin?: Date;
  dateSentMax?: Date;
}
export interface GetReportedCommentDto {
  id: number;
  reason: number;
  description: string;
  comment: CommentDto;
  userID: number;
  commentReportedByID: number;
  username: string;
  dateReported: Date;
  photoURL: string;
}
