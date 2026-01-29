export interface CheckoutDTO {
    orderId: number;
    percentToPay: number; 
}

export interface PaymentPreferenceResponse {
    redirectUrl: string; 
    id: string;
}