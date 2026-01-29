import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CheckoutDTO, PaymentPreferenceResponse } from '../models/checkout.model';
import { Observable } from 'rxjs';
import { PaymentParameters, PaymentResponseDTO } from '../models/Payment.model';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private http = inject(HttpClient);
  private apiUrl = 'https://backend-api-tk7o.onrender.com/api/Payment'; 

  createPreference(data: CheckoutDTO): Observable<PaymentPreferenceResponse> {
    return this.http.post<PaymentPreferenceResponse>(this.apiUrl, data);
  }

  getPayments(pageNumber: number, pageSize: number, status?: string, filterDates?: Date[] , searchText?: string  ): Observable<HttpResponse<PaymentResponseDTO[]>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      if (status) {
        params = params.set('status', status);
    };
    if (searchText) {
    params = params.set('FilterText', searchText);
  }

  if (filterDates && filterDates.length > 0) {

    if (filterDates[0]) {
      params = params.set('InitialDate', filterDates[0].toISOString());
    }


    if (filterDates[1]) {
      params = params.set('EndDate', filterDates[1].toISOString());
    }
  }

    return this.http.get<PaymentResponseDTO[]>(this.apiUrl, { 
      params: params, 
      observe: 'response'
    });
  }


  getPaymentById(id: number): Observable<PaymentResponseDTO> {
    return this.http.get<PaymentResponseDTO>(`${this.apiUrl}/${id}`);
  }
}