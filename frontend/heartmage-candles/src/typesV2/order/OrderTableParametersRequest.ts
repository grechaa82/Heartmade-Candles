import { OrderStatus } from './OrderStatus';

export type TypesOfSorting =
  | 'status'
  | 'totalprice'
  | 'createdat'
  | 'updatedat';

export class OrderTableParameters {
  sortBy?: TypesOfSorting;
  ascending: boolean = true;
  createdFrom?: Date;
  createdTo?: Date;
  status?: OrderStatus;
  pageSize: number = 10;
  pageIndex: number = 0;

  constructor(data: Partial<OrderTableParameters>) {
    Object.assign(this, data);
  }

  toQueryString(): string {
    const queryParams: string[] = [];

    for (const key in this) {
      if (
        this[key] !== undefined &&
        this[key] !== null &&
        !Number.isNaN(this[key]) &&
        this[key] !== ''
      ) {
        if (this[key] instanceof Date) {
          queryParams.push(
            `${key}=${encodeURIComponent((this[key] as Date).toISOString())}`,
          );
        } else {
          queryParams.push(`${key}=${encodeURIComponent(String(this[key]))}`);
        }
      }
    }

    return queryParams.join('&');
  }
}
