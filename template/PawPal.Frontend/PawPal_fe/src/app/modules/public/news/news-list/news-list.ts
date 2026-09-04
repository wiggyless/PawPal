import { ChangeDetectorRef, Component, inject, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { FormControl, FormGroup } from '@angular/forms';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { NewsService } from '../../../../api-services/news/news.service';
import { ListNewsQuery, ListNewsQueryDto } from '../../../../api-services/news/news.model';
import { environment } from '../../../../../environments/environment';
import { AddNewsDialog } from '../add-news-dialog/add-news-dialog';

@Component({
  selector: 'app-news-list',
  standalone: false,
  templateUrl: './news-list.html',
  styleUrl: './news-list.scss',
})
export class NewsListComponent implements OnInit {
  currentUser = inject(CurrentUserService);
  newsService = inject(NewsService);
  router = inject(Router);
  dialog = inject(MatDialog);
  sanitizer = inject(DomSanitizer);
  cd = inject(ChangeDetectorRef);
  env = environment;

  newsItems: ListNewsQueryDto[] = [];
  isLoaded = signal(false);

  request = new ListNewsQuery();
  page = {
    pageSize: 8,
    currentPage: 1,
    totalItems: 0,
    totalPages: 0,
    pageSizeOption: [8, 16, 32],
  };

  filterForm = new FormGroup({
    search: new FormControl<string>(''),
    hasPhoto: new FormControl<boolean | null>(null),
    sortDescending: new FormControl<boolean>(true),
  });
  datePicker = new FormGroup({
    start: new FormControl<Date | null>(null),
    end: new FormControl<Date | null>(null),
  });

  constructor() {
    this.request.paging.page = 1;
    this.request.paging.pageSize = this.page.pageSize;
  }

  ngOnInit(): void {
    this.loadNews();
  }

  searchNews(): void {
    this.request = {
      ...this.request,
      search: this.filterForm.value.search ?? undefined,
      hasPhoto: this.filterForm.value.hasPhoto ?? undefined,
      sortDescending: this.filterForm.value.sortDescending ?? true,
      publishedFrom: this.datePicker.value.start ?? undefined,
      publishedTo: this.datePicker.value.end ?? undefined,
    };
    this.request.paging.page = 1;
    this.loadNews();
  }

  clearFilters(): void {
    this.filterForm.reset({ search: '', hasPhoto: null, sortDescending: true });
    this.datePicker.reset({ start: null, end: null });
    this.searchNews();
  }

  loadNews(): void {
    this.isLoaded.set(false);
    this.newsService.listNews(this.request).subscribe({
      next: (res) => {
        this.newsItems = res.items;
        this.page = {
          pageSize: res.pageSize,
          currentPage: res.currentPage,
          totalItems: res.totalItems,
          totalPages: res.totalPages,
          pageSizeOption: this.page.pageSizeOption,
        };
        this.isLoaded.set(true);
        this.cd.detectChanges();
      },
      error: () => {
        this.isLoaded.set(true);
        this.cd.detectChanges();
      },
    });
  }

  handlePageEvent(event: PageEvent): void {
    this.request.paging.page = event.pageIndex + 1;
    this.request.paging.pageSize = event.pageSize;
    this.loadNews();
  }

  getImageUrl(photoUrl?: string): SafeUrl {
    return this.sanitizer.bypassSecurityTrustUrl(this.env.apiUrl + '/' + (photoUrl ?? ''));
  }

  getPreview(content: string, limit: number = 30): string {
    return content.length > limit ? content.slice(0, limit).trimEnd() + '…' : content;
  }

  routeToNews(item: ListNewsQueryDto): void {
    this.router.navigate(['news', item.id]);
  }

  openAddNewsDialog(): void {
    const dialogRef = this.dialog.open(AddNewsDialog);
    dialogRef.afterClosed().subscribe((created) => {
      if (created) {
        this.loadNews();
      }
    });
  }
}
