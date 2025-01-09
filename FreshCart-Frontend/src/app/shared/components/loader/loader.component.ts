import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { LoaderService } from '../../../core/services/loader.service';

@Component({
  selector: 'app-loader',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './loader.component.html',
  styleUrl: './loader.component.css'
})
export class LoaderComponent {
  isLoading$: Observable<boolean>;

  constructor(private loaderService: LoaderService) {
    this.isLoading$ = this.loaderService.loading$;
  }
}
