import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-new-order',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './new-order.component.html',
  styleUrls: ['./new-order.component.css']
})
export class NewOrderComponent implements OnInit {
  order: any = {
    employee: '',
    shipper: '',
    shipName: '',
    shipAddress: '',
    shipCity: '',
    shipCountry: '',
    orderDate: '',
    requiredDate: '',
    shippedDate: '',
    freight: 0,
    product: '',
    unitPrice: 0,
    quantity: 0,
    discount: 0
  };

  employees: any[] = [];
  shippers: any[] = [];
  products: any[] = [];
  
  isSaving: boolean = false;

  constructor(private router: Router, private http: HttpClient) {}

  ngOnInit(): void {
    this.loadEmployees();
    this.loadShippers();
    this.loadProducts();
  }

  loadEmployees(): void {
    this.http.get('/api/employees').subscribe({
      next: (data: any) => {
        this.employees = data;
      },
      error: (error) => {
        console.error('Error loading employees:', error);
      }
    });
  }

  loadShippers(): void {
    this.http.get('/api/shippers').subscribe({
      next: (data: any) => {
        this.shippers = data;
      },
      error: (error) => {
        console.error('Error loading shippers:', error);
      }
    });
  }

  loadProducts(): void {
    this.http.get('/api/products').subscribe({
      next: (data: any) => {
        this.products = data;
      },
      error: (error) => {
        console.error('Error loading products:', error);
      }
    });
  }

  onSave() {
    this.isSaving = true;
    this.http.post('/api/orders', this.order).subscribe({
      next: () => {
        this.isSaving = false;
        alert('Order saved successfully!');
        this.router.navigate(['/home']);
      },
      error: (error) => {
        this.isSaving = false;
        alert('There was an error saving the order. Please try again.');
        console.error('Error saving order:', error);
      }
    });
  }

  onClose() {
    this.router.navigate(['/home']);
  }
}
