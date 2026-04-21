import { BasePagedQuery } from '../../core/models/paging/base-paged-query';

export interface CreateAdoptionRequest {
  status: string;
  dateSend: Date;
  userID: number;
  postID: number;
  requirementID: number;
}
export interface GetAdoptionRequestListQuery extends BasePagedQuery {
  userID: number;
  searchStatus?: string;
  searchDateSent?: string;
  sent: boolean;
}
export interface GetAdoptionRequestList {
  requestId: number;
  status: string;
  dateSent: Date;
  postID: number;
  requirementId: number;
  gender: string;
  name: string;
  city: string;
  canton: string;
  breed: string;
}
export interface GetAdoptionRequestById {
  requestId: number;
  status: string;
  dateSent: Date;
  userId: number;
  postId: number;
  requirementId: number;
}

// by PostID

export interface GetAdoptionRequestListByPostIDQuery extends BasePagedQuery {
  postID: number;
}

export interface GetAdoptionRequestByPostID {
  requestID: number;
  userName: string;
  cityName: string;
  cantonName: string;
  status: string;
  postID: number;
  dateSent: Date;
  requirementId: number;
}

export interface UpdateRequestByID {
  requestID: number;
  status: string;
}
