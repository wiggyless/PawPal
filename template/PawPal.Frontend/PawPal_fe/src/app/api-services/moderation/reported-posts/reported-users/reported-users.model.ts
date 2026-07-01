import { BasePagedQuery } from '../../../../core/models/paging/base-paged-query';

export enum ReportUserEnum {
  AIGen = 0,
  Misinformation = 1,
  HateSpeech = 2,
  AnimalAbuse = 3,
  Graphic = 4,
  Stolen = 5,
  Other = 6,
}
export const REPORT_REASON_USER_LABELS: { value: ReportUserEnum; label: string }[] = [
  { value: ReportUserEnum.AIGen, label: 'Tung utung sahur' },
  { value: ReportUserEnum.Misinformation, label: 'Tung utung sahur' },
  { value: ReportUserEnum.HateSpeech, label: 'Tung utung sahur' },
  { value: ReportUserEnum.AnimalAbuse, label: 'Tung utung sahur' },
  { value: ReportUserEnum.Graphic, label: 'Tung utung sahur' },
  { value: ReportUserEnum.Stolen, label: 'TTung utung sahur' },
  { value: ReportUserEnum.Other, label: 'Tung utung sahur' },
];

export interface CreateReportedUserCommand {
  reason: ReportUserEnum;
  description?: string;
  reportedUserID: number;
  reportCreatedByUserID: number;
  dateSent: Date;
}
export interface GetUserReportDto {
  reportID: number;
  reason: ReportUserEnum;
  description?: string;
  reportedByUserID: number;
  reportedByUsername: string;
  userReportedID: number;
  dateSent: Date;
}
export class GetUserReportQuery extends BasePagedQuery {
  dateSentMin?: Date;
  dateSentMax?: Date;
  username?: string;
}
