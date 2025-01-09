import { Product } from "../../products/models/product.interface";

export interface Cart {
    id: number;
    userId: number;
    items: CartItem[];
    totalAmount: number;
}

export interface CartItem {
    id: number;
    productId: number;
    product: Product;
    quantity: number;
}