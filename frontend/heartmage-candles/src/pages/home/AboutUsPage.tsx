import { FC } from 'react';
import { useNavigate } from 'react-router-dom';

import Button from '../../components/shared/Button';

import Style from './AboutUsPage.module.css';

const AboutUs: FC = () => {
  const navigate = useNavigate();

  const handleButtonClick = () => {
    navigate('/constructor');
  };

  return (
    <div className={Style.block}>
      <h3>
        В данный момент эта страница в разработке
        <br />
        Приносим свои извинения
      </h3>
      <p>Вы можете посетить конструктор и заказать свечи</p>
      <div className={Style.btn}>
        <Button text="Попробовать конструктор" onClick={handleButtonClick} color="#2e67ea" />
      </div>
    </div>
  );
};

export default AboutUs;
