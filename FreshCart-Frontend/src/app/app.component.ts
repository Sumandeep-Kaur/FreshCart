import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { FooterComponent } from './shared/components/footer/footer.component';
import Aos from 'aos';
import { LoaderComponent } from "./shared/components/loader/loader.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, FooterComponent, LoaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'FreshCart';
  constructor(private router: Router) {}

  ngOnInit() {
    Aos.init({
      duration: 1000,
      easing: 'ease-in-out',
      once: true,
      mirror: false
    });

    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        Aos.refresh(); 
        window.scrollTo(0, 0);
      }
    });
  }
}
