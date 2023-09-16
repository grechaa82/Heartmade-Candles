import { CandleDetail } from './CandleDetail';

export interface OrderItem {
  candleDetail: CandleDetail;
  quantity: number;
  price: number;
}
