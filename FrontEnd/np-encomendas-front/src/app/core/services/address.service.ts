import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddressRequest, AddressResponse } from '../models/address.model';

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  private http = inject(HttpClient);
  private apiUrl = 'https://backend-api-tk7o.onrender.com/api/Address'; 

  getAllAddresses(): Observable<AddressResponse[]> {
    return this.http.get<AddressResponse[]>(this.apiUrl);
  }

  createAddress(data: AddressRequest): Observable<AddressResponse> {
    return this.http.post<AddressResponse>(this.apiUrl, data);
  }
}
