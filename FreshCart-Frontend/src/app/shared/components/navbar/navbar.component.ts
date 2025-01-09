import { Component, inject, OnInit } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { ProductService } from '../../../features/products/services/product.service';
import { CategoryService } from '../../../features/categories/services/category.service';
import { Category } from '../../../features/categories/models/category.interface';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  categories: Category[] = [];
  searchTerm: string = '';
  selectedCategory: Category | undefined;
  private authService = inject(AuthService);
  private categoryService = inject(CategoryService);
  private router = inject(Router);
  currentUser$ = this.authService.currentUser$;

  ngOnInit(): void {
    this.loadCategories();
  }

  logout(): void {
    this.authService.logout();
  }

  onSearch(): void {
    if (this.searchTerm.trim()) {
      this.router.navigate(['/products/search', this.searchTerm]);
    }
  }

  loadCategories() {
    this.categoryService.getAllCategories().subscribe({
      next: (data) => {
        this.categories = data;
        console.log(this.categories);
      },
      error: (error) => {
        console.error('Error loading categories:', error);
      }
    });
  }

  selectCategory(category: Category, event: Event) {
    event.preventDefault(); // Prevent default anchor behavior
    this.selectedCategory = category;
    this.router.navigate(['/products/category', category.id]);
  }
}
