import { FC } from 'react';
import { useNavigate } from 'react-router-dom';

import Button from '../../components/shared/Button';

import Style from './ThankPage.module.css';

const ThankPage: FC = () => {
  const navigate = useNavigate();

  const handleButtonClick = () => {
    navigate('/constructor');
  };

  return (
    <div className={Style.thankBlock}>
      <h3>
        Спасибо за оформление заказа!
        <br />
        Скоро мы свяжемся для уточнений
      </h3>
      <div className={Style.thankBtn}>
        <Button text="Создать еще свечу" onClick={handleButtonClick} />
      </div>
    </div>
  );
};

export default ThankPage;
