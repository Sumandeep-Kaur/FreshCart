import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Cart } from 'src/app/shared/interfaces/Cart';
import { Product } from 'src/app/shared/interfaces/Product';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private apiURL = 'API_URL';

  constructor(private httpClient: HttpClient) { }

  addToCart(product: Product, userId: string, quantity: number = 1): Observable<any> {
    return this.httpClient.post<any>(this.apiURL + '/add/id?userId=' + userId + '&quantity=' + quantity, product)
    .pipe(catchError(this.errorHandler));
  }

  updateCart(items: Cart[]): Observable<any> {
    return this.httpClient.request<any>(
      'POST',
      this.apiURL + '/update',
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        }),
        body: JSON.stringify(items)
      })
      .pipe(catchError(this.errorHandler));
  }

  getUserCart(id: string): Observable<Cart[]> {
    return this.httpClient
      .get<Cart[]>(this.apiURL + '/getCart/id?userId=' + id)
      .pipe(catchError(this.errorHandler));
  }

  deleteCartItem(id: number): Observable<any> {
    return this.httpClient
      .delete<Cart>(this.apiURL + '/delete/id?id=' + id)
      .pipe(catchError(this.errorHandler));
  }

  errorHandler(error: {
    error: { message: string };
    status: any;
    message: any;
  }) {
    console.log(error);
    return throwError(
      () => new Error('Something bad happened; please try again later.')
    );
  }
}
