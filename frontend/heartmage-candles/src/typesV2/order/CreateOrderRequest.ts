import { Feedback } from './Feedback';
import { User } from './User';

export interface CreateOrderRequest {
  configuredCandlesString: string;
  basketId: string;
  user: User;
  feedback: Feedback;
}
