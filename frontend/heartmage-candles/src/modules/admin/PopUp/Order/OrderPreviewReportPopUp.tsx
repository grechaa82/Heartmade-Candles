import { FC } from 'react';

import PopUp, { PopUpProps } from '../../../../components/admin/PopUp/PopUp';
import { Order } from '../../../../typesV2/shared/Order';
import ListProductsCart from '../../../order/ListProductsCart';

import Style from './OrderPreviewReportPopUp.module.css';
import {
  getStatusString,
  getStatusStringRus,
} from '../../../../typesV2/order/OrderStatus';

export interface OrderPreviewReportPopUpProps extends PopUpProps {
  title: string;
  order: Order;
}

const OrderPreviewReportPopUp: FC<OrderPreviewReportPopUpProps> = ({
  onClose,
  title,
  order,
}) => {
  const handleFormatDate = (date: Date) => {
    const day = date.getDate();
    const month = date.toLocaleString('default', { month: 'short' });
    return `${day} ${month}`;
  };
  return (
    <PopUp onClose={onClose}>
      <div className={Style.container}>
        <p className={Style.title}>
          {order.id}{' '}
          <span className={Style[getStatusString(order.status)]}>
            {getStatusStringRus(order.status)}
          </span>
        </p>
        <div className={Style.infoBlock}>
          <p className={Style.secondaryInfo}>
            Строка конфигурации <span>{order.basket.filterString}</span>
          </p>
          <p className={Style.secondaryInfo}>
            Создано <span>{handleFormatDate(new Date(order.createdAt))}</span>
          </p>
          <p className={Style.secondaryInfo}>
            Обновлено <span>{handleFormatDate(new Date(order.updatedAt))}</span>
          </p>
          <p className={Style.secondaryInfo}>
            Общее количество <span>{order.basket.totalQuantity}</span>
          </p>
          <p className={`${Style.secondaryInfo} ${Style.priceInfo}`}>
            Сумма <span>{order.basket.totalPrice}</span>
          </p>
        </div>
        <ListProductsCart products={order.basket.items} />
      </div>
    </PopUp>
  );
};

export default OrderPreviewReportPopUp;
