import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { PublicRoutingModule } from './modules/public/public-routing-module';
@NgModule({
  declarations: [App],
  imports: [BrowserModule, AppRoutingModule, PublicRoutingModule],
  providers: [provideBrowserGlobalErrorListeners(), provideHttpClient(withFetch())],
  bootstrap: [App],
})
export class AppModule {}
