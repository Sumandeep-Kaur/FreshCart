import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  isLoading = false;

  loginForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  get email() { return this.loginForm.get('email'); }
  get password() { return this.loginForm.get('password'); }

  async onSubmit() {
    if (this.loginForm.valid) {
      this.isLoading = true;
      try {
        await this.authService.login(this.loginForm.value).toPromise();
        this.authService.currentUser$.subscribe(user => {
          console.log(user?.role);
          if(user?.role === "Admin") {
            this.router.navigate(['/dashboard']);
          } else {
            this.router.navigate(['/']);
          }
        });
      } catch (error: any) {
        // Handle error (you might want to add proper error handling)
        console.error('Login failed:', error);
      } finally {
        this.isLoading = false;
      }
    }
  }
}
