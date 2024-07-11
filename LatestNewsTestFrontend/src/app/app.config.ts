import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { MatCard } from '@angular/material/card';
import { MatGridList } from '@angular/material/grid-list';
import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';


export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), provideHttpClient(), provideAnimationsAsync()]
};
