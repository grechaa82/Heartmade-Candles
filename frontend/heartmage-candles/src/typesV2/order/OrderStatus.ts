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

export function getStatusStringRus(status: number | string): string {
  const statusNumber = typeof status === 'string' ? parseInt(status) : status;

  switch (statusNumber) {
    case OrderStatus.Created:
      return 'Создан';
    case OrderStatus.Confirmed:
      return 'Подтвержден';
    case OrderStatus.Placed:
      return 'Оформлен';
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

export const getStatusString = (status: number): string => {
  switch (status) {
    case OrderStatus.Created:
      return 'Created';
    case OrderStatus.Confirmed:
      return 'Confirmed';
    case OrderStatus.Placed:
      return 'Placed';
    case OrderStatus.Paid:
      return 'Paid';
    case OrderStatus.InProgress:
      return 'InProgress';
    case OrderStatus.Packed:
      return 'Packed';
    case OrderStatus.InDelivery:
      return 'InDelivery';
    case OrderStatus.Completed:
      return 'Completed';
    case OrderStatus.Cancelled:
      return 'Cancelled';
    default:
      return '';
  }
};
