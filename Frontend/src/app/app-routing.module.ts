import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './shared/components/home/home.component';
import { ProductDetailsComponent } from './shared/components/product-details/product-details.component';
import { LoginComponent } from './shared/components/login/login.component';
import { SignupComponent } from './shared/components/signup/signup.component';
import { ShoppingCartComponent } from './shared/components/shopping-cart/shopping-cart.component';
import { MyOrdersComponent } from './shared/components/my-orders/my-orders.component';
import { SearchResultComponent } from './shared/components/search-result/search-result.component';
import { AdminDashboardComponent } from './shared/components/admin-dashboard/admin-dashboard.component';
import { authGuard } from './core/auth/auth.guard';

const routes: Routes = [
  {path: '', redirectTo: "/home", pathMatch:"full"},
  {path: 'home', component: HomeComponent},
  {path: 'product-details/:id', component: ProductDetailsComponent},
  {path: 'login', component: LoginComponent},
  {path: 'signup', component: SignupComponent},
  {path: 'my-cart/:id', component: ShoppingCartComponent},
  {path: 'my-orders', component: MyOrdersComponent},
  {path: 'search/:isCategory/:product', component: SearchResultComponent},
  {path: 'dashboard', component: AdminDashboardComponent},
  {path: '**', redirectTo: "/home", pathMatch: "full"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
