import { Injectable, signal, inject } from '@angular/core';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { DialoguePopupComponent } from '../../modules/shared/components/dialogue-popup/dialogue-popup.component';
import { DialoguePopup, DialogueType } from './dialogue-popup.model';

@Injectable({ providedIn: 'root' })
export class DialoguePopupService {
  private overlay = inject(Overlay);
  private overlayRef: OverlayRef | null = null;

  dialoguePopups = signal<DialoguePopup[]>([]);
  private nextId = 0;

  private add(
    type: DialogueType,
    title: string,
    message: string,
    buttonText?: string,
    secondaryButtonText?: string,
    onConfirm?: () => void,
    onSecondary?: () => void,
    duration = 10000,
  ) {
    const id = this.nextId++;
    this.dialoguePopups.update((n) => [
      ...n,
      {
        id,
        type,
        title,
        message,
        buttonText,
        secondaryButtonText,
        delete: false,
        onConfirm,
        onSecondary,
      },
    ]);

    if (!this.overlayRef) {
      this.attachToBodyRoot();
    }

    setTimeout(() => this.dismiss(id), duration);
  }

  private attachToBodyRoot() {
    this.overlayRef = this.overlay.create({
      positionStrategy: this.overlay.position().global(),
      scrollStrategy: this.overlay.scrollStrategies.noop(),
    });

    const portal = new ComponentPortal(DialoguePopupComponent);
    this.overlayRef.attach(portal);
  }

  warning(
    title: string,
    message: string,
    buttonText?: string,
    secondaryButtonText?: string,
    onConfirm?: () => void,
    onSecondary?: () => void,
  ) {
    this.add('warning', title, message, buttonText, secondaryButtonText, onConfirm, onSecondary);
  }
  success(title: string, message: string, buttonText?: string) {
    this.add('success', title, message, buttonText);
  }
  error(title: string, message: string, buttonText?: string) {
    this.add('error', title, message, buttonText);
  }
  info(title: string, message: string, buttonText?: string) {
    this.add('info', title, message, buttonText);
  }

  dismiss(id: number) {
    this.dialoguePopups.update((n) => n.filter((x) => x.id !== id));

    if (this.dialoguePopups().length === 0 && this.overlayRef) {
      this.overlayRef.dispose();
      this.overlayRef = null;
    }
  }
}
