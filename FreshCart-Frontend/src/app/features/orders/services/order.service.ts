import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.prod';
import { Observable } from 'rxjs';
import { Order } from '../models/order.interface';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private http = inject(HttpClient);
    private apiUrl = `${environment.apiUrl}/api/orders`;
  
    getUserOrders(): Observable<Order[]> {
      return this.http.get<Order[]>(this.apiUrl);
    }

    getOrderDetails(orderId: number): Observable<Order> {
      return this.http.get<Order>(`${this.apiUrl}/${orderId}`);
    }
  
    placeOrder(): Observable<Order[]> {
      return this.http.post<Order[]>(`${this.apiUrl}/checkout`, {});
    }
}
