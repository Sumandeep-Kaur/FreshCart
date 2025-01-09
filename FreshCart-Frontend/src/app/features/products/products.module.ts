import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductDetailsComponent } from './components/product-details/product-details.component';

const routes: Routes = [
  { path: '', component: ProductListComponent},
  { path: 'category/:categoryId', component: ProductListComponent },  
  { path: 'search/:searchTerm', component: ProductListComponent },  
  { path: ':id', component: ProductDetailsComponent }  
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class ProductsModule { }
