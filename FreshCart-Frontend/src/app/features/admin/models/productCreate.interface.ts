export interface ProductCreate {
    name: string;
    description: string;
    price: number;
    stock: number;
    discountPercentage?: number;
    categoryId: number;
    image: File;
}