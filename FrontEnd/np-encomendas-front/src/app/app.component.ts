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

    this.oidcSecurityService.checkAuth().subscribe(({ isAuthenticated, userData }) => {
        console.log('App CheckAuth:', isAuthenticated, userData);
        this.isLoading = false; 
    });

    
  }
  

}
