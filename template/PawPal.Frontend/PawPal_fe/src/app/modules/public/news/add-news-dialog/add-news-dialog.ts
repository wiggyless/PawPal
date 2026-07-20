import { Component, inject, signal } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { NewsService } from '../../../../api-services/news/news.service';
import { DialoguePopupService } from '../../../../api-services/dialogue-popup/dialogue-popup.service';

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

  form = this.fb.group({
    title: ['', Validators.required],
    content: ['', Validators.required],
  });

  selectedPhoto: File | null = null;
  photoPreview = signal<string | null>(null);
  isSubmitting = signal(false);

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
    if (this.form.invalid || !this.selectedPhoto) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.newsService
      .createNews({
        title: this.form.value.title as string,
        content: this.form.value.content as string,
        photo: this.selectedPhoto,
      })
      .subscribe({
        next: () => {
          this.isSubmitting.set(false);
          this.dialoguePopup.success('News published', 'Your news post is now live.', 'OK');
          this.dialogRef.close(true);
        },
        error: () => {
          this.isSubmitting.set(false);
          this.dialoguePopup.error('Something went wrong', 'Could not publish the news post.', 'OK');
        },
      });
  }
}
