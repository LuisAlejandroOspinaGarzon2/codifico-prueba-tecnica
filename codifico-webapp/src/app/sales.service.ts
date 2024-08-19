import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SalesService {
  private apiUrl = 'http://localhost:5011/api/sales-prediction'; 

  constructor(private http: HttpClient) {}

  getCustomers(page: number, pageSize: number, sortBy: string, sortDirection: string, searchTerm: string): Observable<any> {
    const params = {
      page: page.toString(),
      pageSize: pageSize.toString(),
      sortBy,
      sortDirection,
      searchTerm
    };
    return this.http.get<any>(this.apiUrl, { params });
  }
}
