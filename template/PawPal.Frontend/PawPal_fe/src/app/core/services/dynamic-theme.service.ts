import { Injectable, signal, effect } from '@angular/core';

export type Theme = 'light' | 'dark';

@Injectable({
  providedIn: 'root',
})
export class DyanmicThemeService {
  private currentTheme = signal<Theme>((localStorage.getItem('theme') as Theme) || 'light');

  theme = this.currentTheme.asReadonly();

  constructor() {
    effect(() => {
      const activeTheme = this.currentTheme();
      localStorage.setItem('theme', activeTheme);
      this.updateRenderedTheme(activeTheme);
    });
  }

  toggleTheme(): void {
    this.currentTheme.update((theme) => (theme === 'light' ? 'dark' : 'light'));
  }

  private updateRenderedTheme(theme: Theme): void {
    const body = document.body;
    if (theme === 'dark') {
      body.classList.add('dark-theme');
    } else {
      body.classList.remove('dark-theme');
    }
  }
}
