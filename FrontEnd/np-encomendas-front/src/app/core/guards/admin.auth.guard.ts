import { inject, Injectable } from "@angular/core";
import { CanActivate, Router, UrlTree } from "@angular/router";
import { OidcSecurityService } from "angular-auth-oidc-client";
import { map, Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AdminAuthGuard implements CanActivate {
  private oidcSecurityService = inject(OidcSecurityService);
  private router = inject(Router);

  canActivate(): Observable<boolean | UrlTree> {
    return this.oidcSecurityService.userData$.pipe(
      map(({ userData }) => {
        if (!userData) {
          return this.router.createUrlTree(['/unauthorized']);
        }

        const roles = userData.role || userData['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

        let isAdmin = false;

        if (Array.isArray(roles)) {
          isAdmin = roles.includes('Admin');
        } else {
          isAdmin = roles === 'Admin';
        }

        if (isAdmin) {
          return true;
        } else {
          return this.router.createUrlTree(['/']); 
        }
      })
    );
  }
}