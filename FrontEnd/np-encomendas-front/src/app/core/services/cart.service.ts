import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { AddProductToCartDTO, CartResponse } from '../models/cart.model';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private http = inject(HttpClient);

  private apiUrl = 'https://backend-api-tk7o.onrender.com/api/cart'; 
  cartCount$ = new BehaviorSubject<number>(0);


  getCart(): Observable<CartResponse> {
    return this.http.get<CartResponse>(this.apiUrl).pipe(
      tap(res => this.updateCartCount(res))
    );
  }


  addToCart(data: AddProductToCartDTO): Observable<CartResponse> {
    return this.http.post<CartResponse>(this.apiUrl, data).pipe(
      tap(res => this.updateCartCount(res))
    );
  }


  removeItem(productId: number): Observable<CartResponse> {
    return this.http.delete<CartResponse>(`${this.apiUrl}/${productId}`).pipe(
      tap(res => this.updateCartCount(res))
    );
  }


  clearCart(): Observable<CartResponse> {
    return this.http.delete<CartResponse>(this.apiUrl).pipe(
      tap(res => this.updateCartCount(res))
    );
  }

  updateQuantity(cartItemId: number, quantity: number): Observable<CartResponse> {
    const payload = { cartItemId, newQuantity: quantity };
    return this.http.put<CartResponse>(this.apiUrl, payload).pipe(
      tap(res => this.updateCartCount(res))
    );
  }

  private updateCartCount(response: CartResponse | null) {
    const listaItens = response?.cartItems || [];

    if (listaItens && Array.isArray(listaItens)) {
      const total = listaItens.reduce((acc, item) => acc + item.quantity, 0);
      this.cartCount$.next(total);
    } else {
      this.cartCount$.next(0);
    }
  }
}