export class OrderItemFilter {
  static readonly MIN_LENGTH_FILTER: number = 18;

  candleId: number;
  numberOfLayerId: number;
  layerColorIds: number[];
  wickId: number;
  quantity: number;
  decorId?: number;
  smellId?: number;
  filter?: string;

  constructor(
    candleId: number,
    numberOfLayerId: number,
    layerColorIds: number[],
    wickId: number,
    quantity: number,
    decorId?: number,
    smellId?: number,
  ) {
    this.candleId = candleId;
    this.numberOfLayerId = numberOfLayerId;
    this.layerColorIds = layerColorIds;
    this.smellId = smellId;
    this.quantity = quantity;
    this.decorId = decorId;
    this.wickId = wickId;
  }

  getFilter(): string {
    if (!this.filter) {
      this.filter = OrderItemFilter.parseToFilter(this);
    }
    return this.filter;
  }

  static parseToFilter(orderItemFilter: OrderItemFilter): string {
    let filter = `c-${orderItemFilter.candleId}`;
    filter += `~n-${orderItemFilter.numberOfLayerId}`;
    if (orderItemFilter.layerColorIds) {
      const layerColorsString = orderItemFilter.layerColorIds.join('_');
      filter += `~l-${layerColorsString}`;
    }
    if (orderItemFilter.decorId) {
      filter += `~d-${orderItemFilter.decorId}`;
    }
    if (orderItemFilter.smellId) {
      filter += `~s-${orderItemFilter.smellId}`;
    }
    if (orderItemFilter.wickId) {
      filter += `~w-${orderItemFilter.wickId}`;
    }
    filter += `~q-${orderItemFilter.quantity}`;

    return filter;
  }

  static parseToOrderItemFilter(filter: string): OrderItemFilter {
    const parts = filter.split('~');
    let candleId = 0;
    let numberOfLayerId = 0;
    let layerColorIds: number[] = [];
    let wickId = 0;
    let quantity = 0;
    let decorId: number | undefined;
    let smellId: number | undefined;

    parts.forEach((part) => {
      const [key, value] = part.split('-');
      switch (key) {
        case 'c':
          candleId = parseInt(value);
          break;
        case 'n':
          numberOfLayerId = parseInt(value);
          break;
        case 'l':
          layerColorIds = value.split('_').map(Number);
          break;
        case 'w':
          wickId = parseInt(value);
          break;
        case 'q':
          quantity = parseInt(value);
          break;
        case 'd':
          decorId = parseInt(value);
          break;
        case 's':
          smellId = parseInt(value);
          break;
      }
    });

    return new OrderItemFilter(
      candleId,
      numberOfLayerId,
      layerColorIds,
      wickId,
      quantity,
      decorId,
      smellId,
    );
  }
}

export function tryParseToOrderItemFilter(filter: string): OrderItemFilter | string[] {
  //Проверка

  const orderItemFilter: OrderItemFilter = OrderItemFilter.parseToOrderItemFilter(filter);

  return orderItemFilter;
}

export function validateOrderItemFilter(orderItemFilter: OrderItemFilter): string[] | undefined {
  let nonconformityMessages: string[] = [];

  const orderItemFilterForComparison = OrderItemFilter.parseToOrderItemFilter(
    orderItemFilter.getFilter(),
  );

  if (orderItemFilter.candleId !== orderItemFilterForComparison.candleId) {
    nonconformityMessages.push('Невалидное значение у candleId');
  }
  if (orderItemFilter.numberOfLayerId !== orderItemFilterForComparison.numberOfLayerId) {
    nonconformityMessages.push('Невалидное значение у numberOfLayerId');
  }
  if (orderItemFilter.layerColorIds !== orderItemFilterForComparison.layerColorIds) {
    nonconformityMessages.push('Невалидное значение у layerColorIds');
  }
  if (orderItemFilter.wickId !== orderItemFilterForComparison.wickId) {
    nonconformityMessages.push('Невалидное значение у wickId');
  }
  if (orderItemFilter.decorId !== orderItemFilterForComparison.decorId) {
    nonconformityMessages.push('Невалидное значение у decorId');
  }
  if (orderItemFilter.smellId !== orderItemFilterForComparison.smellId) {
    nonconformityMessages.push('Невалидное значение у smellId');
  }

  return nonconformityMessages.length < 0 ? nonconformityMessages : undefined;
}
