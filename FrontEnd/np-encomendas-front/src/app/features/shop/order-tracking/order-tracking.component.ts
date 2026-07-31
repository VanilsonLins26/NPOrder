import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DividerModule } from 'primeng/divider';
import { TagModule } from 'primeng/tag';
import { TimelineModule } from 'primeng/timeline';
import { ToastModule } from 'primeng/toast';
import { OrderService } from '../../../core/services/order.service';

@Component({
  selector: 'app-order-tracking',
  imports: [CommonModule, ButtonModule, CardModule, TagModule, DividerModule, TimelineModule, ToastModule],
  providers: [MessageService],
  templateUrl: './order-tracking.component.html',
  styleUrl: './order-tracking.component.scss'
})
export class OrderTrackingComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private orderService = inject(OrderService);
  private messageService = inject(MessageService);

  order: any = null;
  isLoading = true;
  actionLoading = false;

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadOrder(Number(id));
    }
  }

  loadOrder(id: number) {
    this.isLoading = true;
    this.orderService.getOrderById(id).subscribe({
      next: (res) => {
        this.order = res;
        this.isLoading = false;
      },
      error: () => {
        this.messageService.add({severity:'error', summary:'Erro', detail:'Pedido não encontrado'});
        this.router.navigate(['/meus-pedidos']);
      }
    });
  }

  confirmReceipt() {
    if(!this.order) return;
    this.actionLoading = true;
    
    this.orderService.confirmDelivery(this.order.id).subscribe({
      next: (res) => {
        this.order = res; 
        this.messageService.add({severity:'success', summary:'Sucesso', detail:'Entrega confirmada!'});
        this.actionLoading = false;
      },
      error: (err) => {
        this.messageService.add({severity:'error', summary:'Erro', detail: err.error || 'Falha ao confirmar'});
        this.actionLoading = false;
      }
    });
  }

  cancelOrder() {
    if(!this.order) return;
    this.actionLoading = true;

    this.orderService.cancelOrder(this.order.id).subscribe({
      next: (res) => {
        this.order = res;
        this.messageService.add({severity:'success', summary:'Cancelado', detail:'Pedido cancelado com sucesso.'});
        this.actionLoading = false;
      },
      error: (err) => {
        this.messageService.add({severity:'error', summary:'Erro', detail: err.error || 'Não foi possível cancelar'});
        this.actionLoading = false;
      }
    });
  }

  getStatusLabel(status: string): string {
      const statusMap: { [key: string]: string } = {
          'PendingPayment': 'Aguardando Pagamento',
          'Confirmed': 'Pedido Confirmado',
          'ReadyForPickup': 'Pronto para Retirada',
          'OutForDelivery': 'Saiu para Entrega',
          'Delivered': 'Entregue',
          'Canceled': 'Cancelado'
      };
      
      return statusMap[status] || status;
  }

  getStatusBadgeClass(status: string): string {
    switch (status?.toLowerCase()) {
      case 'pendingpayment':  return 'status-pending';
      case 'confirmed':       return 'status-confirmed';
      case 'readyforpickup':  return 'status-ready';
      case 'outfordelivery':  return 'status-delivery';
      case 'delivered':       return 'status-delivered';
      case 'canceled':        return 'status-cancelled';
      default:                return 'status-pending';
    }
  }

  getStatusIcon(status: string): string {
    switch (status?.toLowerCase()) {
      case 'pendingpayment':  return 'pi-clock';
      case 'confirmed':       return 'pi-check-circle';
      case 'readyforpickup':  return 'pi-shopping-bag';
      case 'outfordelivery':  return 'pi-send';
      case 'delivered':       return 'pi-verified';
      case 'canceled':        return 'pi-times-circle';
      default:                return 'pi-info-circle';
    }
  }

  getOrderSteps(): { label: string; desc: string; icon: string; done: boolean; active: boolean }[] {
    const s = this.order?.statusName?.toLowerCase() || '';
    const steps = [
      { key: 'pendingpayment', label: 'Aguardando Pagamento', desc: 'Confirme o pagamento para prosseguir', icon: 'pi-clock' },
      { key: 'confirmed',      label: 'Pedido Confirmado',    desc: 'Estamos preparando com carinho',       icon: 'pi-check-circle' },
      { key: 'readyforpickup', label: 'Pronto para Retirada', desc: 'Venha buscar na loja',                  icon: 'pi-shopping-bag' },
      { key: 'outfordelivery', label: 'Saiu para Entrega',    desc: 'A caminho da sua casa!',                icon: 'pi-send' },
      { key: 'delivered',      label: 'Entregue',             desc: 'Aproveite! 🎉',                          icon: 'pi-verified' },
    ];
    const order = ['pendingpayment','confirmed','readyforpickup','outfordelivery','delivered'];
    const current = order.indexOf(s);
    return steps.map((step, i) => ({
      ...step,
      done:   i < current,
      active: i === current,
    }));
  }

  handlePayment() {
    if (!this.order) return;

    if (this.order.payment && this.order.payment.paymentUrl) {
        window.open(this.order.payment.paymentUrl, '_blank');
    } 
    else {
        this.router.navigate(['/checkout', this.order.id]);
    }
  }

  goBack() {
    this.router.navigate(['/meus-pedidos']);
  }
}