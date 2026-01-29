import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { PaginationMetadata, Product, Promotion } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private http = inject(HttpClient);

  private apiUrl = 'https://backend-api-tk7o.onrender.com/api/product';

 getProducts(page: number, size: number, name?: string, minPrice?: number, maxPrice?: number) {
    let params = new HttpParams()
        .set('PageNumber', page.toString())
        .set('PageSize', size.toString());

    if (name) params = params.set('Name', name);
    if (minPrice) params = params.set('MinPrice', minPrice.toString());
    if (maxPrice) params = params.set('MaxPrice', maxPrice.toString());

    return this.http.get<Product[]>(`${this.apiUrl}/paged`, {
      params: params,
      observe: 'response'
    }).pipe(
      map((response: HttpResponse<Product[]>) => {
        const data = response.body || [];

        const paginationHeader = response.headers.get('X-Pagination');
        const meta = paginationHeader ? JSON.parse(paginationHeader) : { totalCount: 0 };

        return { data, meta };
      })
    );
  }

  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`);
  }

  create(product: Product, file?: File): Observable<Product> {
    const formData = this.toFormData(product, file);
    return this.http.post<Product>(this.apiUrl, formData);
  }

  update(id: number, product: Product, file?: File): Observable<Product> {
    const formData = this.toFormData(product, file);
    return this.http.put<Product>(`${this.apiUrl}/${id}`, formData);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  toggleStatus(id: number, active: boolean): Observable<boolean> {

    return this.http.patch<boolean>(`${this.apiUrl}/active/${id}`, active);
  }

  createPromotion(promotion: Promotion): Observable<any> {
    return this.http.post(`${this.apiUrl}/promotion`, promotion);
  }

  private toFormData(product: Product, file?: File): FormData {
    const formData = new FormData();

    formData.append('name', product.name);
    formData.append('price', product.price.toString().replace('.', ','));
    formData.append('unitOfMeasure', product.unitOfMeasure);
    formData.append('description', product.description || '');
    formData.append('customizable', (product.customizable || false).toString());



    if (file) {
      formData.append('imageFile', file);
    } else if (product.imageUrl) {
      formData.append('imageUrl', product.imageUrl);
    }

    return formData;
  }
}
