import { BasePagedQuery } from '../../../core/models/paging/base-paged-query';

export interface PostSecurityQuestion {
  question: string;
}
export interface GetSecurityQuestionsQuery extends BasePagedQuery {}
export interface GetSecurityQuestionsQueryByEmail extends BasePagedQuery {
  email: string;
}
export interface GetSecurityQuestionDTO {
  id: number;
  question: string;
}
export interface UpdateAndDeleteQuestionDTO {
  id: number;
}
