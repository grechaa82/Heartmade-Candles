import { FC, useState } from 'react';

import SelectorWithInput from '../../components/order/SelectorWithInput';
import Input from '../../components/order/Input';
import IconTrashLarge from '../../UI/IconTrashLarge';

import Style from './FormFeedback.module.css';

export interface FormFeedbackProps {}

type FeedbackForm = 'Telegram' | 'Instagram' | 'Whatsapp';

const FormFeedback: FC<FormFeedbackProps> = ({}) => {
  const [selectedForm, setSelectedForm] = useState<string>('');
  const [username, setUsername] = useState<string>('');

  const validateTelegramAndInstagram = (value: string) => {
    const regex = /@[a-zA-Z0-9_]{1,32}$/;
    return regex.test(value);
  };

  const validatePhoneNumber = (value: string) => {
    return /^((8|\+7|7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?\d{3}[\- ]?\d{2}[\- ]?\d{2}?$/.test(value);
  };

  const handleSelectorClick = (form: FeedbackForm) => {
    setUsername('');
    setSelectedForm(form);
  };

  return (
    <div>
      <div className={Style.title}>Личные данные</div>
      <div className={Style.description}>
        Выбирите способ связи и мы свяжемся с вами как можно быстрей
      </div>
      <form action="" className={Style.form}>
        <div className={Style.column}>
          <SelectorWithInput
            title="Telegram"
            icon={IconTrashLarge}
            isSelected={selectedForm === 'Telegram' ? true : false}
            onSelected={() => handleSelectorClick('Telegram')}
            input={
              <Input
                label="Введите ваше имя пользователя"
                required
                value={username}
                onChange={setUsername}
                validation={validateTelegramAndInstagram}
              />
            }
          />
          <SelectorWithInput
            title="Instagram"
            icon={IconTrashLarge}
            isSelected={selectedForm === 'Instagram' ? true : false}
            onSelected={() => handleSelectorClick('Instagram')}
            input={
              <Input
                label="Введите ваше имя пользователя"
                required
                value={username}
                onChange={setUsername}
                validation={validateTelegramAndInstagram}
              />
            }
          />
        </div>
        <div className={Style.column}>
          <SelectorWithInput
            title="Whatsapp"
            icon={IconTrashLarge}
            isSelected={selectedForm === 'Whatsapp' ? true : false}
            onSelected={() => handleSelectorClick('Whatsapp')}
            input={
              <Input
                label="Введите ваше имя пользователя"
                required
                value={username}
                onChange={setUsername}
                validation={validatePhoneNumber}
              />
            }
          />
        </div>
      </form>
    </div>
  );
};

export default FormFeedback;
