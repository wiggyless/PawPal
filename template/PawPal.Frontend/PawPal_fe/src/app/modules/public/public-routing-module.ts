import { NgModule, OnInit } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PublicLayout } from './public-layout/public-layout';
import { CatalogComponent } from './catalog/catalog/catalog';
import { PostComponent } from './post/post/post';
import { Profile } from './profile/profile/profile';
import { NewsListComponent } from './news/news-list/news-list';
import { NewsDetailComponent } from './news/news-detail/news-detail';
const routes: Routes = [
  {
    path: '',
    component: PublicLayout,
  },
  {
    path: 'catalog',
    component: CatalogComponent,
  },
  {
    path: 'post',
    component: PostComponent,
  },
  {
    path: 'profile',
    component: Profile,
  },
  {
    path: 'news',
    component: NewsListComponent,
  },
  {
    path: 'news/:id',
    component: NewsDetailComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PublicRoutingModule {}
