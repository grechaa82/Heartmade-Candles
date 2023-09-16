import { Feedback } from './Feedback';
import { User } from './User';
import { OrderItemFilter } from '../OrderItemFilter';

export interface CreateOrderRequest {
  configuredCandlesString: string;
  orderItemFilters: OrderItemFilter[];
  user: User;
  feedback: Feedback;
}
