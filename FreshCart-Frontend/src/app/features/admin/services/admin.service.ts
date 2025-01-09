import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.prod';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ProductCreate } from '../models/productCreate.interface';
import { Product } from '../../products/models/product.interface';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = `${environment.apiUrl}/api/products`;

  constructor(private http: HttpClient) {}

  createProduct(product: ProductCreate): Observable<Product> {
    const formData = this.createFormData(product);
    
    return this.http.post<Product>(this.apiUrl, formData)
      .pipe(catchError(this.handleError));
  }

  updateProduct(id: number, product: ProductCreate): Observable<Product> {
    const formData = this.createFormData(product);
    
    return this.http.put<Product>(`${this.apiUrl}/${id}`, formData)
      .pipe(catchError(this.handleError));
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
      .pipe(catchError(this.handleError));
  }

  private createFormData(product: ProductCreate): FormData {
    const formData = new FormData();
    
    formData.append('name', product.name);
    formData.append('description', product.description);
    formData.append('price', product.price.toString());
    formData.append('stock', product.stock.toString());
    formData.append('categoryId', product.categoryId.toString());
    formData.append('image', product.image);
    
    if (product.discountPercentage !== undefined) {
      formData.append('discountPercentage', product.discountPercentage.toString());
    }

    return formData;
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else {
      // Server-side error
      if (error.status === 404) {
        errorMessage = 'Product not found';
      } else if (error.status === 400) {
        errorMessage = 'Invalid input data';
      } else if (error.status === 401) {
        errorMessage = 'Unauthorized - Please log in';
      } else if (error.status === 403) {
        errorMessage = 'Forbidden - You do not have permission to perform this action';
      } else {
        errorMessage = error.error?.message || 'Server error';
      }
    }

    return throwError(() => new Error(errorMessage));
  }
}
