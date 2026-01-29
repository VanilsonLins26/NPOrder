import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { InputNumberModule } from 'primeng/inputnumber';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { CartService } from '../../../core/services/cart.service';
import { CartItem, CartResponse } from '../../../core/models/cart.model';

@Component({
  selector: 'app-cart',
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    TableModule,
    ButtonModule,
    InputNumberModule,
    CardModule,
    ToastModule
  ],
  providers: [MessageService],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss'
})
export class CartComponent implements OnInit {
  private cartService = inject(CartService);
  private messageService = inject(MessageService);
  private router = inject(Router);

  cart: CartResponse | null = null;
  isLoading = false;

  ngOnInit() {
    this.loadCart();
  }

  loadCart() {
    this.isLoading = true;
    this.cartService.getCart().subscribe({
      next: (data) => {
        this.cart = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }

  updateQuantity(item: CartItem, newQtd: number) {
    if (newQtd < 1) return;
    
    this.cartService.updateQuantity(item.id, newQtd).subscribe({
      next: (updatedCart) => {
        this.cart = updatedCart;
      },
      error: () => this.showError('Erro ao atualizar quantidade')
    });
  }

  removeItem(productId: number) {
    this.cartService.removeItem(productId).subscribe({
      next: (updatedCart) => {
        this.cart = updatedCart;
        this.messageService.add({ severity: 'success', summary: 'Removido', detail: 'Item removido do carrinho.' });
      },
      error: () => this.showError('Erro ao remover item')
    });
  }

  clearCart() {
    this.cartService.clearCart().subscribe({
      next: (res) => {
        this.cart = res; 
        this.messageService.add({ severity: 'success', summary: 'Limpo', detail: 'Carrinho esvaziado.' });
      },
      error: () => this.showError('Erro ao limpar carrinho')
    });
  }

  checkout() {
    this.router.navigate(['/detalhes-pedido']);
  }

  private showError(msg: string) {
    this.messageService.add({ severity: 'error', summary: 'Erro', detail: msg });
  }
}