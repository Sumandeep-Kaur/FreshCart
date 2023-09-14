import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Cart } from 'src/app/shared/interfaces/Cart';
import { Order } from 'src/app/shared/interfaces/Order';
import { Product } from 'src/app/shared/interfaces/Product';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private apiURL = 'API_URL';

  constructor(private httpClient: HttpClient) { }

  addOrder(items: Cart[], userId: string): Observable<any> {
    return this.httpClient.request<any>(
      'POST',
      this.apiURL + '/add/id?userId=' + userId,
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        }),
        body: JSON.stringify(items)
      })
      .pipe(catchError(this.errorHandler));
  }

  getUserOrders(id: string): Observable<Order[]> {
    return this.httpClient
      .get<Order[]>(this.apiURL + '/getOrders/id?userId=' + id)
      .pipe(catchError(this.errorHandler));
  }

  getTopFiveOrders(month: number): Observable<Product[]> {
    return this.httpClient
      .get<Product[]>(this.apiURL + '/getTopFiveOrders/month?month=' + month)
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
