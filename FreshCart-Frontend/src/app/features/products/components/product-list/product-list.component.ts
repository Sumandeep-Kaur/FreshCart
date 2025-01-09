import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { CartService } from '../../../cart/services/cart.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Product } from '../../models/product.interface';
import { CategoryService } from '../../../categories/services/category.service';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit {
  private categoryService = inject(CategoryService);
  private productService = inject(ProductService);
  private cartService = inject(CartService);
  private toastr = inject(ToastrService);

  products: Product[] = [];
  loading = true;
  error: string | null = null;

  constructor(
    public actRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.actRoute.params.subscribe(params => {
      const categoryId = params['categoryId'];
      const searchTerm = params['searchTerm'];

      this.loading = true;

      if (categoryId) {
        // Get products by category
        this.loadProductsByCategory(categoryId);
      } else if (searchTerm) {
        // Search products by keyword
        this.loadSearchProducts(searchTerm);
      } else {
        // Get all products
        this.loadAllProducts();
      }
    });
  }

  loadAllProducts(): void {
    this.productService.getAllProducts().subscribe({
      next: (data) => {
        this.products = data;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load products. Please try again later.';
        this.loading = false;
        console.error('Error loading products:', error);
      }
    });
  }

  loadProductsByCategory(id: number): void {
    this.categoryService.getProductsByCategoryId(id).subscribe({
      next: (data) => {
        this.products = data;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load products. Please try again later.';
        this.loading = false;
        console.error('Error loading products:', error);
      }
    });
  }

  loadSearchProducts(keyword: string): void {
    this.productService.searchProducts(keyword).subscribe({
      next: (data) => {
        this.products = data;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load products. Please try again later.';
        this.loading = false;
        console.error('Error loading products:', error);
      }
    });
  }

  calculateDiscountedPrice(product: Product): number {
    if (!product.discountPercentage) return product.price;
    const discountedPrice = product.price - (product.price * (product.discountPercentage / 100));
    return Math.round(discountedPrice * 100) / 100;
  }

  addToCart(product: Product): void {
    this.cartService.addToCart(product.id, 1).subscribe({
      next: () => {
        this.toastr.success('Item added to cart.', 'Success!');
      },
      error: (error) => {
        // Handle error
        this.toastr.error('Some error occurred while adding item to cart.', 'Error');
        console.error('Error adding to cart:', error);
      }
    });
  }

  getStars(rating: number): string {
    let fullStars = Math.floor(rating);
    let halfStar = rating % 1 >= 0.5 ? 1 : 0;
    let emptyStars = 5 - fullStars - halfStar;
  
    let starsHTML = '';
  
    for (let i = 0; i < fullStars; i++) {
      starsHTML += '<i class="fas fa-star"></i>';
    }
  
    if (halfStar) {
      starsHTML += '<i class="fas fa-star-half-alt"></i>';
    }
  
    for (let i = 0; i < emptyStars; i++) {
      starsHTML += '<i class="far fa-star"></i>';
    }
  
    return starsHTML;
  }
}
