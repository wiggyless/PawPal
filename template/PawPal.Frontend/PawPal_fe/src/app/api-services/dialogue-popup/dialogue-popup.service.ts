import { Injectable, signal } from '@angular/core';
import { DialoguePopup, DialogueType } from './dialogue-popup.model';

@Injectable({ providedIn: 'root' })
export class DialoguePopupService {
  dialoguePopups = signal<DialoguePopup[]>([]);
  private nextId = 0;

  private add(type: DialogueType, title: string, message: string, buttonText?: string, secondaryButtonText?: string, onConfirm?: () => void, onSecondary?: () => void, duration = 10000) {
  const id = this.nextId++;
  this.dialoguePopups.update(n => [...n, { id, type, title, message, buttonText, secondaryButtonText, delete: false, onConfirm, onSecondary }]);
  setTimeout(() => this.dismiss(id), duration);
}

warning(title: string, message: string, buttonText?: string, secondaryButtonText?: string, onConfirm?: () => void, onSecondary?: () => void) {
  this.add('warning', title, message, buttonText, secondaryButtonText, onConfirm, onSecondary);
}
  success(title: string, message: string, buttonText?: string) { this.add('success', title, message, buttonText); }
  error(title: string, message: string, buttonText?: string)   { this.add('error',   title, message, buttonText); }
  info(title: string, message: string, buttonText?: string)    { this.add('info',    title, message, buttonText); }

  dismiss(id: number) {
    this.dialoguePopups.update(n => n.filter(x => x.id !== id));
  }
}