export interface Product {
    id: number;
    name: string;
    price: number;
    description: string;
    imageUrl: string;
    active: boolean;
    unitOfMeasure: string;
    customizable?: boolean;
    createTime?: Date;
    promotionEndTime?: Date;
    finalPrice?: number; 
    isOnSale?: boolean;
}

export interface ProductRequest {
    name: string;
    price: number;
    description: string;
    imageUrl: string;
    unitOfMeasure: string;
}

export interface PaginationMetadata {
    TotalCount: number;
    pageSize: number;
    currentPage: number;
    totalPages: number;
    hasNext: boolean;
    hasPrevious: boolean;
}

export interface Promotion {
    id?: number;
    productId: number;
    promotionalPrice: number;
    initialDate: Date;
    finalDate: Date;
}