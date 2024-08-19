import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Order {
  orderId: number;
  orderDate: string;
  shipName: string;
  shipCity: string;
  freight: number;
}

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
  orders: Order[] = []; 
  paginatedOrders: Order[] = []; 
  currentPage = 1;
  pageSize = 10;
  totalPages = 1;
  sortDirection = 'asc';
  sortBy = 'orderId';
  customerName = 'Customer Name';

  ngOnInit() {
    this.loadOrders();
  }

  async loadOrders() {
    try {
      const response = await fetch('http://localhost:5011/api/orders');
      if (!response.ok) {
        throw new Error('Failed to fetch orders');
      }
      this.orders = await response.json() as Order[];
      this.totalPages = Math.ceil(this.orders.length / this.pageSize);
      this.paginate();
    } catch (error) {
      console.error('Error fetching orders:', error);
    }
  }

  paginate() {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.paginatedOrders = this.orders.slice(start, end);
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.paginate();
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.paginate();
    }
  }

  sortData(column: string) {
    this.sortBy = column;
    this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    this.orders.sort((a, b) => {
      const isAsc = this.sortDirection === 'asc';
      return this.compare(a[column as keyof Order], b[column as keyof Order], isAsc);
    });
    this.paginate();
  }

  compare(a: number | string, b: number | string, isAsc: boolean) {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }
}
