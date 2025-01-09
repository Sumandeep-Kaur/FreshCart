import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { AuthResponse, LoginRequest, RegisterRequest, User } from '../models/auth.model';
import { environment } from '../../../environments/environment.prod';
import { HttpClient } from '@angular/common/http';
import jwtDecode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();
  private apiUrl = `${environment.apiUrl}/api/auth`;

  constructor(private http: HttpClient) {
    this.loadCurrentUser();
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request)
      .pipe(
        tap(response => {
          localStorage.setItem('token', response.token);
          this.loadCurrentUser();
        })
      );
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, request)
      .pipe(
        tap(response => {
          localStorage.setItem('token', response.token);
          this.loadCurrentUser();
        })
      );
  }

  logout(): void {
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  private loadCurrentUser(): void {
    const token = this.getToken();
    if (token) {
        try {
            const decodedToken: any = jwtDecode(token); 
            if (decodedToken) {
                const user: User = {
                    id: decodedToken.userId,
                    email: decodedToken.email,
                    name: decodedToken.name,
                    role: decodedToken.role
                };
                this.currentUserSubject.next(user);
            }
        } catch (error) {
            console.error('Error decoding token:', error);
            this.logout(); // Clear invalid token
        }
    }
  }
}
