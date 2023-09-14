import { Product } from "./Product";

export interface Cart {
    id: number,
    product: Product,
    cartId: number,
    productId: number,
    quantity: number
}