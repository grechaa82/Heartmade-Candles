import { Basket } from '../order/Basket';
import { Feedback } from '../order/Feedback';
import { OrderStatus } from '../order/OrderStatus';

export interface Order {
  id?: string;
  basketId: string;
  basket?: Basket;
  feedback: Feedback;
  status?: OrderStatus;
  createdAt: Date;
  updatedAt: Date;
}
