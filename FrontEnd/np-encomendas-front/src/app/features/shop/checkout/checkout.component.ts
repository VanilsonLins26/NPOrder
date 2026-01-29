import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DividerModule } from 'primeng/divider';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { RadioButtonModule } from 'primeng/radiobutton';
import { ToastModule } from 'primeng/toast';
import { PaymentService } from '../../../core/services/payment.service';
import { OrderService } from '../../../core/services/order.service';

@Component({
  selector: 'app-checkout',
  imports: [
    CommonModule,
    FormsModule,
    CardModule,
    ButtonModule,
    RadioButtonModule,
    DividerModule,
    ToastModule,
    ProgressSpinnerModule,
    RouterModule
  ],
  providers: [MessageService],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private paymentService = inject(PaymentService);
  private orderService = inject(OrderService);
  private messageService = inject(MessageService);

  orderId: number = 0;
  orderTotal: number = 0; 
  
  selectedOption: number = 100; 
  isLoading = false;
  isProcessingPayment = false;

  ngOnInit() {

    this.orderId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.orderId) {
      this.loadOrderDetails();
    }
  }

  loadOrderDetails() {
    this.isLoading = true;

    this.orderService.getOrderById(this.orderId).subscribe({
        next: (order) => {
            this.orderTotal = order.totalAmount;
            this.isLoading = false;
        },
        error: () => {
            this.isLoading = false;
            this.messageService.add({severity:'error', summary:'Erro', detail:'Pedido não encontrado'});
        }
    });
  }

  goToMercadoPago() {
    this.isProcessingPayment = true;

    this.paymentService.createPreference({
        orderId: this.orderId,
        percentToPay: this.selectedOption
    }).subscribe({
        next: (response) => {

            if(response.redirectUrl) {
                window.location.href = response.redirectUrl;
            } else {

                 window.location.href = (response as any).sandboxInitPoint || (response as any).initPoint;
            }
        },
        error: (err) => {
            console.error(err);
            this.isProcessingPayment = false;
            this.messageService.add({severity:'error', summary:'Erro', detail:'Falha ao conectar com Mercado Pago'});
        }
    });
  }
}