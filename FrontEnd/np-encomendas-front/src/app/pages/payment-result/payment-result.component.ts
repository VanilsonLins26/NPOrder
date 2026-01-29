import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-payment-result',
  imports: [CommonModule, CardModule, ButtonModule],
  templateUrl: './payment-result.component.html',
  styles: [`
    .result-card { max-width: 500px; margin: 0 auto; text-align: center; }
    .icon-large { font-size: 4rem; margin-bottom: 1rem; }
  `]
})
export class PaymentResultComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  type: 'success' | 'failure' | 'pending' = 'pending';
  paymentId: string = '';
  title: string = '';
  message: string = '';
  icon: string = '';
  colorClass: string = '';

  ngOnInit() {
    
    this.route.queryParams.subscribe(params => {
      this.paymentId = params['payment_id'] || params['collection_id'] || '';
      
      const mpStatus = params['collection_status'];
      
      const routeStatus = this.route.snapshot.url[1]?.path;

      this.configureScreen(mpStatus || routeStatus);
    });
  }

 configureScreen(status: string) {
    switch (status) {
      case 'approved': 
      case 'success':  
        this.type = 'success';
        this.title = 'Pagamento Aprovado!';
        this.message = `Seu pedido foi processado com sucesso. Guarde o número do pagamento: ${this.paymentId}`;
        this.icon = 'pi pi-check-circle';
        this.colorClass = 'text-green-500';
        break;

      case 'rejected':
      case 'cancelled':
      case 'failure': 
        this.type = 'failure';
        this.title = 'Pagamento Recusado';
        this.message = 'Houve um problema com seu pagamento. Verifique os dados do cartão ou tente outro meio.';
        this.icon = 'pi pi-times-circle';
        this.colorClass = 'text-red-500';
        break;

      case 'pending':
      case 'in_process':
      case 'authorized':
      default:
        this.type = 'pending';
        this.title = 'Pagamento em Análise';
        this.message = 'Estamos processando seu pagamento. Você receberá uma notificação assim que for confirmado.';
        this.icon = 'pi pi-clock';
        this.colorClass = 'text-yellow-500';
        break;
    }
  }

  goToOrders() {
    this.router.navigate(['/meus-pedidos']);
  }
}
