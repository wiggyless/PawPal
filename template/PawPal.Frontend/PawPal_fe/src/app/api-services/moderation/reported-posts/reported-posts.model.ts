export enum ReportPostEnum {
  AIGen = 0,
  Misinformation = 1,
  HateSpeech = 2,
  AnimalAbuse = 3,
  Graphic = 4,
  Stolen = 5,
  Other = 6,
}

export const REPORT_REASON_LABELS: { value: ReportPostEnum; label: string }[] = [
  { value: ReportPostEnum.AIGen, label: 'This post was created using Generative AI' },
  { value: ReportPostEnum.Misinformation, label: 'This post contains misinformation' },
  { value: ReportPostEnum.HateSpeech, label: 'This post promotes hate speech' },
  { value: ReportPostEnum.AnimalAbuse, label: 'This post shows signs of animal abuse or neglect' },
  { value: ReportPostEnum.Graphic, label: 'This post contains inappropriate or graphic images' },
  { value: ReportPostEnum.Stolen, label: 'This post is rehoming an animal that may be stolen' },
  { value: ReportPostEnum.Other, label: 'Other' },
];

export interface CreateReportedPostCommand {
  reason: ReportPostEnum;
  description?: string;
  postID: number;
  userID: number;
  dateSent: Date;
}
export interface GetReportedPostQuery {
  dateSentMin?: Date;
  dateSentMax?: Date;
}
export interface GetReportedPostDto {
  id: number;
  reason: number;
  description: string;
  postID: number;
  userID: number;
  username: string;
  dateSent: Date;
}
