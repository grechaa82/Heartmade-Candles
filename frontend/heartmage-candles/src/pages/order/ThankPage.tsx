import { FC } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

import Button from '../../components/shared/Button';

import Style from './ThankPage.module.css';

type OrderParams = {
  id: string;
};

const ThankPage: FC = () => {
  const { id } = useParams<OrderParams>();

  const copyToClipboard = (text: string) => {
    const textField = document.createElement('textarea');
    textField.innerText = text;
    document.body.appendChild(textField);
    textField.select();
    document.execCommand('copy');
    textField.remove();
  };

  const handleButtonClickToBot = () => {
    copyToClipboard(id!);
    window.open('https://t.me/HeartmadeCandlesTest_bot');
  };

  const handleButtonClickToCopyId = () => {
    copyToClipboard(id!);
  };

  return (
    <div className={Style.block}>
      <h3>Спасибо за оформление заказа!</h3>
      <p>
        Сохраните номер вашего заказа, он вам понадобится для подтверждения:{' '}
        <span onClick={handleButtonClickToCopyId}>{id}</span>
      </p>
      <div className={Style.btnBlock}>
        <Button text="Открыть телеграм бота" onClick={handleButtonClickToBot} />
        <Button
          text="Скопировать номер заказа"
          onClick={handleButtonClickToCopyId}
        />
      </div>
    </div>
  );
};

export default ThankPage;
