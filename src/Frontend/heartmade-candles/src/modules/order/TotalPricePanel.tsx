import { FC } from 'react';

import Style from './TotalPricePanel.module.css';

export interface TotalPricePanelProps {
  totalPrice: number;
  totalQuantityProduct: number;
}

const TotalPricePanel: FC<TotalPricePanelProps> = ({ totalPrice, totalQuantityProduct }) => {
  return (
    <div className={Style.panel}>
      <div className={Style.totalPriceBlock}>
        <p>Итого</p>
        <p className={Style.totalPrice}>
          {totalPrice.toLocaleString('ru-RU', { useGrouping: true })}
        </p>
      </div>
      <div className={Style.totalQuantityBlock}>{totalQuantityProduct} свечей</div>
      <button className={Style.createOrder}>Оформить заказ</button>
    </div>
  );
};

export default TotalPricePanel;
