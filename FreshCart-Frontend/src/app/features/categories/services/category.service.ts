import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.prod';
import { HttpClient } from '@angular/common/http';
import { Category } from '../models/category.interface';
import { Observable } from 'rxjs';
import { Product } from '../../products/models/product.interface';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private http = inject(HttpClient);
    private apiUrl = `${environment.apiUrl}/api/category`;
  
    getAllCategories(): Observable<Category[]> {
      return this.http.get<Category[]>(this.apiUrl);
    }
  
    getProductsByCategoryId(id: number): Observable<Product[]> {
      return this.http.get<Product[]>(`${this.apiUrl}/${id}/products`);
    }
}
