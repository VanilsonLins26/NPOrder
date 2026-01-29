import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { PaginatorModule } from 'primeng/paginator';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models/product.model';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { FormsModule } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { CartService } from '../../../core/services/cart.service';
import { TextareaModule } from 'primeng/textarea';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';

@Component({
  selector: 'app-product-list',
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
    TagModule,
    PaginatorModule,
    DialogModule,
    InputNumberModule,
    TextareaModule,
    ToastModule,
    IconFieldModule, InputIconModule, InputTextModule
  ],
  providers: [MessageService],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent implements OnInit {
  private productService = inject(ProductService);
  private cartService = inject(CartService);
  private oidcSecurityService = inject(OidcSecurityService);
  private messageService = inject(MessageService);
  filterName: string = '';
  minPrice: number | undefined;
  maxPrice: number | undefined;
  private searchSubject = new Subject<string>();
  products: Product[] = [];
  isAuthenticated = false;

  totalRecords: number = 0;
  pageSize: number = 12;
  pageNumber: number = 1;
  displayModal: boolean = false;
  selectedProduct: Product | null = null;
  quantity: number = 1;
  comment: string = '';

  ngOnInit() {
    this.loadProducts();
    this.oidcSecurityService.isAuthenticated$.subscribe(({ isAuthenticated }) => {
      this.isAuthenticated = isAuthenticated;
    });
    this.searchSubject.pipe(
      debounceTime(500),
      distinctUntilChanged()
    ).subscribe(searchValue => {
      this.filterName = searchValue;
      this.pageNumber = 1;
      this.loadProducts();
    });
  }

onSearch(event: any) {
    this.searchSubject.next(event.target.value);
  }

  onPriceFilter() {
    this.pageNumber = 1;
    this.loadProducts();
  }


  loadProducts() {
    this.productService.getProducts(
        this.pageNumber, 
        this.pageSize, 
        this.filterName, 
        this.minPrice, 
        this.maxPrice
    ).subscribe({
      next: (response) => {

        this.products = response.data;

        if (response.meta) {
          this.totalRecords = response.meta.TotalCount;
        }
      },
      error: (err) => {
        console.error(err);
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Não foi possível carregar os produtos.' });
      }
    });
  }

  onPageChange(event: any) {
    this.pageNumber = (event.first / event.rows) + 1;
    this.pageSize = event.rows;
    this.loadProducts();
  }


  openBuyModal(product: Product) {
    if (!this.isAuthenticated) {
      this.oidcSecurityService.authorize();
      return;
    }

    this.selectedProduct = product;
    this.quantity = 1;
    this.comment = '';
    this.displayModal = true;
  }

  confirmAddToCart() {
    if (!this.selectedProduct) return;

    this.cartService.addToCart({
      productId: this.selectedProduct.id,
      quantity: this.quantity,
      comment: this.comment
    }).subscribe({
      next: (res) => {
        this.displayModal = false;
        this.messageService.add({
          severity: 'success',
          summary: 'Sucesso',
          detail: `${this.selectedProduct?.name} adicionado ao carrinho!`
        });
      },
      error: (err) => {
        console.error(err);
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Erro ao adicionar ao carrinho.' });
      }
    });
  }
}