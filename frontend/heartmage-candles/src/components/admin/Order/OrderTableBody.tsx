import { FC } from 'react';

import { Order } from '../../../typesV2/shared/Order';
import {
  getStatusString,
  getStatusStringRus,
} from '../../../typesV2/order/OrderStatus';

import Style from './OrderTableBody.module.css';

interface OrderTableBodyProps {
  orders: Order[];
  formatDate?: (date: Date) => string;
}

const OrderTableBody: FC<OrderTableBodyProps> = ({ orders, formatDate }) => {
  const handleFormatDate = (date: Date) => {
    const day = date.getDate();
    const month = date.toLocaleString('default', { month: 'short' });
    return `${day} ${month}`;
  };
  return (
    <tbody>
      {orders.map((order) => (
        <tr className={Style.tr} key={order.id}>
          <td>{order.id.substring(order.id.length - 6)}</td>
          <td>
            {(formatDate ? formatDate : handleFormatDate)(
              new Date(order.createdAt)
            )}
          </td>
          <td>
            {(formatDate ? formatDate : handleFormatDate)(
              new Date(order.createdAt)
            )}
          </td>
          <td>{order.basket.filterString.substring(0, 10)}..</td>
          <td className={Style[getStatusString(order.status)]}>
            {getStatusStringRus(order.status)}
          </td>
          <td>{order.basket.items.length}</td>
          <td>{order.basket.totalQuantity}</td>
          <td>{order.basket.totalPrice}</td>
        </tr>
      ))}
    </tbody>
  );
};

export default OrderTableBody;
