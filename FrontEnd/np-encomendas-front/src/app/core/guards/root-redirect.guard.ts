import { inject } from '@angular/core';
import { Router, UrlTree } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { map, take } from 'rxjs';

export const rootRedirectGuard = () => {
  const oidcSecurityService = inject(OidcSecurityService);
  const router = inject(Router);

  return oidcSecurityService.checkAuth().pipe(
    take(1),
    map(({ isAuthenticated, userData }) => {
      
      if (!isAuthenticated) {
        return true; 
      }


      const roles = userData?.role || userData?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      let isAdmin = false;

      if (roles) {
        isAdmin = Array.isArray(roles) ? roles.includes('Admin') : roles === 'Admin';
      }

      if (isAdmin) {
        return router.createUrlTree(['/admin/dashboard']);
      } else {
        return router.createUrlTree(['/cardapio']);
      }
    })
  );
};