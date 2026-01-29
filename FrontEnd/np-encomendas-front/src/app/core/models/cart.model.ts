import { Product } from "./product.model";

export interface AddProductToCartDTO {
    productId: number;
    quantity: number;
    comment?: string;
}


export interface CartItem {
    id: number;
    quantity: number;
    unityPrice: number;
    totalPrice: number;
    productId: number;
    cartHeaderId: number;
    comment?: string;
    product: Product;
}


export interface CartHeader {
    id: number;
    userId: string;
}


export interface CartResponse {
    items: CartResponse;
    cartHeader: CartHeader;
    total: number; 
    cartItems: CartItem[];
}