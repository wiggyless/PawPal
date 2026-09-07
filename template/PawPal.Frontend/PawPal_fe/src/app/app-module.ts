import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { DialoguePopupComponent } from './modules/shared/components/dialogue-popup/dialogue-popup.component';
import { authInterceptor } from '../app/core/interceptors/auth-interceptor.service';
import { rateLimitInterceptor } from './core/interceptors/rate-limit-interceptor.service';
import { errorLoggingInterceptor } from './core/interceptors/error-logging-interceptor.service';
import { loadingBarInterceptor } from './core/interceptors/loading-bar-interceptor.service';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { provideNativeDateAdapter } from '@angular/material/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
@NgModule({
  declarations: [App],
  imports: [BrowserModule, AppRoutingModule, DialoguePopupComponent, DragDropModule],
  providers: [
    provideNativeDateAdapter(),
    provideBrowserGlobalErrorListeners(),
    provideAnimationsAsync(),
    provideHttpClient(
      withFetch(),
      withInterceptors([
        loadingBarInterceptor,
        errorLoggingInterceptor,
        authInterceptor,
        rateLimitInterceptor,
      ]),
    ),
  ],
  bootstrap: [App],
})
export class AppModule {}
