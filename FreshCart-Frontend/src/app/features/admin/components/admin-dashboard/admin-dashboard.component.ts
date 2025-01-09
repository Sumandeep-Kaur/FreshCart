import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Product } from '../../../products/models/product.interface';
import { AdminService } from '../../services/admin.service';
import { ProductService } from '../../../products/services/product.service';
import { ProductCreate } from '../../models/productCreate.interface';
import { lastValueFrom } from 'rxjs';
import { Category } from '../../../categories/models/category.interface';
import { CategoryService } from '../../../categories/services/category.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css'
})
export class AdminDashboardComponent implements OnInit {
  products = signal<Product[]>([]);
  categories = signal<Category[]>([]);
  showModal = signal(false);
  isEditMode = signal(false);
  isSubmitting = signal(false);
  selectedProductId = signal<number | null>(null);
  private adminService = inject(AdminService);
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  
  productForm: FormGroup;
  
  constructor(
    private fb: FormBuilder
  ) {
    this.productForm = this.initializeForm();
  }
  
  ngOnInit(): void {
    this.loadProducts();
    this.loadCategories();
  }
  
  private initializeForm(): FormGroup {
    return this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', Validators.required],
      price: ['', [Validators.required, Validators.min(0.01)]],
      stock: ['', [Validators.required, Validators.min(0)]],
      discountPercentage: [null, [Validators.min(0), Validators.max(100)]],
      categoryId: ['', Validators.required],
      image: [null, Validators.required]
    });
  }

  calculateDiscountedPrice(product: Product): number {
    if (!product.discountPercentage) return product.price;
    const discountedPrice = product.price - (product.price * (product.discountPercentage / 100));
    return Math.round(discountedPrice * 100) / 100;
}
  
  openAddModal(): void {
    this.productForm.reset();
    this.isEditMode.set(false);
    this.showModal.set(true);
  }
  
  openEditModal(product: Product): void {
    this.selectedProductId.set(product.id);
    this.productForm.patchValue({
      name: product.name,
      description: product.description,
      price: product.price,
      stock: product.stock,
      discountPercentage: product.discountPercentage,
      categoryId: product.categoryId
    });
    this.isEditMode.set(true);
    this.showModal.set(true);
  }
  
  closeModal(): void {
    this.showModal.set(false);
    this.productForm.reset();
    this.selectedProductId.set(null);
    this.isEditMode.set(false);
  }
  
  onFileSelect(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      this.productForm.patchValue({ image: file });
      this.productForm.get('image')?.updateValueAndValidity();
    }
  }
  
  isFieldInvalid(fieldName: string): boolean {
    const field = this.productForm.get(fieldName);
    return field ? (field.invalid && (field.dirty || field.touched)) : false;
  }
  
  async saveProduct(): Promise<void> {
    if (this.productForm.valid) {
      try {
        this.isSubmitting.set(true);
        const productData: ProductCreate = {
          name: this.productForm.value.name,
          description: this.productForm.value.description,
          price: this.productForm.value.price,
          stock: this.productForm.value.stock,
          discountPercentage: this.productForm.value.discountPercentage,
          categoryId: this.productForm.value.categoryId,
          image: this.productForm.value.image
        };

        if (this.isEditMode()) {
          const productId = this.selectedProductId();
          if (productId) {
            await lastValueFrom(this.adminService.updateProduct(productId, productData));
          }
        } else {
          await lastValueFrom(this.adminService.createProduct(productData));
        }

        this.closeModal();
        await this.loadProducts();
      } catch (error) {
        console.error('Error saving product:', error);
        // Implement error notification here
      } finally {
        this.isSubmitting.set(false);
      }
    }
  }
  
  async deleteProduct(id: number): Promise<void> {
    if (confirm('Are you sure you want to delete this product?')) {
      try {
        await lastValueFrom(this.adminService.deleteProduct(id));
        await this.loadProducts();
        // Show success notification
      } catch (error) {
        console.error('Error deleting product:', error);
        // Show error notification
      }
    }
  }
  
  private async loadProducts(): Promise<void> {
    try {
      const products = await lastValueFrom(this.productService.getAllProducts());
      this.products.set(products);
    } catch (error) {
      console.error('Error loading products:', error);
      // Show error notification
    }
  }
  
  private async loadCategories(): Promise<void> {
    try {
      const categories = await lastValueFrom(this.categoryService.getAllCategories());
      this.categories.set(categories);
    } catch (error) {
      console.error('Error loading categories:', error);
      // Show error notification
    }
  }
}
