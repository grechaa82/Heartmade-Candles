import { Feedback } from './Feedback';
import { OrderItemFilterRequest } from './OrderItemFilterRequest';
import { User } from './User';

export interface CreateOrderRequest {
  configuredCandlesString: string;
  orderItemFilters: OrderItemFilterRequest[];
  user: User;
  feedback: Feedback;
}
