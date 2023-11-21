import { BasketItem } from './BasketItem';

export interface Basket {
  id: string;
  items: BasketItem[];
  totalQuantity: number;
  totalPrice: number;
}
