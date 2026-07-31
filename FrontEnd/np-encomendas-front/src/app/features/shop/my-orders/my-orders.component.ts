import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { OrderService } from '../../../core/services/order.service';
import { TooltipModule } from 'primeng/tooltip';
import { PaginatorModule } from 'primeng/paginator';

@Component({
  selector: 'app-my-orders',
  imports: [CommonModule, TableModule, ButtonModule, TagModule, RouterModule, CardModule, TooltipModule, PaginatorModule],
  templateUrl: './my-orders.component.html',
  styleUrl: './my-orders.component.scss'
})
export class MyOrdersComponent implements OnInit {
  private orderService = inject(OrderService);
  private router = inject(Router);

  orders: any[] = [];
  totalRecords: number = 0;
  loading: boolean = true;
  pageSize: number = 10;

  ngOnInit() {
    this.loadOrders({ first: 0, rows: 10 });
  }

  loadOrders(event: any) {
  this.loading = true;
  const pageNumber = (event.first / event.rows) + 1;
  
  this.orderService.getMyOrders(pageNumber, event.rows).subscribe({
    next: (response) => {
      this.orders = response.body || []; 

      const paginationHeader = response.headers.get('X-Pagination');

      if (paginationHeader) {
        const metadata = JSON.parse(paginationHeader);
        
        this.totalRecords = metadata.TotalCount;
      } else {
        this.totalRecords = this.orders.length;
      }

      this.loading = false;
    },
    error: (err) => {
      console.error('Erro ao carregar pedidos', err);
      this.loading = false;
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

  viewDetails(orderId: number) {
    this.router.navigate(['/meus-pedidos', orderId]);
  }

  getStatusSeverity(status: string): 'success' | 'info' | 'warn' | 'danger' | undefined {
    switch (status.toLowerCase()) {
      case 'delivered': case 'confirmed' : return 'success';
      case 'canceled': return 'danger';
      case 'pendingpayment': return 'warn';
      case 'outfordelivery': return 'info';
      default: return 'info';
    }
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

  getStatusAccentClass(status: string): string {
    switch (status?.toLowerCase()) {
      case 'pendingpayment':  return 'accent-pending';
      case 'confirmed':       return 'accent-confirmed';
      case 'readyforpickup':  return 'accent-confirmed';
      case 'outfordelivery':  return 'accent-delivery';
      case 'delivered':       return 'accent-delivered';
      case 'canceled':        return 'accent-cancelled';
      default:                return 'accent-default';
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

}