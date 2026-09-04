import { Component, inject, Inject, Optional, signal } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NewsService } from '../../../../api-services/news/news.service';
import { DialoguePopupService } from '../../../../api-services/dialogue-popup/dialogue-popup.service';
import { GetNewsByIdQueryDto } from '../../../../api-services/news/news.model';
import { environment } from '../../../../../environments/environment';

export interface AddNewsDialogData {
  news?: GetNewsByIdQueryDto;
}

@Component({
  selector: 'app-add-news-dialog',
  standalone: false,
  templateUrl: './add-news-dialog.html',
  styleUrl: './add-news-dialog.scss',
})
export class AddNewsDialog {
  private fb = inject(FormBuilder);
  dialogRef = inject(MatDialogRef<AddNewsDialog>);
  newsService = inject(NewsService);
  dialoguePopup = inject(DialoguePopupService);
  env = environment;

  isEditMode = false;
  newsId: number | null = null;

  form = this.fb.group({
    title: ['', Validators.required],
    content: ['', Validators.required],
  });

  selectedPhoto: File | null = null;
  photoPreview = signal<string | null>(null);
  isSubmitting = signal(false);

  constructor(@Optional() @Inject(MAT_DIALOG_DATA) public data: AddNewsDialogData | null) {
    if (data?.news) {
      this.isEditMode = true;
      this.newsId = data.news.id;
      this.form.setValue({ title: data.news.title, content: data.news.content });
      if (data.news.photoURL) {
        this.photoPreview.set(this.env.apiUrl + '/' + data.news.photoURL);
      }
    }
  }

  onPhotoSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    this.selectedPhoto = file;
    const reader = new FileReader();
    reader.onload = () => this.photoPreview.set(reader.result as string);
    reader.readAsDataURL(file);
  }

  close(): void {
    this.dialogRef.close(false);
  }

  submit(): void {
    if (this.form.invalid || (!this.isEditMode && !this.selectedPhoto)) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    const title = this.form.value.title as string;
    const content = this.form.value.content as string;

    const onSuccess = () => {
      this.isSubmitting.set(false);
      this.dialoguePopup.success(
        this.isEditMode ? 'News updated' : 'News published',
        this.isEditMode ? 'Your changes have been saved.' : 'Your news post is now live.',
        'OK',
      );
      this.dialogRef.close(true);
    };
    const onError = () => {
      this.isSubmitting.set(false);
      this.dialoguePopup.error('Something went wrong', 'Could not save the news post.', 'OK');
    };

    if (this.isEditMode) {
      this.newsService
        .updateNews(this.newsId as number, { title, content, photo: this.selectedPhoto ?? undefined })
        .subscribe({ next: onSuccess, error: onError });
    } else {
      this.newsService
        .createNews({ title, content, photo: this.selectedPhoto as File })
        .subscribe({ next: onSuccess, error: onError });
    }
  }
}
