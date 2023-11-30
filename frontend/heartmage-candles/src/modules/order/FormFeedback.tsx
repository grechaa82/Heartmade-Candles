import { FC, useState, useEffect } from 'react';

import SelectorWithInput from '../../components/order/SelectorWithInput';
import Input from '../../components/shared/Input';
import { feedbackType } from '../../typesV2/order/Feedback';
import { IconProps } from '../../UI/IconProps';

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
  icon?: FC<IconProps>;
}

export interface FormFeedbackProps {
  itemsFormFeedbacks: ItemFormFeedback[];
}

const FormFeedback: FC<FormFeedbackProps> = ({ itemsFormFeedbacks }) => {
  const evenItems = itemsFormFeedbacks.filter((_, index) => index % 2 === 0);
  const oddItems = itemsFormFeedbacks.filter((_, index) => index % 2 !== 0);

  const [windowSize, setWindowSize] = useState({
    width: window.innerWidth,
    height: window.innerHeight,
  });

  useEffect(() => {
    const handleResize = () => {
      setWindowSize({
        width: window.innerWidth,
        height: window.innerHeight,
      });
    };

    window.addEventListener('resize', handleResize);

    return () => {
      window.removeEventListener('resize', handleResize);
    };
  }, []);

  return (
    <div>
      <div className={Style.title}>Личные данные</div>
      <div className={Style.description}>
        Выбирите способ связи и мы свяжемся с вами как можно быстрей
      </div>
      <form action="" className={Style.form}>
        {windowSize.width > 576 ? (
          <>
            <div className={Style.column}>
              {evenItems.map((item, index) => (
                <SelectorWithInput
                  title={item.title.toString()}
                  icon={item.icon}
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
                  icon={item.icon}
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
          </>
        ) : (
          <div className={Style.column}>
            {itemsFormFeedbacks.map((item, index) => (
              <SelectorWithInput
                title={item.title.toString()}
                icon={item.icon}
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
        )}
      </form>
    </div>
  );
};

export default FormFeedback;
