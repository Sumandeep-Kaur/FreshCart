import { Component, OnInit } from '@angular/core';
import { Cart } from '../../interfaces/Cart';
import { CartService } from 'src/app/core/services/cart.service';
import { ToastrService } from 'ngx-toastr';
import { OrderService } from 'src/app/core/services/order.service';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent implements OnInit {

  public cartItems: Cart[] = [];
  public userId: string;
  public orders: Cart[] = [];
  public orderPlaced: boolean = false;

  constructor(private cartService: CartService, private orderService: OrderService,
    private toastr: ToastrService) { }

  ngOnInit(): void { 
    this.userId = this.getUserId();
    this.cartService.getUserCart(this.userId).subscribe((data: Cart[]) => {
      this.cartItems = data;
      for(var i of this.cartItems) {
        this.orders.push(i);
      }
    });
  }

  getUserId(): string {
    const user = JSON.parse(localStorage.getItem("userInfo"));
    return user.id;
  }

  decrement(cartItem: Cart) {
    if (cartItem.quantity > 1) {
      cartItem.quantity--;
      var index = this.orders.findIndex(o => o.id == cartItem.id);
      if(index != -1) { 
        this.orders[index].quantity = cartItem.quantity;
      }
    }
  }

  increment(cartItem: Cart) {
    if(cartItem.product.unitsInStock < cartItem.quantity) {
      this.toastr.error("Only " + cartItem.quantity + " items are in stock", "Sorry!");
    } else { 
      cartItem.quantity++;
      var index = this.orders.findIndex(o => o.id == cartItem.id);
      if(index != -1) { 
        this.orders[index].quantity = cartItem.quantity;
      }
    }
  }

  getTotalPrice() {
    let sum: number = 0;
    this.orders.forEach(c => sum += (c.product.price * c.quantity));
    return sum;
  }

  getTotalDiscount() {
    let sum: number = 0;
    this.orders.forEach(c => sum += (c.product.discount * c.quantity));
    return sum;
  }

  updateOrderItems(item) {
    var index = this.orders.findIndex(o => o.id == item.value);
    if(item.checked) {
      if(index === -1) {
        var i = this.cartItems.findIndex(o => o.id == item.value);
        this.orders.push(this.cartItems[i]);
      }
    } else {
      this.orders.splice(index,1);
    }
  }

  updateCart() {
    this.cartService.updateCart(this.cartItems).subscribe({
      next: () => {
        this.toastr.success("Cart is updated.", "Success!");
      },
      error: () => {
        console.log("Error ocurred");
      }
    })
  }

  makeOrder() {
    var userId = JSON.parse(localStorage.getItem("userInfo")).id;
    this.orderService.addOrder(this.orders, userId).subscribe({
      next: () => {
        this.orderPlaced = true;
        this.toastr.success("Your order is received.", "Success!");
      },
      error: () => {
        console.log("Error ocurred");
      }
    })
  }

  deleteItem(id: number) {
    this.cartService.deleteCartItem(id).subscribe({
      next:(response) => {
        if(response.status === "Success") {
          window.location.reload;
          window.location.reload;
          this.toastr.success("Item removed from cart.", "Success")
        } else {
          this.toastr.error("Some error occurred while removing item. Please try again.", "Error!");
        }
      },
      error: () => {
        console.log("Some Error Occurred");
      }
    });
  }

}
