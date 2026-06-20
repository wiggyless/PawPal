import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { PublicRoutingModule } from './modules/public/public-routing-module';
import { DialoguePopupComponent } from './modules/shared/components/dialogue-popup/dialogue-popup.component';
import { authInterceptor } from '../app/core/interceptors/auth-interceptor.service';
import { rateLimitInterceptor } from './core/interceptors/rate-limit-interceptor.service';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { provideNativeDateAdapter } from '@angular/material/core';
@NgModule({
  declarations: [App],
  imports: [
    BrowserModule,
    AppRoutingModule,
    PublicRoutingModule,
    DialoguePopupComponent,
    DragDropModule,
  ],
  providers: [
    provideNativeDateAdapter(),
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(
      withFetch(),
      withInterceptors([authInterceptor]),
      withInterceptors([rateLimitInterceptor]),
    ),
  ],
  bootstrap: [App],
})
export class AppModule {}
