import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
@Component({
  selector: 'app-catalog',
  standalone: false,
  templateUrl: './catalog.html',
  styleUrl: './catalog.scss',
})
export class CatalogComponent extends BaseComponent {}
