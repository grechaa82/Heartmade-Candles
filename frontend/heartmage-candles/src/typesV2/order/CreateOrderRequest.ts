import { Feedback } from './Feedback';

export interface CreateOrderRequest {
  configuredCandlesString: string;
  basketId: string;
  feedback: Feedback;
}
