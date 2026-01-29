import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddressResponse } from '../models/address.model';
import { CreateOrderDTO, Order } from '../models/order.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private http = inject(HttpClient);
  
  private apiUrl = 'https://backend-api-tk7o.onrender.com/api'; 

  getUserAddresses(): Observable<AddressResponse[]> {
    return this.http.get<AddressResponse[]>(`${this.apiUrl}/address`);
  }

  getMyOrders(pageNumber: number = 1, pageSize: number = 10) {
  let params = new HttpParams()
    .set('pageNumber', pageNumber.toString())
    .set('pageSize', pageSize.toString())


  return this.http.get<any[]>(`${this.apiUrl}/order/client/paged`, { 
    params, 
    observe: 'response' 
  });
}

getAllOrdersAdmin(
    pageNumber: number, 
    pageSize: number, 
    viewModel?: number, 
    filterText?: string,
    StatusOptions?: string,
    filterDates?: Date[]
  ): Observable<HttpResponse<any[]>> {

    const params = this.createAdminParams(pageNumber, pageSize, viewModel, filterText, StatusOptions, filterDates);

    return this.http.get<any[]>(`${this.apiUrl}/order/admin/paged`, { 
      params, 
      observe: 'response' 
    });
  }

  createOrder(data: CreateOrderDTO): Observable<Order> {
    return this.http.post<Order>(`${this.apiUrl}/order`, data);
  }

  getOrderById(id: number): Observable<Order> {

    return this.http.get<Order>(`${this.apiUrl}/order/${id}`);
  }

  cancelOrder(id: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/order/${id}/cancel`, {});
  }


  confirmDelivery(id: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/order/${id}/delivered`, {});
  }


  markReadyForPickup(id: number): Observable<Order> {
    return this.http.post<Order>(`${this.apiUrl}/order/${id}/readyforpickup`, {});
  }

  markOutForDelivery(id: number): Observable<Order> {
    return this.http.post<Order>(`${this.apiUrl}/order/${id}/outfordelivery`, {});
  }

  private createAdminParams(
    pageNumber: number, 
    pageSize: number, 
    viewModel?: number,
    filterText?: string,
    StatusOptions?: string,
    filterDates?: Date[]
  ): HttpParams {
    
    let params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString())

      if (viewModel !== undefined && viewModel !== null) {
      params = params.set('ViewModel', viewModel.toString());
    }

    if (filterText) {
      params = params.set('FilterText', filterText);
    }

    if (StatusOptions) {
      params = params.set('StatusOptions', StatusOptions);
    }

    if (filterDates && filterDates.length > 0) {
      if (filterDates[0]) params = params.set('InicialDate', filterDates[0].toISOString());
      if (filterDates[1]) params = params.set('EndDate', filterDates[1].toISOString());
    }

    return params;
  }
  
}