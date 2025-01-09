import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { Category } from '../../models/category.interface';
import { CategoryService } from '../../services/category.service';
import { CommonModule } from '@angular/common';
import * as bootstrap from 'bootstrap';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.css'
})
export class CategoryListComponent implements OnInit {
  categories: Category[] = [];
  currentSlide = 0;
  Math = Math;
  private categoryService = inject(CategoryService);

  ngOnInit(): void {
    this.categoryService.getAllCategories().subscribe({
      next: (categories) => {
        this.categories = categories;
      },
      error: (error) => {
        console.error('Error fetching categories:', error);
      }
    });
  }

  prevSlide(): void {
    this.currentSlide = this.currentSlide === 0 ? 
      Math.ceil(this.categories.length / 4) - 1 : 
      this.currentSlide - 1;
  }

  nextSlide(): void {
    this.currentSlide = this.currentSlide === Math.ceil(this.categories.length / 4) - 1 ? 
      0 : 
      this.currentSlide + 1;
  }


  getSlideIndexes(): number[] {
    return Array(Math.ceil(this.categories.length / 4)).fill(0).map((_, i) => i);
  }

  getCategoriesForSlide(slideIndex: number): any[] {
    const start = slideIndex * 4;
    const end = start + 4;
    return this.categories.slice(start, end);
  }
}
