import { ChangeDetectorRef, Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { NewsService } from '../../../../api-services/news/news.service';
import { GetNewsByIdQueryDto } from '../../../../api-services/news/news.model';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-news-detail',
  standalone: false,
  templateUrl: './news-detail.html',
  styleUrl: './news-detail.scss',
})
export class NewsDetailComponent implements OnInit {
  route = inject(ActivatedRoute);
  router = inject(Router);
  currentUser = inject(CurrentUserService);
  newsService = inject(NewsService);
  sanitizer = inject(DomSanitizer);
  cd = inject(ChangeDetectorRef);
  env = environment;

  newsId = 0;
  news: GetNewsByIdQueryDto | undefined;
  isLoaded = signal(false);

  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.route.paramMap.subscribe((params) => {
      this.newsId = Number(params.get('id'));
      this.loadNews();
    });
  }

  loadNews(): void {
    this.isLoaded.set(false);
    this.newsService.getNewsById(this.newsId).subscribe({
      next: (res) => {
        this.news = res;
        this.isLoaded.set(true);
        this.cd.detectChanges();
      },
      error: () => {
        this.router.navigate(['news']);
      },
    });
  }

  getImageUrl(photoUrl?: string): SafeUrl {
    return this.sanitizer.bypassSecurityTrustUrl(this.env.apiUrl + '/' + (photoUrl ?? ''));
  }

  goBack(): void {
    this.router.navigate(['news']);
  }
}
