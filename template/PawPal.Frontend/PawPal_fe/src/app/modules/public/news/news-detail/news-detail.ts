import { ChangeDetectorRef, Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { NewsService } from '../../../../api-services/news/news.service';
import { GetNewsByIdQueryDto } from '../../../../api-services/news/news.model';
import { environment } from '../../../../../environments/environment';
import { AddNewsDialog } from '../add-news-dialog/add-news-dialog';
import { DialoguePopupService } from '../../../../api-services/dialogue-popup/dialogue-popup.service';

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
  dialog = inject(MatDialog);
  dialoguePopup = inject(DialoguePopupService);
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

  openEditDialog(): void {
    if (!this.news) return;
    const dialogRef = this.dialog.open(AddNewsDialog, { data: { news: this.news } });
    dialogRef.afterClosed().subscribe((updated) => {
      if (updated) {
        this.loadNews();
      }
    });
  }

  deleteNews(): void {
    if (!this.news) return;
    const id = this.news.id;

    this.dialoguePopup.warning(
      'Confirm action',
      'Are you sure that you want to delete this news post?',
      'Yes',
      'No',
      () => {
        this.newsService.deleteNews(id).subscribe({
          next: () => {
            this.dialoguePopup.success('Deleted', 'The news post has been deleted.', 'OK');
            this.router.navigate(['news']);
          },
          error: () => {
            this.dialoguePopup.error('Something went wrong', 'Could not delete the news post.', 'OK');
          },
        });
      },
      () => {},
    );
  }
}
