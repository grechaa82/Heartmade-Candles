import { FC } from 'react';

import SelectorWithInput from '../../components/order/SelectorWithInput';
import Input from '../../components/shared/Input';
import IconTrashLarge from '../../UI/IconTrashLarge';
import { feedbackType } from '../../typesV2/order/Feedback';

import Style from './FormFeedback.module.css';

export interface ItemFormFeedback {
  title: feedbackType;
  label: string;
  value: string;
  onChangeSelectedForm: (value: feedbackType) => void;
  onChangeUsername: (value: string) => void;
  isRequired: boolean;
  isSelected: boolean;
  validation: (value: string) => boolean;
}

export interface FormFeedbackProps {
  itemsFormFeedbacks: ItemFormFeedback[];
}

const FormFeedback: FC<FormFeedbackProps> = ({ itemsFormFeedbacks }) => {
  const evenItems = itemsFormFeedbacks.filter((_, index) => index % 2 === 0);
  const oddItems = itemsFormFeedbacks.filter((_, index) => index % 2 !== 0);

  return (
    <div>
      <div className={Style.title}>Личные данные</div>
      <div className={Style.description}>
        Выбирите способ связи и мы свяжемся с вами как можно быстрей
      </div>
      <form action="" className={Style.form}>
        <div className={Style.column}>
          {evenItems.map((item, index) => (
            <SelectorWithInput
              title={item.title.toString()}
              icon={IconTrashLarge}
              isSelected={item.isSelected}
              onSelected={() => item.onChangeSelectedForm(item.title)}
              input={
                <Input
                  label="Введите ваше имя пользователя"
                  required
                  value={item.value}
                  onChange={item.onChangeUsername}
                  validation={item.validation}
                />
              }
              key={index}
            />
          ))}
        </div>
        <div className={Style.column}>
          {oddItems.map((item, index) => (
            <SelectorWithInput
              title={item.title.toString()}
              icon={IconTrashLarge}
              isSelected={item.isSelected}
              onSelected={() => item.onChangeSelectedForm(item.title)}
              input={
                <Input
                  label="Введите ваше имя пользователя"
                  required
                  value={item.value}
                  onChange={item.onChangeUsername}
                  validation={item.validation}
                />
              }
              key={index}
            />
          ))}
        </div>
      </form>
    </div>
  );
};

export default FormFeedback;
