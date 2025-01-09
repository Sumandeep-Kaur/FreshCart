import { Review } from "./review.interface";

export interface Product {
    id: number;
    name: string;
    description: string;
    price: number;
    stock: number;
    averageRating: number;
    discountPercentage?: number;
    categoryId: number;
    categoryName: string;
    imageUrl: string;
    reviews: Review[];
}