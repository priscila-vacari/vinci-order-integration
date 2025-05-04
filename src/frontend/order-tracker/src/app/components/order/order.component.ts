import { Component } from '@angular/core';
import { Order, OrderService } from '../../services/order.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss'],
  standalone: true,
  imports: [FormsModule, CommonModule, HttpClientModule]  
})
export class OrderComponent {
  order: Order = { id: 0, cliente: '', valor: 0, dataPedido: '' };
  retrievedOrder?: Order;
  orderId: number = 0;

  constructor(private orderService: OrderService) {}

  createOrder() {
    this.orderService.create(this.order).subscribe({
      next: () => {
        alert('✅ Pedido enviado com sucesso!');
      },
      error: (err) => {
        switch (err.status) {
          case 400:
            alert('⚠️ Requisição inválida. Verifique os dados informados.');
            break;
            break;
          case 409:
            alert('⚠️ Pedido já existe ou conflito de dados.');
            break;
          default:
            alert('❌ Erro inesperado. Tente novamente mais tarde.');
            console.error('Erro:', err);
        }
      }
    });
  }
  
  fetchOrder() {
    this.orderService.getById(this.orderId).subscribe({
      next: o => (this.retrievedOrder = o),
      error: () => alert('Pedido não encontrado.')
    });
  }
}
