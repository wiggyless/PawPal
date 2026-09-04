import { BasePagedQuery } from '../../core/models/paging/base-paged-query';

export interface ListNewsQueryDto {
  id: number;
  title: string;
  content: string;
  publishedAt: string;
  photoURL?: string;
}

export interface GetNewsByIdQueryDto {
  id: number;
  title: string;
  content: string;
  publishedAt: string;
  photoURL?: string;
}

export class ListNewsQuery extends BasePagedQuery {
  search?: string;
  publishedFrom?: string | Date | null;
  publishedTo?: string | Date | null;
  hasPhoto?: boolean | null;
  sortDescending?: boolean;
}

export interface CreateNewsRequest {
  title: string;
  content: string;
  photo: File;
}

export interface UpdateNewsRequest {
  title: string;
  content: string;
  photo?: File;
}
