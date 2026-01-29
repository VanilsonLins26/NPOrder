export interface AddressResponse {
    id: number;
    userId: string;
    street: string;
    number: string;
    complement?: string;
    district: string;
    zipCode: string;
    isDefault: boolean;
}

export interface AddressRequest {
    street: string;
    number: string;
    complement?: string;
    district: string;
    zipCode: string;
}