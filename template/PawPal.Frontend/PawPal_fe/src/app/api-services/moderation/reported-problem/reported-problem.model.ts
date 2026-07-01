import { BasePagedQuery } from '../../../core/models/paging/base-paged-query';

export class ReportedProblemsQuery extends BasePagedQuery {
  dateMin?: Date;
  dateMax?: Date;
  username?: string;
}
export interface ReportedProblemsDto {
  description: string;
  userID: number;
  id: number;
  username: string;
  dateSent: Date;
}
export interface CreateReportedProblem {
  description: string;
  userID: number;
  dateSent: Date;
}
