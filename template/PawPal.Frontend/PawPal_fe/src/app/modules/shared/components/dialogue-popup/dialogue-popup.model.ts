export type DialogueType = 'success' | 'error' | 'info' | 'warning';

export interface DialoguePopup {
  id: number;
  type: DialogueType;
  title: string;
  message: string;
  buttonText?: string;
  secondaryButtonText?: string;
  delete: boolean;
}