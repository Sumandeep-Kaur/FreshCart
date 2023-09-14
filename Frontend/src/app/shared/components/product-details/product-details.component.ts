import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from 'src/app/core/services/product.service';
import { Product } from '../../interfaces/Product';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CartService } from 'src/app/core/services/cart.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {

  product: Product;
  quantity: number = 1;
  addForm !: FormGroup;
  public stars: boolean[] = Array(5).fill(false);
  public ratings: number[] = Array(5).fill(0);
  totalReviews: number = 0;
  avgRating: number = 0;
  id: number = this.actRoute.snapshot.params['id'];

  constructor(public actRoute: ActivatedRoute, public productService: ProductService,
    private formBuilder: FormBuilder, private cartService: CartService, private toast: ToastrService) { }

  ngOnInit(): void {
    this.productService.getProduct(this.id).subscribe((data: Product) => {
      this.product = data;
      this.totalReviews = this.product.reviews.length;
      let sum = 0;
      for(var i = 0; i < this.totalReviews; i++) {
        sum += this.product.reviews[i].rating;
        this.ratings[this.product.reviews[i].rating - 1]++;
      }
      this.avgRating = sum / this.totalReviews;
    });

    this.addForm = this.formBuilder.group({
      rating: ["", Validators.required],
      title: ["", [Validators.required]],
      description: ["", [Validators.required, Validators.maxLength(255)]],
      productId: [this.id],
      userId: [this.getUserId()],
      userName: [this.getUserName()],
      date: [""]
    });
  }

  isAuthenticated() {
    return localStorage.getItem("loggedIn");
  }

  public noAccess() {
    this.toast.warning("You need to first login to add review.")
  }

  get Rating(): FormControl {
    return this.addForm.get('rating') as FormControl;
  }

  get Title(): FormControl {
    return this.addForm.get('title') as FormControl;
  }

  get Description(): FormControl {
    return this.addForm.get('description') as FormControl;
  }

  getUserId(): string {
    const user = localStorage.getItem("userInfo") ? JSON.parse(localStorage.getItem("userInfo")) : null;
    return user ? user.id : "";
  }

  getUserName(): string {
    const user = localStorage.getItem("userInfo") ? JSON.parse(localStorage.getItem("userInfo")) : null;
    return user ? user.name : "";
  }

  getDate(dat: Date) {
    var date = new Date(dat); 
    var day = date.getDate();
    var year = date.getFullYear();
    var month = date.toLocaleString('default', {month: 'long'});
    return day + " " + month + " " + year;
  }

  generateStars(rating: number = this.avgRating) {
    return Array(rating).fill(0);
  }

  decrement() {
    if(this.quantity > 1) { 
      this.quantity--;
    } 
  }

  increment() {
    if(this.quantity < this.product.unitsInStock) { 
      this.quantity++;
    } else {
      this.toast.warning("Only " +  this.product.unitsInStock + " items are in stock.", "Sorry!");
    }
  }

  public rate(rating: number) {
    this.addForm.patchValue({
      rating: rating
    })
    console.log('rating', rating);
    this.stars = this.stars.map((_, i) => rating > i);
    console.log('stars', this.stars);
  }

  addToCart(product: Product) {
    if(!this.isAuthenticated()) {
      this.toast.info("You need to login to add product to your cart.", "Notice!");
    } else if(this.product.unitsInStock == 0) {
      this.toast.error("This product is currently out of stock.", "Sorry!");
    } else {
      var userId = JSON.parse(localStorage.getItem("userInfo")).id;
      this.cartService.addToCart(product, userId, this.quantity).subscribe({
        next:() => {
          this.toast.success('Success!', 'Product is added to your cart.');
        },
        error:() => {
          console.log("Error ocurred");
        }
      })
    }
  }

  addReview() {
    if(this.addForm.valid) {
      this.productService.addReview(this.addForm.value).subscribe({
        next:(response) => {
          if(response.status === "Success") {
            this.addForm.reset();
            window.location.reload();
            this.toast.success("Review Added Successfully");
          } else {
            this.toast.success("Something Error occurred while adding your review. Pleae try again!");
          }
        },
        error:() => {
          console.log("Error ocurred")
        }
      })
    }
  }

}
