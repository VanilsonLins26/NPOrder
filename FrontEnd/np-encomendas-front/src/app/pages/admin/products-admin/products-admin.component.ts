import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { Product, Promotion } from '../../../core/models/product.model';
import { ProductService } from '../../../core/services/product.service';
import { FileUploadModule } from 'primeng/fileupload';
import { TagModule } from 'primeng/tag';
import { CheckboxModule } from 'primeng/checkbox';
import { DatePickerModule } from 'primeng/datepicker';
import { ToggleSwitch } from 'primeng/toggleswitch';
import { TooltipModule } from 'primeng/tooltip';
import { SelectModule } from 'primeng/select';
import { AppComponent } from '../../../app.component';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';

@Component({
  selector: 'app-products-admin',
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, ToolbarModule,
    InputTextModule, InputTextModule, DialogModule, ConfirmDialogModule,
    ToastModule, InputNumberModule, FileUploadModule, TagModule,CheckboxModule,
    DatePickerModule, ToggleSwitch, TooltipModule, SelectModule,IconFieldModule,
    InputIconModule, InputTextModule

  ],
  providers: [ ConfirmationService],
  templateUrl: './products-admin.component.html',
  styleUrl: './products-admin.component.scss'
})
export class ProductsAdminComponent {

  products: Product[] = [];
  productDialog: boolean = false;
  product: Product = this.getEmptyProduct();
  submitted: boolean = false;
  loading: boolean = true;
  selectedFile: File | undefined;
  imagePreview: string | null = null;
  filterName: string = '';
  private searchSubject = new Subject<string>();

  totalRecords: number = 0;
  pageSize: number = 10;
  currentPage: number = 1;

  promoDialog: boolean = false;
  newPromo: Promotion = this.getEmptyPromo();
  submittedPromo: boolean = false;

  units = [
    { label: 'Unidade (Un)', value: 'Un' },
    { label: 'Quilo (Kg)', value: 'Kg' },
    { label: 'Grama (g)', value: 'g' },
    { label: 'Litro (L)', value: 'L' },
    { label: 'Fatia', value: 'Fatia' },
    { label: 'Caixa', value: 'Caixa' }
  ];

  constructor(
    private productService: ProductService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) { 
    this.searchSubject.pipe(debounceTime(500), distinctUntilChanged())
          .subscribe(val => {
              this.filterName = val;
              this.loadProducts({ first: 0, rows: this.pageSize }); 
          });}

  onFileSelect(event: any) {
    const file = event.files[0];
    this.selectedFile = file;

    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.imagePreview = e.target.result;
    };
    reader.readAsDataURL(file);
  }

  onSearch(event: any) {
      this.searchSubject.next(event.target.value);
  }

  cleanSearch() {
    this.filterName = ''; 
    this.loadProducts({ first: 0, rows: 10 });
}


  loadProducts(event: TableLazyLoadEvent) {
    this.loading = true;

    const pageNumber = (event.first! / event.rows!) + 1;
    const pageSize = event.rows!;

    if (event.rows) this.pageSize = event.rows; 

    this.productService.getProducts(pageNumber, this.pageSize, this.filterName).subscribe({
      next: (response) => {
        this.products = response.data;
        this.totalRecords = response.meta.TotalCount;
        this.loading = false;
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Falha ao carregar produtos' });
        this.loading = false;
      }
    });
  }

  onStatusChange(product: Product) {
    this.productService.toggleStatus(product.id!, product.active!).subscribe({
      next: () => {
        this.messageService.add({ 
            severity: 'success', 
            summary: 'Sucesso', 
            detail: `Produto ${product.active ? 'Ativado' : 'Desativado'}!` 
        });
      },
      error: () => {
        // Reverte visualmente se der erro no back
        product.active = !product.active; 
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Não foi possível alterar o status' });
      }
    });
  }

  openPromoDialog(product: Product) {
      this.newPromo = this.getEmptyPromo();
      this.newPromo.productId = product.id!;
      this.submittedPromo = false;
      this.promoDialog = true;
  }

  savePromotion() {
      this.submittedPromo = true;

      if (this.newPromo.promotionalPrice && this.newPromo.initialDate && this.newPromo.finalDate) {
          // Validação básica de data
          if(this.newPromo.initialDate >= this.newPromo.finalDate) {
              this.messageService.add({ severity: 'warn', summary: 'Atenção', detail: 'Data final deve ser maior que a inicial.' });
              return;
          }

          this.productService.createPromotion(this.newPromo).subscribe({
              next: () => {
                  this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Promoção Criada!' });
                  this.promoDialog = false;
              },
              error: () => this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Erro ao criar promoção.' })
          });
      }
  }

  openNew() {
    this.product = this.getEmptyProduct();
    this.selectedFile = undefined;
    this.submitted = false;
    this.productDialog = true;
    this.imagePreview = null;
  }

  editProduct(product: Product) {
    this.imagePreview = null;
    this.product = { ...product };
    this.productDialog = true;
  }

  deleteProduct(product: Product) {
    this.confirmationService.confirm({
      message: 'Tem certeza que deseja excluir ' + product.name + '?',
      header: 'Confirmar Exclusão',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.productService.delete(product.id!).subscribe({
          next: () => {
            this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Produto excluído' });
            this.loadProducts({ first: 0, rows: 10 });
          },
          error: () => {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Não foi possível excluir' });
          }
        });
      }
    });
  }

  saveProduct() {
    this.submitted = true;

    if (this.product.name?.trim() && this.product.price && this.product.description && this.product.unitOfMeasure) {

      if (this.product.id) {
        this.productService.update(this.product.id, this.product, this.selectedFile).subscribe({
          next: () => {
            this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Produto Atualizado' });
            this.hideDialog();
            this.loadProducts({ first: 0, rows: 10 });
          },
          error: () => this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Erro ao atualizar' })
        });
      } else {
        this.productService.create(this.product, this.selectedFile).subscribe({
          next: () => {
            this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Produto Criado' });
            this.hideDialog();
            this.loadProducts({ first: 0, rows: 10 });
          },
          error: () => this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Erro ao criar' })
        });
      }
    }
  }


  hideDialog() {
    this.productDialog = false;
    this.submitted = false;
  }

  getEmptyProduct(): Product {
    return {
      id: 0,
      name: '',
      price: 0,
      description: '',
      unitOfMeasure: '',
      imageUrl: '',
      active: true,
      customizable: false
    };
  }

  getEmptyPromo(): Promotion {
      return {
          productId: 0,
          promotionalPrice: 0,
          initialDate: new Date(),
          finalDate: new Date()
      };
  }
}