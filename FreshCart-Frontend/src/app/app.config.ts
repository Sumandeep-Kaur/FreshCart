import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { provideToastr } from 'ngx-toastr';
import { provideAnimations } from '@angular/platform-browser/animations';
import { loadingInterceptor } from './core/interceptors/loading.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes), 
    provideHttpClient(), 
    provideHttpClient(withInterceptors([authInterceptor, loadingInterceptor])),
    provideAnimations(),
    provideToastr({
      timeOut: 1000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    }), 
  ]
};
