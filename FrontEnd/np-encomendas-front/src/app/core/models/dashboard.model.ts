export interface DashboardStats {
    totalRevenue: number;
    totalOrders: number;
    pendingOrders: number;
    totalProducts: number;
    monthlyRevenue: number[]; 
    monthlyLabels: string[];  
    orderStatusCounts: number[]; 
}