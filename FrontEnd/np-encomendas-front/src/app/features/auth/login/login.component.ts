import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-login',
  imports: [CommonModule, CardModule, ButtonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  private readonly oidcSecurityService = inject(OidcSecurityService);

  login() {
    // Inicia o redirecionamento para o IdentityServer
    this.oidcSecurityService.authorize();
  }

}
