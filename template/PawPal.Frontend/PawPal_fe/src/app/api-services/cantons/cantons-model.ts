import { ListCitiesQueryDto } from '../cities/cities.model';
export interface ListCantonsDto {
  id: number;
  fullName: string;
  cities: ListCitiesQueryDto[];
}
