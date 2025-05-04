import { Component } from '@angular/core';
import { OrderComponent } from './components/order/order.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  standalone: true,
  imports: [CommonModule, OrderComponent]
})

export class AppComponent {
  title = 'order-tracker';
}
