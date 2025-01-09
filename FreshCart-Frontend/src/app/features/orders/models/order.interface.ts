import { Product } from "../../products/models/product.interface";

export interface Order {
    id: number;
    userId: number;
    status: string;
    orderDate: Date;
    items: OrderItem[];
    totalAmount: number;
}

export interface OrderItem {
    id: number;
    productId: number;
    product: Product;
    quantity: number;
    discountPercentage: number;
    unitPrice: number;
    totalPrice: number;
}