import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { TooltipModule } from 'primeng/tooltip';
import { DeliveryMethod, Order, Status } from '../../../core/models/order.model';
import { OrderService } from '../../../core/services/order.service';
import { SelectButtonModule } from 'primeng/selectbutton';
import { DatePicker, DatePickerModule } from 'primeng/datepicker';
import { Select, SelectModule } from 'primeng/select';
import { InputIconModule } from 'primeng/inputicon';
import { IconFieldModule } from 'primeng/iconfield';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-orders-admin',
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, ToolbarModule,
    DialogModule, ConfirmDialogModule, ToastModule, TagModule, TooltipModule,
    SelectButtonModule, DatePickerModule, SelectModule, InputIconModule,IconFieldModule,
    InputTextModule

  ],
  providers: [ ConfirmationService],
  templateUrl: './orders-admin.component.html',
  styleUrl: './orders-admin.component.scss'
})
export class OrdersAdminComponent {

  orders: Order[] = [];
  selectedOrder: Order | null = null;
  orderDialog: boolean = false;
  loading: boolean = true;
  totalRecords: number = 0;
  filterText: string = '';
  filterDates: Date[] | undefined;
  selectedStatusFilter: string | undefined;
  lastLazyLoadEvent: any;

  viewOptions = [
    { label: 'Em Produção', value: 1 },      
    { label: 'Histórico', value: 2 },
    { label: 'Pendente', value: 3 }          
];
  currentViewModel?: number = 1;

  status?: string = undefined;
  DeliveryMethod = DeliveryMethod;

  constructor(
    private orderService: OrderService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) { }

 loadOrders(event: TableLazyLoadEvent) {
  this.lastLazyLoadEvent = event;
  this.loading = true;
  
  const pageNumber = (event.first! / event.rows!) + 1;
  const pageSize = event.rows!;

  this.orderService.getAllOrdersAdmin(
      pageNumber,
      pageSize,
      this.currentViewModel, 
      this.filterText,       
      this.status,            
      this.filterDates       
    ).subscribe({
    next: (response) => {
      this.orders = response.body || [];

 
      this.orders.forEach((order: any) => {
        if (order.deliverTime) {
          const date = new Date(order.deliverTime);
          date.setHours(0, 0, 0, 0); 
          order.groupKey = date.toDateString(); 
        }
      });

      const paginationHeader = response.headers.get('X-Pagination');
      
      if (paginationHeader) {
        const metadata = JSON.parse(paginationHeader);
        this.totalRecords = metadata.TotalCount || metadata.totalCount || 0;
      } else {
        this.totalRecords = this.orders.length; 
      }

      this.loading = false;
    },
    error: () => {
      this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Erro ao carregar encomendas' });
      this.loading = false;
    }
  });
}

  onViewChange() {
    this.selectedStatusFilter = undefined; 
    
    this.applyFilters(); 
}

  viewOrder(order: Order) {

    this.loading = true;
    this.orderService.getOrderById(order.id).subscribe({
      next: (fullOrder) => {
        this.selectedOrder = fullOrder;
        this.orderDialog = true;
        this.loading = false;
        console.log("status "+ this.status)
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Não foi possível carregar detalhes' });
        this.loading = false;
      }
    });
  }
clearFilters() {
    this.filterText = '';
    this.filterDates = undefined;
    this.selectedStatusFilter = undefined;
    this.applyFilters();
}

applyFilters() {
    const event: TableLazyLoadEvent = { first: 0, rows: 10 };
    this.loadOrders(event);
}

cleanAndApplyFilters() {
    const event: TableLazyLoadEvent = { first: 0, rows: 10 };
    this.currentViewModel = undefined;
    this.loadOrders(event);
}


  changeStatus(action: 'pickup' | 'delivery' | 'cancel' | 'delivered') {
    if (!this.selectedOrder) return;

    let obs$;
    let successMsg = '';

    switch (action) {
      case 'pickup':
        obs$ = this.orderService.markReadyForPickup(this.selectedOrder.id);
        successMsg = 'Pronto para Retirada!';
        break;
      case 'delivery':
        obs$ = this.orderService.markOutForDelivery(this.selectedOrder.id);
        successMsg = 'Saiu para Entrega!';
        break;
      case 'delivered':
        obs$ = this.orderService.confirmDelivery(this.selectedOrder.id);
        successMsg = 'Encomenda entregue com sucesso!';
        break;
      case 'cancel':
        this.confirmCancel();

        if (this.lastLazyLoadEvent) {
                this.loadOrders(this.lastLazyLoadEvent);
            }
        return;
        
    }

    if (obs$) {
      this.loading = true;
      obs$.subscribe({
        next: (updatedOrder) => {
          this.selectedOrder = updatedOrder;


          const index = this.orders.findIndex(o => o.id === updatedOrder.id);
          if (index !== -1) {
      
             if (this.currentViewModel === 1 && action === 'delivered') {
                this.orders.splice(index, 1);
                this.totalRecords--; 
                this.orderDialog = false;
                
             } else {
                this.orders[index] = updatedOrder;
             }
          }
          
          this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: successMsg });
          this.loading = false;
        },
        error: (err) => {
          const msg = err.error || 'Falha na operação';
          this.messageService.add({ severity: 'error', summary: 'Erro', detail: msg });
          this.loading = false;
        }
      });
    }
  }

  confirmCancel() {
    this.confirmationService.confirm({
      message: 'Tem certeza que deseja cancelar esta encomenda? Essa ação não pode ser desfeita.',
      header: 'Confirmar Cancelamento',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => {
        this.orderService.cancelOrder(this.selectedOrder!.id).subscribe({
          next: (updatedOrder) => {
            this.selectedOrder = updatedOrder;
            const index = this.orders.findIndex(o => o.id === updatedOrder.id);
            if (index !== -1) this.orders[index] = updatedOrder;
            this.messageService.add({ severity: 'success', summary: 'Cancelado', detail: 'Encomenda cancelada.' });
          },
          error: (err) => this.messageService.add({ severity: 'error', summary: 'Erro', detail: err.error || 'Erro ao cancelar' })
        });
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

  getSeverity(statusName: string): "success" | "secondary" | "info" | "warn" | "danger" | "contrast" | undefined {
    switch (statusName?.toLowerCase()) {
      case 'pedido confirmado': return 'warn';       
      case 'pronto para retirada': return 'info';    
      case 'saiu para entrega': return 'info';       
      case 'entregue': return 'success';            
      case 'cancelado': return 'danger';           
      default: return 'secondary';
    }
  }

  statusOptions = [
        { label: 'Pendente', value: 'PendingPayment', severity: 'warning' },
        { label: 'Confirmado', value: 'Confirmed', severity: 'success' },
        { label: 'Pronto para Retirada', value: 'ReadyForPickup', severity: 'info' },
        { label: 'Saiu para Entrega', value: 'OutForDelivery', severity: 'help' },
        { label: 'Entregue', value: 'Delivered', severity: 'success' },
        { label: 'Cancelado', value: 'Canceled', severity: 'danger' }
    ];

  isToday(dateString: string): boolean {
  if (!dateString) return false;
  const d = new Date(dateString);
  const t = new Date();
  return d.setHours(0,0,0,0) === t.setHours(0,0,0,0);
}

getGroupLabel(dateString: string): string {
  if (!dateString) return 'Data não informada';

  const date = new Date(dateString);
  const today = new Date();
  const tomorrow = new Date();
  tomorrow.setDate(today.getDate() + 1);

  const d = date.setHours(0,0,0,0);
  const t = today.setHours(0,0,0,0);
  const tm = tomorrow.setHours(0,0,0,0);

  if (d === t) {
      return 'HOJE';
  } else if (d === tm) {
      return 'AMANHÃ';
  } else {
      return new Date(dateString).toLocaleDateString('pt-BR');
  }
}
}