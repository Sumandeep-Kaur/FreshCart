import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/core/services/product.service';
import { Product } from '../../interfaces/Product';
import { CartService } from 'src/app/core/services/cart.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {

  public products: Product[] = [];

  constructor(private productService: ProductService, private cartService: CartService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.productService.getAllProducts().subscribe((data: Product[]) => {
      this.products = data;
    });
  }

  isAuthenticated(){
    return localStorage.getItem("loggedIn");
  }

  addToCart(product: Product) {
    if(!this.isAuthenticated()) {
      this.toastr.warning("You need to first login to add product to your cart.");
    } else if(product.unitsInStock == 0) {
      this.toastr.error("This product is currently out of stock.", "Sorry!");
    } else {
      var userId = JSON.parse(localStorage.getItem("userInfo")).id;
      this.cartService.addToCart(product, userId).subscribe({
        next:() => {
          this.toastr.success('Success!', 'Product is added to your cart.');
        },
        error:() => {
          console.log("Error ocurred");
        }
      })
    }
  }
}
