import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AppComponent } from './app.component';
import { HomeComponent } from './shared/components/home/home.component';
import { ProductListComponent } from './shared/components/product-list/product-list.component';
import { HeaderComponent } from './shared/components/header/header.component';
import { FooterComponent } from './shared/components/footer/footer.component';
import { ProductDetailsComponent } from './shared/components/product-details/product-details.component';
import { LoginComponent } from './shared/components/login/login.component';
import { SignupComponent } from './shared/components/signup/signup.component';
import { ShoppingCartComponent } from './shared/components/shopping-cart/shopping-cart.component';
import { MyOrdersComponent } from './shared/components/my-orders/my-orders.component';
import { SearchResultComponent } from './shared/components/search-result/search-result.component';
import { AdminDashboardComponent } from './shared/components/admin-dashboard/admin-dashboard.component';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProductService } from './core/services/product.service';
import { CategoryService } from './core/services/category.service';
import { UserService } from './core/services/user.service';
import { CartService } from './core/services/cart.service';
import { OrderService } from './core/services/order.service';
import { SpinnerComponent } from './shared/components/spinner/spinner.component';
import { LoaderService } from './core/services/loader.service';
import { LoadingInterceptor } from './core/interceptors/loading.interceptor';
import { authGuard } from './core/auth/auth.guard';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ProductListComponent,
    HeaderComponent,
    FooterComponent,
    ProductDetailsComponent,
    LoginComponent,
    SignupComponent,
    ShoppingCartComponent,
    MyOrdersComponent,
    SearchResultComponent,
    AdminDashboardComponent,
    SpinnerComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-bottom-right'
    })
  ],
  providers: [ProductService, CategoryService, UserService, CartService, OrderService, LoaderService,
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule {  }
