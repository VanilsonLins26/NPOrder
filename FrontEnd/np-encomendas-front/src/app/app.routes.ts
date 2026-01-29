import { mapToCanActivate, Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { ProductListComponent } from './features/shop/product-list/product-list.component';
import { CartComponent } from './features/shop/cart/cart.component';
import { AutoLoginPartialRoutesGuard } from 'angular-auth-oidc-client';
import { Component } from '@angular/core';
import { OrderDetailsComponent } from './features/shop/order-details/order-details.component';
import { CheckoutComponent } from './features/shop/checkout/checkout.component';
import { MyOrdersComponent } from './features/shop/my-orders/my-orders.component';
import { OrderTrackingComponent } from './features/shop/order-tracking/order-tracking.component';
import { PaymentResultComponent } from './pages/payment-result/payment-result.component';
import { ProductsAdminComponent } from './pages/admin/products-admin/products-admin.component';
import { AdminAuthGuard } from './core/guards/admin.auth.guard';
import { OrdersAdminComponent } from './pages/admin/orders-admin/orders-admin.component';
import { PaymentListComponent } from './pages/admin/payment-list/payment-list.component';
import { DashboardAdminComponent } from './pages/admin/dashboard/dashboard.component';
import { rootRedirectGuard } from './core/guards/root-redirect.guard';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { 
        path: '', 
        pathMatch: 'full',
        canActivate: [rootRedirectGuard], 
        component: ProductListComponent
    },
    { path: 'cardapio', component: ProductListComponent },
    { path: 'carrinho', component: CartComponent,canActivate: [AutoLoginPartialRoutesGuard] },
    { path: 'detalhes-pedido', component : OrderDetailsComponent, canActivate: [AutoLoginPartialRoutesGuard]},
    { path: 'checkout/:id', component : CheckoutComponent, canActivate: [AutoLoginPartialRoutesGuard]},
    { path: 'meus-pedidos', component: MyOrdersComponent, canActivate: [AutoLoginPartialRoutesGuard] },
    { path: 'meus-pedidos/:id', component: OrderTrackingComponent, canActivate: [AutoLoginPartialRoutesGuard] },
    { 
    path: 'payment',
    children: [
        { path: 'success', component: PaymentResultComponent, canActivate: [AutoLoginPartialRoutesGuard] },
        { path: 'failure', component: PaymentResultComponent, canActivate: [AutoLoginPartialRoutesGuard] },
        { path: 'pending', component: PaymentResultComponent, canActivate: [AutoLoginPartialRoutesGuard] }
    ]
    },
    { 
    path: 'admin/produtos', 
    component: ProductsAdminComponent,
    canActivate: [AdminAuthGuard]
  },
  { 
    path: 'admin/encomendas', 
    component: OrdersAdminComponent,
    canActivate: [AdminAuthGuard]
  },
  {
        path: 'admin/financeiro',
        component: PaymentListComponent,
        canActivate: [AdminAuthGuard]
    },
    { 
        path: 'admin/dashboard',
        component: DashboardAdminComponent,
        canActivate: [AdminAuthGuard]
    },
    { path: '**', redirectTo: 'login' }
];
