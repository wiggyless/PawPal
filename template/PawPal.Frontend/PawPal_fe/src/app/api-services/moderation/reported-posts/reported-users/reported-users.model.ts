import { BasePagedQuery } from '../../../../core/models/paging/base-paged-query';

// Must stay in the same order as the backend's PawPal.Domain.Entities.Moderation.ReportUserEnum,
// since the numeric value is what's actually sent to/received from the API.
export enum ReportUserEnum {
  Unresponsive = 0,
  AbusiveBehavior = 1,
  Harassment = 2,
  IllegalSelling = 3,
  ScamOrFakeAccount = 4,
  AnimalMistreatmentHistory = 5,
  SpamAccount = 6,
  Other = 7,
}
export const REPORT_REASON_USER_LABELS: { value: ReportUserEnum; label: string }[] = [
  {
    value: ReportUserEnum.Unresponsive,
    label: 'This user is not responding after being confirmed for adoption',
  },
  { value: ReportUserEnum.AbusiveBehavior, label: 'This user showed aggressive or abusive behavior' },
  { value: ReportUserEnum.Harassment, label: 'This user is harassing me or another member' },
  {
    value: ReportUserEnum.IllegalSelling,
    label: 'This user tried to sell an animal instead of adopting it out',
  },
  {
    value: ReportUserEnum.ScamOrFakeAccount,
    label: 'This user appears to be operating a scam or fake account',
  },
  {
    value: ReportUserEnum.AnimalMistreatmentHistory,
    label: 'This user has a history of neglecting or mistreating adopted animals',
  },
  { value: ReportUserEnum.SpamAccount, label: 'This user is spamming or creating multiple fake listings' },
  { value: ReportUserEnum.Other, label: 'Other' },
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
