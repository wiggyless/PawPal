import { Injectable, signal } from '@angular/core';
import { DialoguePopup, DialogueType } from './dialogue-popup.model';

@Injectable({ providedIn: 'root' })
export class DialoguePopupService {
  dialoguePopups = signal<DialoguePopup[]>([]);
  private nextId = 0;

  private add(type: DialogueType, title: string, message: string, buttonText?: string, secondaryButtonText?: string, duration = 10000) {
    const id = this.nextId++;
    this.dialoguePopups.update(n => [...n, { id, type, title, message, buttonText, secondaryButtonText, delete:false}]);
    setTimeout(() => this.dismiss(id), duration);
  }

  success(title: string, message: string, buttonText?: string) { this.add('success', title, message, buttonText); }
  error(title: string, message: string, buttonText?: string)   { this.add('error',   title, message, buttonText); }
  info(title: string, message: string, buttonText?: string)    { this.add('info',    title, message, buttonText); }
  //we will use this 'warning' thing for sensitive actions like deleting an account or post :3
  warning(title: string, message: string, buttonText?: string, secondaryButtonText?: string) { this.add('warning', title, message, buttonText, secondaryButtonText); }

  dismiss(id: number) {
    this.dialoguePopups.update(n => n.filter(x => x.id !== id));
  }
}