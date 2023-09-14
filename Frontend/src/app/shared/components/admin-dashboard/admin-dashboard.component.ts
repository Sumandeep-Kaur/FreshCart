import { Component, OnInit } from '@angular/core';
import { Product } from '../../interfaces/Product';
import { ProductService } from 'src/app/core/services/product.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Category } from '../../interfaces/Category';
import { CategoryService } from 'src/app/core/services/category.service';
import { OrderService } from 'src/app/core/services/order.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
  
  public allProducts: Product[] = [];
  public topFiveProducts: Product[] = [];
  public categories: Category[] = [];
  public months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
  id: number;
  month: number = new Date().getMonth() + 1;
  addMode: boolean = true;
  addForm !: FormGroup;

  constructor(private productService: ProductService, private categoryService: CategoryService,
    private orderService: OrderService, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.productService.getAllProducts().subscribe((data: Product[]) => {
      this.allProducts = data;
    });

    this.orderService.getTopFiveOrders(this.month).subscribe((data: Product[]) => {
      this.topFiveProducts = data;
    });

    this.categoryService.getAllCategories().subscribe((data: Category[]) => {
      this.categories = data;
    });

    this.addForm = this.formBuilder.group({
      name: ["", [Validators.required, Validators.maxLength(100), Validators.pattern('^[a-zA-Z0-9 \'\-]+$')]],
      categoryId: ["", [Validators.required]],
      description: ["", [Validators.required, Validators.maxLength(255)]],
      price: ["", [Validators.required]],
      unitsInStock: ["", [Validators.required]],
      imageUrl: ["", [Validators.required, Validators.pattern("[^\\s]+(.*?)\\.(jpg|png|JPG|PNG)$")]],
      discount: ["0"],
      specs: ["", [Validators.maxLength(100)]]
    });
  }

  get Name(): FormControl {
    return this.addForm.get('name') as FormControl;
  }

  get Category(): FormControl {
    return this.addForm.get('categoryId') as FormControl;
  }

  get Description(): FormControl {
    return this.addForm.get('description') as FormControl;
  }

  get ImageUrl(): FormControl {
    return this.addForm.get('imageUrl') as FormControl;
  }

  get Price(): FormControl {
    return this.addForm.get('price') as FormControl;
  }

  get UnitsInStock(): FormControl {
    return this.addForm.get('unitsInStock') as FormControl;
  }

  get Specs(): FormControl {
    return this.addForm.get('specs') as FormControl;
  }

  openEditModal(product: Product) {
    this.addMode = false;
    this.id = product.id;
    this.addForm.patchValue({
      name: product.name,
      description: product.description,
      categoryId: product.category.id,
      price: product.price,
      discount: product.discount,
      unitsInStock: product.unitsInStock,
      imageUrl: product.imageUrl,
      specs: product.specs
     });
  }

  addProduct() {
    if(this.addForm.valid) {
      this.productService.addProduct(this.addForm.value).subscribe({
        next:(response) => {
          if(response.status === "Success") {
            this.addForm.reset();
            window.location.reload();
          } else {
            console.log("Failuer");
          }
        },
        error:() => {
          console.log("Error ocurred")
        }
      })
    }
  }

  updateProduct() {
    if(this.addForm.valid) {
      this.productService.updateProduct(this.id, this.addForm.value).subscribe({
        next:(response) => {
          if(response.status === "Success") {
            this.addForm.reset();
            window.location.reload();
            console.log("Success");
          } else {
            console.log("Failuer");
          }
        },
        error:() => {
          console.log("Some Error ocurred")
        }
      })
    }
  }
  
  deleteProduct(id: number) {
    this.productService.deleteProduct(id).subscribe({
      next:(response) => {
        if(response.status === "Success") {
          window.location.reload;
          window.location.reload;
          console.log("Success");
        } else {
          console.log("Failure");
        }
      },
      error: () => {
        console.log("Some Error Occurred");
      }
    });
  }

  getTopFiveProducts(searchMonth) {
    this.month = new Date(`${searchMonth} 1, 2022`).getMonth() + 1;
    console.log(this.month);
    this.orderService.getTopFiveOrders(this.month).subscribe((data: Product[]) => {
      this.topFiveProducts = data;
    });
  }
}
