export enum OrderStatus {
  Created,
  Confirmed,
  Placed,
  Paid,
  InProgress,
  Packed,
  InDelivery,
  Completed,
  Cancelled,
}

export function getStatusString(status: number): string {
  switch (status) {
    case OrderStatus.Created:
      return 'Создан';
    case OrderStatus.Confirmed:
      return 'Подтвержден';
    case OrderStatus.Placed:
      return 'Размещен';
    case OrderStatus.Paid:
      return 'Оплачен';
    case OrderStatus.InProgress:
      return 'В процессе';
    case OrderStatus.Packed:
      return 'Упакован';
    case OrderStatus.InDelivery:
      return 'Доставляется';
    case OrderStatus.Completed:
      return 'Завершен';
    case OrderStatus.Cancelled:
      return 'Отменен';
    default:
      return 'Неизвестный статус';
  }
}
