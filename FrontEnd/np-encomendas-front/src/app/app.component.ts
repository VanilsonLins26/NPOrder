import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { NavbarComponent } from "./layout/navbar/navbar.component";
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { CommonModule } from '@angular/common';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-root',
  imports: [NavbarComponent,CommonModule,RouterOutlet, ProgressSpinnerModule, ToastModule],
  templateUrl: './app.component.html',
  providers: [MessageService],
  styles: [`
    .loading-container {
      height: 100vh;
      display: flex;
      justify-content: center;
      align-items: center;
      flex-direction: column;
      background-color: #f8f9fa;
    }
  `]
})


export class AppComponent implements OnInit {
  private router = inject(Router);
  private readonly oidcSecurityService = inject(OidcSecurityService);

  isLoading = true;

  ngOnInit() {

    this.oidcSecurityService.checkAuth().subscribe(({ isAuthenticated, userData, accessToken }) => {

      console.log('App iniciou. Está logado?', isAuthenticated);

      if (isAuthenticated) {
        console.log('Token salvo com sucesso:', accessToken);
        console.log('Dados do usuário:', userData);
        this.navigateBasedOnRole(userData);
      } else {
        console.log('Usuário não está logado.');
        this.isLoading = false;
      }
    });

    
  }

  

  private navigateBasedOnRole(userData: any) {
    const roles = userData.role || userData['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    let isAdmin = false;

    if (Array.isArray(roles)) {
      isAdmin = roles.includes('Admin');
    } else {
      isAdmin = roles === 'Admin';
    }

    const currentUrl = this.router.url;

    if (isAdmin && (currentUrl === '/' || currentUrl.includes('code='))) {
      this.router.navigate(['/admin/products']).then(() => {
        this.isLoading = false;
      });
    } else {
      this.isLoading = false;
    }
  }
}
