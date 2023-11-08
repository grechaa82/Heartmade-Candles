import { FC } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

import Button from '../../components/shared/Button';

import Style from './ThankPage.module.css';

type OrderParams = {
  id: string;
};

const ThankPage: FC = () => {
  const { id } = useParams<OrderParams>();
  const navigate = useNavigate();

  const handleButtonClick = () => {
    navigate('/constructor');
  };

  return (
    <div className={Style.thankBlock}>
      <h3>Спасибо за оформление заказа!</h3>
      <p>Скоро мы свяжемся для уточнений по-вашему заказу: {id}</p>
      <div className={Style.thankBtn}>
        <Button text="Создать еще свечу" onClick={handleButtonClick} />
      </div>
    </div>
  );
};

export default ThankPage;
