import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.prod';
import { Cart } from '../models/cart.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/cart`;

  getCart(): Observable<Cart> {
    return this.http.get<Cart>(this.apiUrl);
  }

  addToCart(productId: number, quantity: number): Observable<Cart> {
    const params = new HttpParams()
      .set('productId', productId)
      .set('quantity', quantity);

    return this.http.post<Cart>(`${this.apiUrl}/items`, null, { params });
  }

  updateCartItem(cartItemId: number, quantity: number): Observable<Cart> {
    const params = new HttpParams().set('quantity', quantity);
    return this.http.put<Cart>(`${this.apiUrl}/items/${cartItemId}`, null, { params });
  }

  removeFromCart(cartItemId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/items/${cartItemId}`);
  }

  clearCart(): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/clear`);
  }
}
