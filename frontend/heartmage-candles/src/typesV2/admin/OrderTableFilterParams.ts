import { OrderStatus } from '../order/OrderStatus';
import { TypesOfSorting } from '../order/OrderTableParametersRequest';

export interface OrderTableFilterParams {
  sortBy: TypesOfSorting | null;
  ascending: boolean;
  createdFrom: Date | null;
  createdTo: Date | null;
  status: OrderStatus | null;
  pageSize: number;
  pageIndex: number;
}
