import { Component, inject } from '@angular/core';
import { OrderService } from '../../services/order.service';
import { Order, OrderItem } from '../../models/order.interface';
import { catchError, of } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-list.component.html',
  styleUrl: './order-list.component.css'
})
export class OrderListComponent {
  orders: Order[] = [];
  error: string | null = null;
  private orderService = inject(OrderService);

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.orderService.getUserOrders()
      .pipe(
        catchError(error => {
          this.error = 'Failed to load orders. Please try again later.';
          return of([]);
        })
      )
      .subscribe(orders => {
        this.orders = orders;
      });
  }

  getOrderStatus(status: string): { text: string; class: string } {
    const statusMap: { [key: string]: { text: string; class: string } } = {
      'PENDING': { text: 'Pending', class: 'bg-warning' },
      'PROCESSING': { text: 'Processing', class: 'bg-info' },
      'COMPLETED': { text: 'Completed', class: 'bg-success' },
      'CANCELLED': { text: 'Cancelled', class: 'bg-danger' }
    };
    return statusMap[status] || { text: status, class: 'bg-secondary' };
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  calculateTotalItems(items: OrderItem[]): number {
    return items.reduce((sum, item) => sum + item.quantity, 0);
  }
}
