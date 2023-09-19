import { Feedback } from './Feedback';
import { User } from './User';

export interface CreateOrderRequest {
  configuredCandlesString: string;
  orderItemFilters: OrderItemFilterRequest[];
  user: User;
  feedback: Feedback;
}

export interface OrderItemFilterRequest {
  candleId: number;
  decorId?: number;
  numberOfLayerId: number;
  layerColorIds: number[];
  smellId?: number;
  wickId: number;
  quantity: number;
}
