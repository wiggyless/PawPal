import { Component, inject, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer',
  standalone: true,
  templateUrl: './footer-component.html',
  styleUrl: './footer-component.scss',
})
export class FooterComponent implements OnInit {
  ngOnInit(): void {}
  clearText(): void {
    const textarea = document.querySelector('.footer-left textarea') as HTMLTextAreaElement;
    if (textarea) {
      textarea.value = '';
    }
  }
}
