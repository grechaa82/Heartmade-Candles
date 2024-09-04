import { FC, useState, useEffect } from 'react';

import Style from './TotalPricePanel.module.css';

export interface TotalPricePanelProps {
  totalPrice: number;
  totalQuantityProduct: number;
  onCreateOrder: () => void;
  isValid?: boolean;
}

type ButtonState = 'default' | 'invalid' | 'valid';

const TotalPricePanel: FC<TotalPricePanelProps> = ({
  totalPrice,
  totalQuantityProduct,
  onCreateOrder,
  isValid,
}) => {
  const [buttonState, setButtonState] = useState<ButtonState>('default');

  useEffect(() => {
    const newState =
      isValid === undefined ? 'default' : isValid ? 'valid' : 'invalid';
    setButtonState(newState);
  }, [isValid]);

  return (
    <div className={Style.panel}>
      <div className={Style.infoBlock}>
        <div className={Style.totalPriceBlock}>
          <p>Итого</p>
          <p className={Style.totalPrice}>
            {totalPrice.toLocaleString('ru-RU', { useGrouping: true })} Р
          </p>
        </div>
        <div className={Style.totalQuantityBlock}>
          Свечей {totalQuantityProduct}
        </div>
      </div>
      <button
        className={`${Style.createOrder} ${Style[buttonState]}`}
        onClick={onCreateOrder}
      >
        Оформить заказ
      </button>
    </div>
  );
};

export default TotalPricePanel;
