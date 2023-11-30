import { FC } from 'react';

import Style from './TotalPricePanel.module.css';

export interface TotalPricePanelProps {
  totalPrice: number;
  totalQuantityProduct: number;
  onCreateOrder: () => void;
}

const TotalPricePanel: FC<TotalPricePanelProps> = ({
  totalPrice,
  totalQuantityProduct,
  onCreateOrder,
}) => {
  return (
    <div className={Style.panel}>
      <div className={Style.infoBlock}>
        <div className={Style.totalPriceBlock}>
          <p>Итого</p>
          <p className={Style.totalPrice}>
            {totalPrice.toLocaleString('ru-RU', { useGrouping: true })} p
          </p>
        </div>
        <div className={Style.totalQuantityBlock}>
          Свечей {totalQuantityProduct}
        </div>
      </div>
      <button className={Style.createOrder} onClick={onCreateOrder}>
        Оформить заказ
      </button>
    </div>
  );
};

export default TotalPricePanel;
