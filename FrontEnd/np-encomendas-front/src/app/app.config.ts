import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeng/themes/aura';
import { AbstractSecurityStorage, authInterceptor, provideAuth } from 'angular-auth-oidc-client';

export class LocalStorageService implements AbstractSecurityStorage {
    read(key: string) {
        return localStorage.getItem(key);
    }
    write(key: string, value: any) {
        localStorage.setItem(key, value);
    }
    remove(key: string) {
        localStorage.removeItem(key);
    }
    clear() {
        localStorage.clear();
    }
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    
    // Configura o HTTP para enviar o Token automaticamente nas requisições
    provideHttpClient(withFetch(), withInterceptors([authInterceptor()])),
    
    provideAnimationsAsync(),
    
    // Configuração visual (PrimeNG)
    providePrimeNG({
        theme: {
            preset: Aura,
            options: { darkModeSelector: false || 'none' }
        }
    }),


    provideAuth({
      config: {
        authority: 'https://nporder-identiity-server.onrender.com', 
        redirectUrl: window.location.origin + '/callback',
        postLogoutRedirectUri: window.location.origin,
        clientId: 'angular_client', 
        scope: 'openid profile nporder_api offline_access',
        responseType: 'code',
        silentRenew: true,
        useRefreshToken: true,
        renewTimeBeforeTokenExpiresInSeconds: 30,
        secureRoutes: ['https://backend-api-tk7o.onrender.com/']
      },
    }),

    { provide: AbstractSecurityStorage, useClass: LocalStorageService }

  ]
};