import { Category } from "./Category";
import { Review } from "./Review";

export interface Product {
    id: number,
    name: string,
    categoryid: number,
    category: Category,
    description: string,
    price: number,
    discount: number,
    specs: string,
    imageUrl: string,
    unitsInStock: number,
    reviews: Review[]
}