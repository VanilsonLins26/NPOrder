export interface PaymentOrderDTO {
    id: number;
    userName: string;
    statusName: string;
    deliverTime: string; 
}

export interface PaymentResponseDTO {
    id: number; 
    status: string;
    orderId: number;
    transactionAmount: number;
    netReceivedAmount: number;
    feeAmount: number;
    installments: number;
    statusDetail: string;
    paymentMethodId: string;
    paymentTypeId: string;
    dateCreated: string;
    dateApproved?: string;
    moneyReleaseDate?: string;
    paymentUrl?: string;
    order: PaymentOrderDTO;
}

export interface PaymentParameters {
    pageNumber: number;
    pageSize: number;
}