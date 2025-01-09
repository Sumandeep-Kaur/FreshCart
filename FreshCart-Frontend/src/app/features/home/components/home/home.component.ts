import { Component, inject, Inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ProductListComponent } from "../../../products/components/product-list/product-list.component";
import { AuthService } from '../../../../core/services/auth.service';
import { CategoryListComponent } from "../../../categories/components/category-list/category-list.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink, ProductListComponent, CategoryListComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  private authService = inject(AuthService);

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      console.log(user);
    });
  }
  
}
