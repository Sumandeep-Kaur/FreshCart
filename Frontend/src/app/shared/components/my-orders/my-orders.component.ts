import { Component, OnInit } from '@angular/core';
import { Order } from '../../interfaces/Order';
import { OrderService } from 'src/app/core/services/order.service';

@Component({
  selector: 'app-my-orders',
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.css']
})
export class MyOrdersComponent implements OnInit {

  public orderItems: Order[] = [];
  public userId: string;

  constructor(private orderService: OrderService) { }

  ngOnInit(): void {
    this.userId = this.getUserId();
    this.orderService.getUserOrders(this.userId).subscribe((data: Order[]) => {
      this.orderItems = data;
    });
  }

  getUserId(): string {
    const user = JSON.parse(localStorage.getItem("userInfo"));
    return user.id;
  }

  getDate(dat: Date) {
    var date = new Date(dat); 
    var day = date.getDate();
    var year = date.getFullYear();
    var month = date.toLocaleString('default', {month: 'long'});
    return day + " " + month + " " + year;
  }
}
