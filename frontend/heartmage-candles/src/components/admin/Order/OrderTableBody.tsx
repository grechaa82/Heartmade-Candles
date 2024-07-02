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
  updateOrderStatus: (orderId: string, newStatus: OrderStatus) => void;
}

const OrderTableBody: FC<OrderTableBodyProps> = ({
  orders,
  formatDate,
  updateOrderStatus,
}) => {
  const [openMenuIndex, setOpenMenuIndex] = useState<number | null>(null);
  const menuRefs = useRef<Array<HTMLDivElement | null>>([]);
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
      if (
        menuRefs.current &&
        !menuRefs.current.some((ref) => ref?.contains(event.target as Node))
      ) {
        setOpenMenuIndex(null);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  const getActions = (order: Order): Action[] => {
    const actions: Action[] = [];

    for (const status of Object.values(OrderStatus)) {
      if (typeof status === 'number' && status !== order.status) {
        actions.push({
          label: getStatusStringRus(status as OrderStatus),
          onClick: () => updateOrderStatus(order.id, status as OrderStatus),
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
            <div
              className={Style.contextMenu}
              ref={(el) => (menuRefs.current[index] = el)}
            >
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
                  actions={getActions(order)}
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
