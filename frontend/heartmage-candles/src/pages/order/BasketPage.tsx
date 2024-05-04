import { FC, useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { FormProvider, SubmitHandler, useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';

import { Basket } from '../../typesV2/order/Basket';
import { CreateOrderRequest } from '../../typesV2/order/CreateOrderRequest';
import ListProductsCart from '../../modules/order/ListProductsCart';
import TotalPricePanel from '../../modules/order/TotalPricePanel';
import { feedbackType } from '../../typesV2/order/Feedback';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import { ParseToFilter } from '../../typesV2/order/ConfiguredCandleFilter';
import IconTelegram from '../../UI/IconTelegram';
import IconWhatsapp from '../../UI/IconWhatsapp';
import Input from '../../components/shared/Input';
import FormFeedback from '../../modules/order/FormFeedback';
import SelectorWithInput from '../../components/order/SelectorWithInput';

import { OrdersApi } from '../../services/OrdersApi';
import { BasketApi } from '../../services/BasketApi';

import Style from './BasketPage.module.css';

type BasketParams = {
  id: string;
};

type validationType = {
  bot: string;
  telegram: string;
  whatsapp: string;
};

const OrderPage: FC = () => {
  const { id } = useParams<BasketParams>();
  const [basket, setBasket] = useState<Basket>();
  const [selectedTypeFeedback, setTypeFeedback] = useState<feedbackType>();
  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const navigate = useNavigate();

  const telegramRegExp =
    /(?:@|(?:(?:(?:https?:\/\/)?t(?:elegram)?)\.me\/))(\w{4,})$/;
  const whatsappRegExp =
    /^(\+?\d{0,4})?\s?-?\s?(\(?\d{3}\)?)\s?-?\s?(\(?\d{3}\)?)\s?-?\s?(\(?\d{4}\)?)?$/;

  const formFeedback = yup
    .object({
      bot: yup.string().matches(telegramRegExp, {
        message: 'Invalid username or account link',
        excludeEmptyString: true,
      }),

      telegram: yup.string().matches(telegramRegExp, {
        message: 'Invalid username or account link',
        excludeEmptyString: true,
      }),

      whatsapp: yup.string().matches(whatsappRegExp, {
        message: 'Invalid phone number',
        excludeEmptyString: true,
      }),
    })
    .test('isSelected', 'One of field is required', function (value) {
      if (value.bot || value.telegram || value.whatsapp) {
        return true;
      }
      return false;
    })
    .required('One of field is required');

  const methods = useForm({
    mode: 'onChange',
    resolver: yupResolver(formFeedback),
  });

  const {
    handleSubmit,
    formState: { isValid, errors, isDirty },
  } = methods;

  useEffect(() => {
    async function fetchData() {
      if (id) {
        const basketResponse = await BasketApi.getById(id);
        if (basketResponse.data && !basketResponse.error) {
          setBasket(basketResponse.data);
        } else {
          setErrorMessage([...errorMessage, basketResponse.error as string]);
        }
      }
    }

    fetchData();
  }, [id]);

  async function createOrder(feedbackType: feedbackType, userName: string) {
    const createOrderRequest: CreateOrderRequest = {
      configuredCandlesString: getConfiguredCandlesFilter(),
      basketId: basket.id,
      feedback: {
        feedback: feedbackType!,
        userName: userName,
      },
    };

    const orderItemsResponse = await OrdersApi.createOrder(createOrderRequest);

    if (orderItemsResponse.data && !orderItemsResponse.error) {
      navigate(`/orders/${orderItemsResponse.data}/thank`);
    } else {
      setErrorMessage([...errorMessage, orderItemsResponse.error as string]);
    }
  }

  const getConfiguredCandlesFilter = () => {
    if (basket) {
      return basket.items
        .map((item) => ParseToFilter(item.configuredCandleFilter))
        .join('.');
    }
    return '';
  };

  const handleNavigateToConstructor = () => {
    navigate(`/constructor?${getConfiguredCandlesFilter()}`);
  };

  const onSubmit: SubmitHandler<validationType> = (data) => {
    if (selectedTypeFeedback === feedbackType.Bot && data.bot) {
      createOrder(selectedTypeFeedback, data.bot);
    } else if (
      selectedTypeFeedback === feedbackType.Telegram &&
      data.telegram
    ) {
      createOrder(selectedTypeFeedback, data.telegram);
    } else if (
      selectedTypeFeedback === feedbackType.Whatsapp &&
      data.whatsapp
    ) {
      createOrder(selectedTypeFeedback, data.whatsapp);
    } else {
      setErrorMessage((prev) => [...prev, 'Выберите тип обратной связи']);
    }
  };

  const onClick = () => {
    handleSubmit(onSubmit)();
  };

  return (
    <div className={Style.container}>
      <ListErrorPopUp messages={errorMessage} />
      {basket && (
        <>
          <div className={Style.leftPanel}>
            <div className={Style.backBtn}>
              <button onClick={handleNavigateToConstructor}>Вернуться</button>
            </div>
            <ListProductsCart products={basket.items} />
            <FormFeedback
              title="Личные данные"
              description="Выбирите способ связи и мы свяжемся с вами"
            >
              <FormProvider {...methods}>
                <form onSubmit={handleSubmit(onSubmit)}>
                  <SelectorWithInput
                    title="Telegram Bot"
                    icon={IconTelegram}
                    onSelected={() => setTypeFeedback(feedbackType.Bot)}
                    isSelected={
                      selectedTypeFeedback === feedbackType.Bot ? true : false
                    }
                  >
                    <Input
                      name="bot"
                      error={errors.bot?.message}
                      label="Имя пользователя или ссылка"
                    />
                  </SelectorWithInput>
                  <SelectorWithInput
                    title="Чат в Telegram"
                    icon={IconTelegram}
                    onSelected={() => setTypeFeedback(feedbackType.Telegram)}
                    isSelected={
                      selectedTypeFeedback === feedbackType.Telegram
                        ? true
                        : false
                    }
                  >
                    <Input
                      name="telegram"
                      error={errors.telegram?.message}
                      label={'Имя пользователя или ссылка'}
                    />
                  </SelectorWithInput>
                  <SelectorWithInput
                    title="Whatsapp"
                    icon={IconWhatsapp}
                    onSelected={() => setTypeFeedback(feedbackType.Whatsapp)}
                    isSelected={
                      selectedTypeFeedback === feedbackType.Whatsapp
                        ? true
                        : false
                    }
                  >
                    <Input
                      name="whatsapp"
                      error={errors.whatsapp?.message}
                      label="Введите номер телефона"
                    />
                  </SelectorWithInput>
                </form>
              </FormProvider>
            </FormFeedback>
          </div>
          <div className={Style.rightPanel}>
            <TotalPricePanel
              totalPrice={basket.totalPrice}
              totalQuantityProduct={basket.totalQuantity}
              onCreateOrder={onClick}
              isValid={!isDirty ? undefined : isValid}
            />
          </div>
        </>
      )}
    </div>
  );
};

export default OrderPage;
