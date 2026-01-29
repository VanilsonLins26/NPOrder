import { inject } from '@angular/core';
import { Router, UrlTree } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { map, take } from 'rxjs';

export const rootRedirectGuard = () => {
  const oidcSecurityService = inject(OidcSecurityService);
  const router = inject(Router);

  return oidcSecurityService.userData$.pipe(
    take(1),
    map((dados) => {
      let isAdmin = false;

      if (dados) {
        const userData = dados.userData || dados;
        const roles = userData?.role;

        if (roles) {
          isAdmin = Array.isArray(roles) 
            ? roles.includes('Admin') 
            : roles === 'Admin';
        }
      }

      if (isAdmin) {
        return router.createUrlTree(['/admin/dashboard']);
      } else {
        return router.createUrlTree(['/cardapio']);
      }
    })
  );
};