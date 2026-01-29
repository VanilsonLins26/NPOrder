import { Component } from '@angular/core';
import { MessageService } from 'primeng/api';
import { PaymentResponseDTO } from '../../../core/models/Payment.model';
import { PaymentService } from '../../../core/services/payment.service';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';
import { InputTextModule } from 'primeng/inputtext';
import { DatePickerModule } from 'primeng/datepicker';
import { SelectModule } from 'primeng/select';
import { IconFieldModule } from 'primeng/iconfield';   
import { InputIconModule } from 'primeng/inputicon';

@Component({
  selector: 'app-payment-list',
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    DialogModule,
    ToastModule,
    TagModule,
    TooltipModule,
    InputTextModule,
    DatePickerModule,
    SelectModule,
    IconFieldModule,
    InputIconModule
  ],
  templateUrl: './payment-list.component.html',
  styleUrl: './payment-list.component.scss',
  providers: [MessageService]
})
export class PaymentListComponent {

  payments: PaymentResponseDTO[] = [];
  totalRecords: number = 0;
  loading: boolean = true;
  pageSize: number = 10;
  status?: string = '';
  filterDates: Date[] | undefined;
  searchText: string = '';

  lastTableEvent: any;
  displayDialog: boolean = false;
  selectedPayment: PaymentResponseDTO | null = null;

  statusOptions = [
    { label: 'Aprovado', value: 'approved', severity: 'success' },
    { label: 'Pendente', value: 'pendente', severity: 'warning' },
    { label: 'Cancelado', value: 'cancelled', severity: 'danger' }
  ];

  constructor(
    private paymentService: PaymentService,
    private messageService: MessageService
  ) { }

  loadPayments(event: TableLazyLoadEvent) {
    this.loading = true;
    this.lastTableEvent = event; 

    const pageNumber = (event.first! / event.rows!) + 1;
    const pageSize = event.rows!;


    const statusEnvio = this.status || undefined;

    this.paymentService.getPayments(
      pageNumber,
      pageSize,
      statusEnvio,    
      this.filterDates, 
      this.searchText  
    ).subscribe({
      next: (response) => {

        this.payments = response.body || [];

        const paginationHeader = response.headers.get('X-Pagination');

        if (paginationHeader) {
          try {
            const metadata = JSON.parse(paginationHeader);
            this.totalRecords = metadata.TotalCount || metadata.totalCount || 0;
          } catch (e) {
            console.error('Erro no X-Pagination:', e);
            this.totalRecords = this.payments.length;
          }
        } else {
          this.totalRecords = this.payments.length;
        }

        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  applyFilters() {
    const event: TableLazyLoadEvent = {
      first: 0,
      rows: 10
    };
    this.loadPayments(event);
  }

  clearFilters() {
    this.status = undefined;
    this.filterDates = undefined;
    this.searchText = '';
    this.applyFilters();
  }

  showDetails(payment: PaymentResponseDTO) {
    this.selectedPayment = payment;
    this.displayDialog = true;
  }


  getSeverity(status: string): "success" | "secondary" | "info" | "warn" | "danger" | "contrast" | undefined {
    switch (status?.toLowerCase()) {
      case 'approved': return 'success';
      case 'pending': return 'warn';
      case 'in_process': return 'info';
      case 'rejected': return 'danger';
      case 'cancelled': return 'danger';
      case 'refunded': return 'info';
      default: return 'secondary';
    }
  }
  translateStatus(status: string): string {
    const map: { [key: string]: string } = {
      'approved': 'Aprovado',
      'pending': 'Pendente',
      'in_process': 'Em processamento',
      'rejected': 'Recusado',
      'cancelled': 'Cancelado',
      'refunded': 'Estornado'
    };
    return map[status.toLowerCase()] || status;
  }


  translatePaymentMethod(type: string, methodId: string): string {
    const typeMap: { [key: string]: string } = {
      'account_money': 'Saldo Mercado Pago',
      'ticket': 'Boleto Bancário',
      'bank_transfer': 'Pix / Transferência',
      'credit_card': 'Cartão de Crédito',
      'debit_card': 'Cartão de Débito',
      'prepaid_card': 'Cartão Pré-pago',
      'atm': 'Caixa Eletrônico',
      'digital_currency': 'Moeda Digital'
    };


    if (methodId === 'pix') return 'Pix';
    if (methodId === 'bolbradesco') return 'Boleto Bradesco';

    return typeMap[type] || type || methodId;
  }

  translateStatusDetail(detail: string): string {
    if (!detail) return '';

    const detailMap: { [key: string]: string } = {

      'accredited': 'Pagamento creditado',


      'pending_contingency': 'Processando o pagamento',
      'pending_review_manual': 'Em análise de segurança',
      'deferred_retry': 'Aguardando confirmação bancária',

      'cc_rejected_bad_filled_card_number': 'Número do cartão incorreto',
      'cc_rejected_bad_filled_date': 'Data de validade incorreta',
      'cc_rejected_bad_filled_other': 'Dados do cartão incorretos',
      'cc_rejected_bad_filled_security_code': 'Código de segurança incorreto',
      'cc_rejected_blacklist': 'Cartão recusado',
      'cc_rejected_call_for_authorize': 'Não autorizado (Ligar para o banco)',
      'cc_rejected_card_disabled': 'Cartão desabilitado',
      'cc_rejected_card_error': 'Erro no cartão',
      'cc_rejected_duplicated_payment': 'Pagamento duplicado',
      'cc_rejected_high_risk': 'Recusado por prevenção a fraude',
      'cc_rejected_insufficient_amount': 'Saldo insuficiente',
      'cc_rejected_invalid_installments': 'Número de parcelas inválido',
      'cc_rejected_max_attempts': 'Máximo de tentativas excedido',
      'cc_rejected_other_reason': 'Recusado pelo banco emissor'
    };

    return detailMap[detail] || detail;
  }

  openPaymentLink(url: string | undefined) {
    if (url) {
      window.open(url, '_blank');
    }
  }
}