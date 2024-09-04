import { FC } from 'react';

import PopUp, { PopUpProps } from '../../../components/shared/PopUp/PopUp';
import Button from '../../../components/shared/Button';

import Style from './LoadCandlePopUp.module.css';

export interface LoadCandlePopUpProps extends PopUpProps {
  loadCandles: () => void;
}

const LoadCandlePopUp: FC<LoadCandlePopUpProps> = ({
  onClose,
  loadCandles,
}) => {
  const handleLoadCandles = () => {
    loadCandles();
    onClose();
  };

  return (
    <PopUp onClose={onClose}>
      <div className={Style.container}>
        <p className={Style.title}>
          Мы обнаружили, что у вас есть свечи.
          <br />
          Хотите попытаться их загрузить?
        </p>
        <div className={Style.buttonBlock}>
          <Button
            text="Отменить"
            onClick={onClose}
            className={Style.cancelBtn}
          ></Button>
          <Button
            text="Загрузить"
            onClick={handleLoadCandles}
            className={Style.confirmationBtn}
          ></Button>
        </div>
      </div>
    </PopUp>
  );
};

export default LoadCandlePopUp;
