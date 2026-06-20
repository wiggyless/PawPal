import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DomSanitizer } from '@angular/platform-browser';
import { ImageCroppedEvent, ImageCropperComponent } from 'ngx-image-cropper'; // ← both from same library

export interface CropDialogData {
  imageFile: File;
}
export interface CropDialogResult {
  croppedFile: File;
  croppedUrl: string;
}

@Component({
  selector: 'app-user-profile-image-crop-dialog',
  standalone: false,
  templateUrl: './user-profile-image-crop-dialog.html',
  styleUrl: './user-profile-image-crop-dialog.scss',
})
export class UserProfileImageCropDialog implements OnInit {
  imageWidth: number = 0;
  imageHeight: number = 0;

  croppedEvent: ImageCroppedEvent | null = null;
  rotation = 0;

  constructor(
    public dialogRef: MatDialogRef<ImageCropperComponent, CropDialogResult>,
    @Inject(MAT_DIALOG_DATA) public data: CropDialogData,
    private sanitizer: DomSanitizer,
  ) {}

  ngOnInit(): void {
    const url = URL.createObjectURL(this.data.imageFile);
    const img = new Image();
    img.onload = () => {
      const maxWidth = Math.min(img.naturalWidth, 1200);
      const maxHeight = Math.min(img.naturalHeight, 800);
      const ratio = img.naturalWidth / img.naturalHeight;
      let dialogWidth: number;
      let dialogHeight: number;
      if (ratio > 1) {
        dialogWidth = maxWidth;
        dialogHeight = maxWidth / ratio;
      } else {
        dialogHeight = maxHeight;
        dialogWidth = maxHeight * ratio;
      }
      this.dialogRef.updateSize(`${dialogWidth}px`, `${dialogHeight + 130}px`);
      URL.revokeObjectURL(url);
    };
    img.src = url;
  }

  onCropped(event: ImageCroppedEvent): void {
    this.croppedEvent = event;
  }

  onLoadFailed(): void {
    this.dialogRef.close();
  }

  async onConfirm(): Promise<void> {
    if (!this.croppedEvent?.blob) return;
    const blob = this.croppedEvent.blob;
    const file = new File([blob], this.data.imageFile.name, { type: 'image/png' });
    const croppedUrl = this.croppedEvent.objectUrl ?? URL.createObjectURL(blob);
    this.dialogRef.close({ croppedFile: file, croppedUrl });
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}