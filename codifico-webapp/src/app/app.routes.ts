import { Routes } from '@angular/router';
import { OrdersComponent } from './orders/orders.component';
import { NewOrderComponent } from './new-order/new-order.component';
import { SalesPredictionComponent } from './sales-prediction/sales-prediction.component';

export const routes: Routes = [
    { path: '', component: SalesPredictionComponent },
  { path: 'orders', component: OrdersComponent },
  { path: 'new-order', component: NewOrderComponent },
];
