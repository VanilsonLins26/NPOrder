import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { RadioButtonModule } from 'primeng/radiobutton';
import { SelectButtonModule } from 'primeng/selectbutton';
import { ToastModule } from 'primeng/toast';
import { DatePickerModule } from 'primeng/datepicker';
import { MessageService } from 'primeng/api';
import { OrderService } from '../../../core/services/order.service';
import { Router } from '@angular/router';
import { CreateOrderDTO } from '../../../core/models/order.model';
import { AddressRequest, AddressResponse } from '../../../core/models/address.model';
import { TagModule } from 'primeng/tag';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { AddressService } from '../../../core/services/address.service';
import { DividerModule } from 'primeng/divider';

@Component({
  selector: 'app-order-details',
imports: [
    CommonModule, FormsModule, CardModule, ButtonModule, 
    SelectButtonModule, DatePickerModule, RadioButtonModule, ToastModule,
    TagModule, DialogModule, InputTextModule, DividerModule
  ],
  providers: [MessageService],
  templateUrl: './order-details.component.html',
  styleUrl: './order-details.component.scss'
})
export class OrderDetailsComponent implements OnInit {
  private orderService = inject(OrderService);
  private addressService = inject(AddressService);
  private router = inject(Router);
  private messageService = inject(MessageService);

  deliveryOptions = [
    { label: 'Entrega (Delivery)', value: 'Delivery', icon: 'pi pi-truck' },
    { label: 'Retirada na Loja', value: 'Pickup', icon: 'pi pi-shopping-bag' }
  ];

  selectedMethod: 'Delivery' | 'Pickup' = 'Delivery';
  selectedDate: Date | undefined;
  minDate: Date = new Date(); 
  
  userAddresses: AddressResponse[] = [];
  selectedAddress: AddressResponse | null = null;

  showAddressModal = false;
  isCreatingAddress = false; 
  
  newAddressForm: AddressRequest = {
    street: '', number: '', district: '', zipCode: '', complement: ''
  };
  
  isLoading = false;

  ngOnInit() {
    this.loadAddresses();
    this.initializeTimeSlot();
    this.minDate.setDate(this.minDate.getDate() + 1); 
  }

  initializeTimeSlot() {
    const now = new Date();
    const minutes = now.getMinutes();

    const nextSlot = new Date(now);
    nextSlot.setSeconds(0);
    nextSlot.setMilliseconds(0);

    if (minutes < 30) {

        nextSlot.setMinutes(30);
    } else {

        nextSlot.setMinutes(0);
        nextSlot.setHours(nextSlot.getHours() + 1);
    }

    this.selectedDate = nextSlot;
    
    this.minDate = nextSlot; 
  }

  loadAddresses() {
    this.addressService.getAllAddresses().subscribe({
      next: (data) => {
        this.userAddresses = data;
        this.selectDefaultAddress();
      },
      error: () => this.messageService.add({severity:'error', summary:'Erro', detail:'Erro ao carregar endereços.'})
    });
  }

  selectDefaultAddress() {
    if (this.userAddresses.length > 0) {
      const defaultAddr = this.userAddresses.find(a => a.isDefault);
      
      if (defaultAddr) {
        this.selectedAddress = defaultAddr;
      } else {
        this.selectedAddress = this.userAddresses[0];
      }
    } else {
        this.selectedAddress = null;
    }
  }

  openAddressModal() {
    this.showAddressModal = true;
    this.isCreatingAddress = false; 
  }

  

  selectAddressFromModal(addr: AddressResponse) {
    this.selectedAddress = addr;
    this.showAddressModal = false;
  }

  saveNewAddress() {
    if(!this.newAddressForm.street || !this.newAddressForm.number || !this.newAddressForm.zipCode || !this.newAddressForm.district) {
        this.messageService.add({severity:'warn', summary:'Atenção', detail:'Preencha os campos obrigatórios.'});
        return;
    }

    this.isLoading = true;
    this.addressService.createAddress(this.newAddressForm).subscribe({
        next: (createdAddr) => {

            this.userAddresses.push(createdAddr);
            this.selectedAddress = createdAddr;
            
            this.isLoading = false;
            this.showAddressModal = false; 
            this.resetForm();
            this.messageService.add({severity:'success', summary:'Sucesso', detail:'Endereço cadastrado!'});
        },
        error: (err) => {
            this.isLoading = false;
            console.error(err);
            this.messageService.add({severity:'error', summary:'Erro', detail:'Falha ao salvar endereço.'});
        }
    });
  }

  resetForm() {
      this.newAddressForm = { street: '', number: '', district: '', zipCode: '', complement: '' };
      this.isCreatingAddress = false;
  }

  submitOrder() {
    if (!this.selectedDate) {
      this.messageService.add({severity:'warn', summary:'Atenção', detail:'Selecione a data e hora.'});
      return;
    }

    if (this.selectedMethod === 'Delivery' && !this.selectedAddress) {
      this.messageService.add({severity:'error', summary:'Erro', detail:'É necessário um endereço para entrega.'});
      return;
    }

    this.isLoading = true;

    const orderDTO: CreateOrderDTO = {
      deliveryMethod: this.selectedMethod === 'Delivery' ? 2 : 1,
      deliveryTime: this.selectedDate,
      addressId: this.selectedMethod === 'Delivery' ? this.selectedAddress?.id : null
    };

    this.orderService.createOrder(orderDTO).subscribe({
      next: (data) => {
        this.router.navigate(['/checkout',data.id]);
      },
      error: (err) => {
        this.isLoading = false;
        this.messageService.add({severity:'error', summary:'Erro', detail:'Falha ao criar o pedido.'});
      }
    });
  }

  
}