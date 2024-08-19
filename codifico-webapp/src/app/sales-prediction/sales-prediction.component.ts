import { Component, OnInit } from '@angular/core';
import { SalesService } from '../sales.service'; 
import { Router } from '@angular/router';

@Component({
  selector: 'app-sales-prediction',
  templateUrl: './sales-prediction.component.html',
  styleUrls: ['./sales-prediction.component.css']
})
export class SalesPredictionComponent implements OnInit {
  customers: any[] = []; 
  paginatedCustomers: any[] = [];
  currentPage = 1;
  pageSize = 10;
  totalPages = 1;
  totalItems = 0;
  sortBy = 'customerName';
  sortDirection = 'asc';
  searchTerm = '';

  constructor(private salesService: SalesService, private router: Router) {}

  ngOnInit() {
    this.loadCustomers();
  }

  loadCustomers() {
    this.salesService.getCustomers(this.currentPage, this.pageSize, this.sortBy, this.sortDirection, this.searchTerm)
      .subscribe((data: any) => {
        this.customers = data.customers || [];  // Asegúrate de que no sea null
        this.totalItems = data.totalCount || 0;
        this.totalPages = Math.ceil(this.totalItems / this.pageSize);
        this.paginate();
      });
  }

  paginate() {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.paginatedCustomers = this.customers.slice(start, end);
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadCustomers();
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadCustomers();
    }
  }

  sortData(column: string) {
    this.sortBy = column;
    this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    this.loadCustomers();
  }

  onSearch(event: Event) {
    const inputElement = event.target as HTMLInputElement; // Cast a HTMLInputElement
    this.searchTerm = inputElement?.value || '';
    this.filterData();
  }

  filterData() {
    this.currentPage = 1;
    this.loadCustomers();
  }

  openOrdersModal(customerId: number) {
    this.router.navigate(['/orders'], { queryParams: { customerId } });
  }

  openNewOrderModal(customerId: number) {
    this.router.navigate(['/new-order'], { queryParams: { customerId } });
  }

  onPageSizeChange(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    this.pageSize = Number(selectElement.value);
    this.loadCustomers();
  }

  min(a: number, b: number): number {
    return Math.min(a, b);
  }
}
