import { ListCitiesQueryDto } from '../cities/cities.model';
export interface ListCantonsDto {
  id: number | null;
  fullName: string;
  cities: ListCitiesQueryDto[];
}
