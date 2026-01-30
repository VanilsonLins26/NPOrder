import { APP_INITIALIZER, ApplicationConfig, inject, provideAppInitializer, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeng/themes/aura';
import { AbstractSecurityStorage, authInterceptor, OidcSecurityService, provideAuth } from 'angular-auth-oidc-client';

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

export function configureAuth(oidcSecurityService: OidcSecurityService) {
  return () => oidcSecurityService.checkAuth();
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),

    provideHttpClient(withFetch(), withInterceptors([authInterceptor()])),
    
    provideAnimationsAsync(),
    
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

    { provide: AbstractSecurityStorage, useClass: LocalStorageService },

    provideAppInitializer(() => {
        const authService = inject(OidcSecurityService);
        return authService.checkAuth();
    }),

    providePrimeNG({
        theme: {
            preset: Aura,
            options: { darkModeSelector: false || 'none' }
        },
        // ADICIONE ISTO AQUI DENTRO:
        translation: {
          accept: 'Sim',
          reject: 'Não',
          dayNames: ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado"],
          dayNamesShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"],
          dayNamesMin: ["D","S","T","Q","Q","S","S"],
          monthNames: ["Janeiro","Fevereiro","Março","Abril","Maio","Junho","Julho","Agosto","Setembro","Outubro","Novembro","Dezembro"],
          monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun","Jul", "Ago", "Set", "Out", "Nov", "Dez"],
          today: 'Hoje',
          clear: 'Limpar',
          weak: 'Fraca',
          medium: 'Média',
          strong: 'Forte',
          passwordPrompt: 'Digite uma senha',
          emptyMessage: 'Nenhum resultado encontrado',
          emptyFilterMessage: 'Nenhum resultado encontrado'
        }
    }),

  ]
};