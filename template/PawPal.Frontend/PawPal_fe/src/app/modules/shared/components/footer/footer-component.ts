import { Component, inject, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer',
  standalone: true,
    templateUrl: './footer-component.html',
    styleUrl: './footer-component.scss',
})
export class FooterComponent implements OnInit {
    ngOnInit(): void {
        console.log("Footer loaded");
    }
    clearText(): void{
        console.log("pressed")
        const textarea = document.querySelector('.footer-left textarea') as HTMLTextAreaElement;
        if (textarea) {
            textarea.value = '';
        }
    }
}