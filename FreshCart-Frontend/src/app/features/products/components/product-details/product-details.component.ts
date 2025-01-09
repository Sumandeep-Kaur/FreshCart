import { CommonModule } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from '../../models/product.interface';
import { Review } from '../../models/review.interface';
import { CartService } from '../../../cart/services/cart.service';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.css'
})
export class ProductDetailsComponent implements OnInit {
  product !: Product;
  private productService = inject(ProductService);
  private cartService = inject(CartService);
  private route = inject(ActivatedRoute);
  protected router = inject(Router);
  private fb = inject(FormBuilder);

  reviewForm !: FormGroup;
  quantity = 1;
  ratingStars = signal([false, false, false, false, false]);


  ngOnInit() {
    this.reviewForm = this.fb.group({
      comment: ['', [Validators.required, Validators.maxLength(255)]]
    });

    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.productService.getProductById(id).subscribe((data: Product) => {
      this.product = data;
    });
  }

  totalReviews = computed(() => this.product.reviews.length);

  ratingDistribution = computed(() => {
    const distribution = [0, 0, 0, 0, 0];
    this.product.reviews.forEach(review => {
      distribution[5 - review.rating]++;
    });
    return distribution;
  });

  calculateDiscountedPrice() {
    if (!this.product.discountPercentage) return this.product.price;
    const discountedPrice = this.product.price - (this.product.price * (this.product.discountPercentage / 100));
    return Math.round(discountedPrice * 100) / 100;
  }

  generateStars(count: number) {
    return Array(Math.round(count)).fill(0);
  }

  incrementQuantity() {
    if (this.quantity < this.product.stock) {
      this.quantity++;
    }
  }

  decrementQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  setRating(rating: number) {
    this.ratingStars.update(() => 
      Array(5).fill(false).map((_, index) => index < rating));
  }

  isAuthenticated() {
    // Implement authentication check
    return true;
  }

  promptLogin() {
    // Implement login prompt
  }

  addToCart() {
    if (!this.product.stock) {
      // Handle out of stock
      return;
    }

    this.cartService.addToCart(this.product.id, this.quantity).subscribe({
      next: () => {
        // Show success message or update cart count
      },
      error: (error) => {
        // Handle error
        console.error('Error adding to cart:', error);
      }
    });
  }

  submitReview() {
    if (this.reviewForm.valid) {
      const rating = this.ratingStars().filter(star => star).length;
      const review: Partial<Review> = {
        rating,
        comment: this.reviewForm.get('comment')?.value,
        reviewDate: new Date()
      };
      
      // Submit review to service
      console.log('Submitting review:', review);
    }
  }
}
