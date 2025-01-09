import { Component, inject, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { Router, RouterLink } from '@angular/router';
import { Cart, CartItem } from '../../models/cart.interface';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Product } from '../../../products/models/product.interface';
import { OrderService } from '../../../orders/services/order.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {
  private cartService = inject(CartService);
  private orderService = inject(OrderService);
  cart: Cart | null = null;
  orderPlaced = false;

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.cartService.getCart().subscribe(cart => {
      this.cart = cart;
      console.log(cart);
    });
  }

  calculateDiscountedPrice(product: Product): number {
      if (!product.discountPercentage) return product.price;
      const discountedPrice = product.price - (product.price * (product.discountPercentage / 100));
      return Math.round(discountedPrice * 100) / 100;
  }

  increment(item: CartItem): void {
    this.cartService.updateCartItem(item.id, item.quantity + 1)
      .subscribe(updatedCart => {
        this.cart = updatedCart;
      });
  }

  decrement(item: CartItem): void {
    if (item.quantity > 1) {
      this.cartService.updateCartItem(item.id, item.quantity - 1)
        .subscribe(updatedCart => {
          this.cart = updatedCart;
        });
    }
  }

  deleteItem(itemId: number): void {
    this.cartService.removeFromCart(itemId)
      .subscribe(() => {
        this.loadCart();
      });
  }

  getTotalPrice(): number {
    if (!this.cart) return 0;
    return this.cart.items
      .reduce((total, item) => total + (item.product.price * item.quantity), 0);
  }

  getTotalDiscount(): number {
    if (!this.cart) return 0;
    const totalDiscount = this.cart.items
      .reduce((total, item) => {
        const price = item.product.price || 0;
        const discountPercentage = item.product.discountPercentage || 0;
        const discountAmount = (price * discountPercentage / 100) * item.quantity;
        return total + discountAmount;
      }, 0);
    
    return Math.round(totalDiscount * 100) / 100;
  }

  makeOrder(): void {
    this.orderService.placeOrder().subscribe({
      next: () => {
        this.orderPlaced = true;
      },
      error: (error) => console.error('Checkout failed:', error)
    });
  }
}
