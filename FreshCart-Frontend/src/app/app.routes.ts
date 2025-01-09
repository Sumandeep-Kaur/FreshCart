import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule)
    },
    {
        path: 'products',
        loadChildren: () => import('./features/products/products.module').then(m => m.ProductsModule)
    },
    {
        path: 'cart',
        canActivate: [authGuard],
        loadChildren: () => import('./features/cart/cart.module').then(m => m.CartModule)
    },
    {
        path: 'orders',
        canActivate: [authGuard],
        loadChildren: () => import('./features/orders/orders.module').then(m => m.OrdersModule)
    },
    {
        path: 'auth',
        loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule)
    },
    {
        path: 'dashboard',
        canActivate: [authGuard],
        loadChildren: () => import('./features/admin/admin.module').then(m => m.AdminModule)
    },
    {
        path: '**',
        redirectTo: ''
    }
];
