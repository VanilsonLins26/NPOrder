import { AddressResponse } from "./address.model";

export enum DeliveryMethod{
    Pickup = 1,
    Delivery = 2
}

export interface Order{
    id: number;
    clientId: string;
    userName: string;
    phone: string;
    addressId?: number;
    address?: AddressResponse;
    deliveryMethod: DeliveryMethod;
    totalAmount: number;
    status: Status;
    deliverTime: Date;
    remainingAmount?: number;
    orderItens: OrderItem[];
    statusName : string;


}

export interface OrderItem {
  id: number;
  productId: number;
  productName: string; 
  productImage?: string; 
  quantity: number;
  unitPrice: number;
  subTotal: number;
  comment?: string; 
}

export interface CreateOrderDTO {
    deliveryMethod: 2 | 1; 
    deliveryTime: Date;
    addressId?: number | null; 
}

export enum Status {
  PendingPayment = 0,         
  Confirmed = 1,       
  ReadyForPickup = 2,     
  OutForDelivery = 3,  
  Delivered = 4,        
  Canceled = 65        
}