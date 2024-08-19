import { Component, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  imports: [RouterOutlet]
})
export class AppComponent {
  private http = inject(HttpClient);

  constructor() {
    this.http.get('/api/employees/Test', { responseType: 'text' }).subscribe({
      next: data => console.log('Raw Data received:', data),
      error: err => console.error('Error:', err),
      complete: () => console.log('Request completed')
    });      
  }
}
