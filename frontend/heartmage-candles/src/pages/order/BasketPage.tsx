import { FC, useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';

import { Basket } from '../../typesV2/order/Basket';
import { CreateOrderRequest } from '../../typesV2/order/CreateOrderRequest';
import ListProductsCart from '../../modules/order/ListProductsCart';
import TotalPricePanel from '../../modules/order/TotalPricePanel';
import { feedbackType } from '../../typesV2/order/Feedback';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import { ParseToFilter } from '../../typesV2/order/ConfiguredCandleFilter';
import InputV2 from '../../components/shared/InputV2';
import FormFeedbackV2 from '../../modules/order/FormFeedbackV2';
import SelectorWithInputV2 from '../../components/order/SelectorWithInputV2';
import IconTelegram from '../../UI/IconTelegram';
import IconWhatsapp from '../../UI/IconWhatsapp';

import { OrdersApi } from '../../services/OrdersApi';
import { BasketApi } from '../../services/BasketApi';

import Style from './BasketPage.module.css';

type BasketParams = {
  id: string;
};

const OrderPage: FC = () => {
  const { id } = useParams<BasketParams>();
  const [basket, setBasket] = useState<Basket>();

  const [selectedTypeFeedback, setTypeFeedback] = useState<feedbackType>();
  const [username, setUsername] = useState<string>();

  const [canCreate, setCanCreate] = useState<boolean>(undefined);

  const navigate = useNavigate();

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const telegramRegExp =
    /(?:@|(?:(?:(?:https?:\/\/)?t(?:elegram)?)\.me\/))(\w{4,})$/;

  const formFeedback = yup
    .object({
      bot: yup
        .string()
        .test(
          'isMatches',
          'Invalid username or account link',
          function (value) {
            if (selectedTypeFeedback === feedbackType.Bot) {
              setUsername(value);
              return telegramRegExp.test(value);
            }
            return true;
          },
        ),

      telegram: yup
        .string()
        .test(
          'isMatches',
          'Invalid username or account link',
          function (value) {
            if (selectedTypeFeedback === feedbackType.Telegram) {
              setUsername(value);
              return telegramRegExp.test(value);
            }
            return true;
          },
        ),
      whatsapp: yup
        .string()
        .test('isMatches', 'Invalid phone number', function (value) {
          if (selectedTypeFeedback === feedbackType.Whatsapp) {
            setUsername(value);
            return /^(\+?\d{0,4})?\s?-?\s?(\(?\d{3}\)?)\s?-?\s?(\(?\d{3}\)?)\s?-?\s?(\(?\d{4}\)?)?$/.test(
              value,
            );
          }
          return true;
        }),
    })
    .required('One of field is required');

  const {
    register,
    formState: { isValid, errors },
  } = useForm({
    mode: 'onChange',
    resolver: yupResolver(formFeedback),
  });

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

  async function createOrder() {
    if (isValid && selectedTypeFeedback !== undefined && basket) {
      const createOrderRequest: CreateOrderRequest = {
        configuredCandlesString: getConfiguredCandlesFilter(),
        basketId: basket.id,
        feedback: {
          feedback: selectedTypeFeedback!,
          userName: username,
        },
      };

      const orderItemsResponse = await OrdersApi.createOrder(
        createOrderRequest,
      );

      if (orderItemsResponse.data && !orderItemsResponse.error) {
        navigate(`/orders/${orderItemsResponse.data}/thank`);
      } else {
        setErrorMessage([...errorMessage, orderItemsResponse.error as string]);
      }
    } else {
      setErrorMessage((prev) => [...prev, 'Выберите тип обратной связи']);
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

  useEffect(() => {
    if (selectedTypeFeedback === undefined || username === undefined) {
      setCanCreate(undefined);
    } else if (!isValid) {
      setCanCreate(false);
    } else if (isValid) {
      setCanCreate(true);
    } else {
      setCanCreate(undefined);
      return undefined;
    }
  }, [isValid, errors]);

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
            <FormFeedbackV2
              title="Личные данные"
              description="Выбирите способ связи и мы свяжемся с вами"
            >
              <SelectorWithInputV2
                title="Telegram Bot"
                icon={IconTelegram}
                onSelected={() => setTypeFeedback(feedbackType.Bot)}
                isSelected={
                  selectedTypeFeedback === feedbackType.Bot ? true : false
                }
              >
                <InputV2
                  label="Имя пользователя или ссылка"
                  name="Telegram Bot"
                  {...register('bot')}
                  error={errors.bot && errors.bot.message}
                />
              </SelectorWithInputV2>
              <SelectorWithInputV2
                title="Чат в Telegram"
                icon={IconTelegram}
                onSelected={() => setTypeFeedback(feedbackType.Telegram)}
                isSelected={
                  selectedTypeFeedback === feedbackType.Telegram ? true : false
                }
              >
                <InputV2
                  label="Имя пользователя или ссылка"
                  name="Чат в Telegram"
                  {...register('telegram')}
                  error={errors.telegram && errors.telegram.message}
                />
              </SelectorWithInputV2>
              <SelectorWithInputV2
                title="Whatsapp"
                icon={IconWhatsapp}
                onSelected={() => setTypeFeedback(feedbackType.Whatsapp)}
                isSelected={
                  selectedTypeFeedback === feedbackType.Whatsapp ? true : false
                }
              >
                <InputV2
                  label="Введите номер телефона"
                  name="Whatsapp"
                  {...register('whatsapp')}
                  error={errors.whatsapp && errors.whatsapp.message}
                />
              </SelectorWithInputV2>
            </FormFeedbackV2>
          </div>
          <div className={Style.rightPanel}>
            <TotalPricePanel
              totalPrice={basket.totalPrice}
              totalQuantityProduct={basket.totalQuantity}
              onCreateOrder={createOrder}
              isValid={canCreate}
            />
          </div>
        </>
      )}
    </div>
  );
};

export default OrderPage;
