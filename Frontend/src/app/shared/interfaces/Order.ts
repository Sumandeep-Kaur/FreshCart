import { Product } from "./Product";
import { Review } from "./Review";

export interface Order {
    id: number,
    product: Product,
    orderId: number,
    productId: number,
    quantity: number,
    orderDate: Date,
    totalPrice: number
}