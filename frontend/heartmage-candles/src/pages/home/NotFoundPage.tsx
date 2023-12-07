import { FC } from 'react';
import { useNavigate } from 'react-router-dom';

import Button from '../../components/shared/Button';

import Style from './NotFoundPage.module.css';

const NotFoundPage: FC = () => {
  const navigate = useNavigate();

  const handleButtonClick = () => {
    navigate('/');
  };

  return (
    <div className={Style.block}>
      <h1>404</h1>
      <p>Мы не смогли найти</p>
      <div className={Style.btn}>
        <Button text="Главная" onClick={handleButtonClick} color="#2e67ea" />
      </div>
    </div>
  );
};

export default NotFoundPage;
