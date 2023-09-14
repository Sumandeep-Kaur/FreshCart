import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Category } from '../../interfaces/Category';
import { CategoryService } from 'src/app/core/services/category.service';
import { UserService } from 'src/app/core/services/user.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Product } from '../../interfaces/Product';
import { ProductService } from 'src/app/core/services/product.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  searchInput: string = '';
  category: string = '';
  public products: Product[] = [];
  public categories: Category[] = [];

  constructor(private categoryService: CategoryService, private userService: UserService, private productService: ProductService, 
    private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.categoryService.getAllCategories().subscribe((data: Category[]) => {
      this.categories = data;
    });
  }

  isAuthenticated() {
    return localStorage.getItem("loggedIn");
  }

  UserName(): string {
    const user = localStorage.getItem("userInfo") ? JSON.parse(localStorage.getItem("userInfo")) : null;
    return user ? user.name : "User";
  }

  isAdmin(): boolean {
    const user = localStorage.getItem("userInfo") ? JSON.parse(localStorage.getItem("userInfo")) : null;
    return user ? user.isAdmin : false;
  }

  getUserId(): string {
    const user = localStorage.getItem("userInfo") ? JSON.parse(localStorage.getItem("userInfo")) : null;
    return user ? user.id : "";
  }

  search() {
    this.router.navigate(['/search', false, this.searchInput]);
  }

  categorySearch(searchCategory) {
    this.category = searchCategory;
    this.router.navigate(['/search', true, searchCategory]);
  }

  logout() {
    localStorage.removeItem("loggedIn");
    localStorage.removeItem("userInfo");
    this.toastr.success("Logged Out");
    this.router.navigate(['/home']);
  }
}
