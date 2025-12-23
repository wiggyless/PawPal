
//ovo je za gettanje svih kategorija zivotinja
export interface ListAnimalCategoriesQueryDto {
    id:number;
    categoryName:string;
    isEnabled:boolean;
}

//ovo je za gettanje jedne kategorije zivotinja
export interface AnimalCategoryByIdQueryDto {
    id:number;
    categoryName:string;
    isEnabled:boolean;
}