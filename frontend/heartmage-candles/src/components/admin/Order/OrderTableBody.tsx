import { FC, useState, useEffect, useRef } from 'react';

import { Order } from '../../../typesV2/shared/Order';
import {
  getStatusString,
  getStatusStringRus,
  OrderStatus,
} from '../../../typesV2/order/OrderStatus';
import IconMagnifyingGlassLarge from '../../../UI/IconMagnifyingGlassLarge';
import IconFadersHorizontalLarge from '../../../UI/IconFadersHorizontalLarge';
import ContextMenu, { Action } from '../ContextMenu';
import OrderPreviewReportPopUp from '../../../modules/admin/PopUp/Order/OrderPreviewReportPopUp';

import Style from './OrderTableBody.module.css';

interface OrderTableBodyProps {
  orders: Order[];
  formatDate?: (date: Date) => string;
}

const OrderTableBody: FC<OrderTableBodyProps> = ({ orders, formatDate }) => {
  const [openMenuIndex, setOpenMenuIndex] = useState<number | null>(null);
  const ref = useRef<HTMLDivElement>(null);
  const [openOrderPopup, setOpenOrderPopup] = useState<boolean>(false);
  const [selectedOrder, setSelectedOrder] = useState<Order | null>(null);

  const handleOpenOrderPopup = (order: Order) => {
    setSelectedOrder(order);
    setOpenOrderPopup(true);
  };

  const handleCloseOrderPopup = () => {
    setOpenOrderPopup(false);
  };

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (ref.current && !ref.current.contains(event.target as Node)) {
        setOpenMenuIndex(null);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [ref]);

  const getActions = (): Action[] => {
    const actions: Action[] = [];

    for (let statusKey in OrderStatus) {
      const status = OrderStatus[statusKey];
      if (!isNaN(Number(status))) {
        actions.push({
          label: getStatusStringRus(status),
          onClick: () => console.log('Action for ' + status),
        });
      }
    }
    return actions;
  };

  const handleFormatDate = (date: Date) => {
    const day = date.getDate();
    const month = date.toLocaleString('default', { month: 'short' });
    return `${day} ${month}`;
  };

  const toggleMenu = (index: number) => {
    setOpenMenuIndex(openMenuIndex === index ? null : index);
  };

  return (
    <tbody>
      {orders.map((order, index) => (
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
          <td>
            <button
              className={Style.contextMenuBtn}
              onClick={() => handleOpenOrderPopup(order)}
            >
              <IconMagnifyingGlassLarge />
            </button>
            {openOrderPopup && selectedOrder && (
              <OrderPreviewReportPopUp
                onClose={handleCloseOrderPopup}
                title="Просмотр заказа"
                order={selectedOrder}
              />
            )}
          </td>
          <td>
            <div className={Style.contextMenu} ref={ref}>
              <button
                onClick={() => toggleMenu(index)}
                className={Style.contextMenuBtn}
              >
                <IconFadersHorizontalLarge />
              </button>
              {openMenuIndex === index && (
                <ContextMenu
                  header="Новый статус"
                  className={Style.noBotton}
                  actions={getActions()}
                />
              )}
            </div>
          </td>
        </tr>
      ))}
    </tbody>
  );
};

export default OrderTableBody;
