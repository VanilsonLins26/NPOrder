import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { ToolbarModule } from 'primeng/toolbar';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Router } from '@angular/router';
import { DrawerModule } from 'primeng/drawer';
import { CartService } from '../../core/services/cart.service';
import { BadgeModule } from 'primeng/badge';

@Component({
  selector: 'app-navbar',
  imports: [CommonModule, ButtonModule, ToolbarModule, DrawerModule, BadgeModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent implements OnInit {
  private readonly oidcSecurityService = inject(OidcSecurityService);
  private readonly router = inject(Router);
  private cartService = inject(CartService);

  isAuthenticated = false;
  userData: any;
  isAdmin = false;
  sidebarVisible: boolean = false;
  cartCount: number = 0;

  ngOnInit() {

    this.cartService.cartCount$.subscribe(count => {
      this.cartCount = count;
    });

    this.oidcSecurityService.isAuthenticated$.subscribe(({ isAuthenticated }) => {
      this.isAuthenticated = isAuthenticated;


      
    });


    this.oidcSecurityService.userData$.subscribe((dados) => {

      if (dados) {
        this.userData = dados.userData || dados;
        const roles = this.userData?.role;
        console.log('Navbar recebeu:', this.userData);

        if (roles) {
          this.isAdmin = Array.isArray(roles)
            ? roles.includes('Admin')
            : roles === 'Admin';
        } else {
          this.isAdmin = false;
        }
      }


    });

    this.cartService.cartCount$.subscribe(count => {
      this.cartCount = count;
    });

    if (this.isAuthenticated && !this.isAdmin) {
            this.cartService.getCart().subscribe({
                error: (err) => console.error('Admin ou erro: Carrinho ignorado', err)
            });
        }
  }



  login() {
    this.oidcSecurityService.authorize();
  }

  logout() {
    this.oidcSecurityService.logoff().subscribe();
    this.isAdmin = false;
    this.isAuthenticated = false;
    this.sidebarVisible = false;
    this.router.navigate(['/']);
  }

  goToCardapio() {
    this.sidebarVisible = false;
    this.router.navigate(['/cardapio']);

  }

  goToCarrinho() {
    this.sidebarVisible = false;
    this.router.navigate(['/carrinho']);
  }

  goToEncomendas() {
    this.sidebarVisible = false;
    this.router.navigate(['/meus-pedidos']);
  }

  goToAdminProducts() {
    this.sidebarVisible = false;
    this.router.navigate(['/admin/produtos']);
  }

  goToAdminOrders() {
    this.sidebarVisible = false;
    this.router.navigate(['/admin/encomendas']);
  }
  goToAdminPayments() {
    this.sidebarVisible = false;
    this.router.navigate(['/admin/financeiro']);
  }
  goToAdminDashboard() {
    this.sidebarVisible = false;
    this.router.navigate(['/admin/dashboard']);
  }
}