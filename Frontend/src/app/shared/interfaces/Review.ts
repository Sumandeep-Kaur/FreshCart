export interface Review {
    id: number,
    productId: number,
    rating: number,
    title: string,
    description: string,
    userId: string,
    userName: string,
    reviewDate: Date
}