import { inject, Injectable } from "@angular/core";
import { CanActivate, Router, UrlTree } from "@angular/router";
import { OidcSecurityService } from "angular-auth-oidc-client";
import { map, Observable, take } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AdminAuthGuard implements CanActivate {
  private oidcSecurityService = inject(OidcSecurityService);
  private router = inject(Router);

  canActivate(): Observable<boolean | UrlTree> {

    return this.oidcSecurityService.checkAuth().pipe(
      take(1),
      map(({ isAuthenticated, userData }) => {
        

        if (!isAuthenticated) {
          return this.router.createUrlTree(['/login']);
        }


        const roles = userData?.role || userData?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

        let isAdmin = false;
        if (Array.isArray(roles)) {
          isAdmin = roles.includes('Admin');
        } else {
          isAdmin = roles === 'Admin';
        }


        if (isAdmin) {
          return true; 
        } else {
  
          return this.router.createUrlTree(['/cardapio']); 
        }
      })
    );
  }
}