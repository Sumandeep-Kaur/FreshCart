import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LoaderService } from '../services/loader.service';
import { catchError, finalize, of, switchMap, timer } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loaderService = inject(LoaderService);
  loaderService.setLoading(true);

    return next(req).pipe(
      switchMap((response) => {
        return timer(1000).pipe(
          switchMap(() => of(response))
        );
      }),
      catchError((error) => {
        return of(error);
      }),
      finalize(() => {
        loaderService.setLoading(false); 
      })
    );
};
