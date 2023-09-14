import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ProductService } from 'src/app/core/services/product.service';
import { Product } from '../../interfaces/Product';
import { ActivatedRoute } from '@angular/router';
import { CartService } from 'src/app/core/services/cart.service';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchResultComponent implements OnInit {

  public products: Product[] = [];
  product: string = this.actRoute.snapshot.params['product'];
  isCategory: boolean = this.actRoute.snapshot.params['isCategory'];

  constructor(private productService: ProductService, private cartService: CartService,
    private actRoute: ActivatedRoute, private toastr: ToastrService) { this.actRoute.params.subscribe(params => console.log(params)); }

  ngOnInit(): void {
    this.productService.searchProducts(this.product, this.isCategory).subscribe((data: Product[]) => {
      this.products = data;
    });

  }

  isAuthenticated() {
    return localStorage.getItem("loggedin");
  }

  addToCart(product: Product) {
    if (!this.isAuthenticated()) {
      this.toastr.warning("You need to first login to add product to your cart.", "Error!");
    } else if (product.unitsInStock == 0) {
      this.toastr.error("This product is currently out of stock.", "Sorry!");
    } else {
      var userId = JSON.parse(localStorage.getItem("userInfo")).id;
      this.cartService.addToCart(product, userId).subscribe({
        next: () => {
          this.toastr.success('Success!', 'Product is added to your cart.');
        },
        error: () => {
          console.log("Error ocurred");
        }
      })
    }
  }

  sortProducts() {
    this.products.sort((a, b) => a.price - b.price);
  }
}
