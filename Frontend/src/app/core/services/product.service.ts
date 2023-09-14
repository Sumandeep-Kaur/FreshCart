import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, throwError } from 'rxjs';
import { Product } from 'src/app/shared/interfaces/Product';
import { Review } from 'src/app/shared/interfaces/Review';


@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private apiURL = 'API_URL';

  constructor(private httpClient: HttpClient) { }

  getAllProducts(): Observable<Product[]> {
    return this.httpClient
      .get<Product[]>(this.apiURL + '/allProducts')
      .pipe(catchError(this.errorHandler));
  }

  getProduct(id: number): Observable<Product> {
    return this.httpClient
      .get<Product>(this.apiURL + '/Product/id?id=' + id)
      .pipe(catchError(this.errorHandler));
  }

  addProduct(product: Product): Observable<any> {
    return this.httpClient.post<Product>(this.apiURL + '/add/', product)
      .pipe(catchError(this.errorHandler));
  }

  updateProduct(id: number, product: Product): Observable<any> {
    return this.httpClient.put<Product>(this.apiURL + '/update/id?id=' + id, product)
      .pipe(catchError(this.errorHandler));
  }

  deleteProduct(id: number): Observable<any> {
    return this.httpClient
      .delete<Product>(this.apiURL + '/delete/id?id=' + id)
      .pipe(catchError(this.errorHandler));
  }

  searchProducts(name: string, isCategory: boolean): Observable<Product[]> {
    return this.httpClient
      .get<Product[]>(this.apiURL + '/get/isCategory/name?name=' + name + '&isCategory=' + isCategory)
      .pipe(catchError(this.errorHandler));
  }

  addReview(review: Review): Observable<any> {
    return this.httpClient.post<Product>(this.apiURL + '/addReview/', review)
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
