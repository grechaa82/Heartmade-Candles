import { FC, useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

import { Basket } from '../../typesV2/order/Basket';
import { CreateOrderRequest } from '../../typesV2/order/CreateOrderRequest';
import ListProductsCart from '../../modules/order/ListProductsCart';
import FormFeedback, {
  ItemFormFeedback,
} from '../../modules/order/FormFeedback';
import TotalPricePanel from '../../modules/order/TotalPricePanel';
import { feedbackType } from '../../typesV2/order/Feedback';
import IconTelegram from '../../UI/IconTelegram';
import IconWhatsapp from '../../UI/IconWhatsapp';
import ButtonWithIcon from '../../components/shared/ButtonWithIcon';
import IconArrowLeftLarge from '../../UI/IconArrowLeftLarge';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import { ParseToFilter } from '../../typesV2/order/ConfiguredCandleFilter';

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
  const [username, setUsername] = useState<string>('');

  const navigate = useNavigate();

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const validatePhone = (value: string) => {
    return /^((8|\+7|7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?\d{3}[\- ]?\d{2}[\- ]?\d{2}?$/.test(
      value,
    );
  };

  const validateTelegram = (value: string) => {
    const regex = /(?:@|(?:(?:(?:https?:\/\/)?t(?:elegram)?)\.me\/))(\w{4,})$/;
    return regex.test(value);
  };

  const itemsFormFeedback: ItemFormFeedback[] = [
    {
      feedbackType: feedbackType.Bot,
      title: 'Telegram Bot',
      label: 'Введите имя пользователя или ссылку на аккаунт',
      value: username,
      onChangeSelectedForm: setTypeFeedback,
      onChangeUsername: setUsername,
      isRequired: true,
      isSelected: selectedTypeFeedback === feedbackType.Bot ? true : false,
      validation: validateTelegram,
      icon: IconTelegram,
    },
    {
      feedbackType: feedbackType.Telegram,
      title: 'Чат в Telegram',
      label: 'Введите имя пользователя или ссылку на аккаунт',
      value: username,
      onChangeSelectedForm: setTypeFeedback,
      onChangeUsername: setUsername,
      isRequired: true,
      isSelected: selectedTypeFeedback === feedbackType.Telegram ? true : false,
      validation: validateTelegram,
      icon: IconTelegram,
    },
    {
      feedbackType: feedbackType.Whatsapp,
      title: 'Whatsapp',
      label: 'Введите номер телефона',
      value: username,
      onChangeSelectedForm: setTypeFeedback,
      onChangeUsername: setUsername,
      isRequired: true,
      isSelected: selectedTypeFeedback === feedbackType.Whatsapp ? true : false,
      validation: validatePhone,
      icon: IconWhatsapp,
    },
  ];

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
    var {
      canCreateOrder,
      errorMessages,
    }: { canCreateOrder: boolean; errorMessages: string[] } = isValidUserData();

    if (canCreateOrder && basket) {
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
      setErrorMessage((prev) => [...prev, ...errorMessages.flat()]);
    }
  }

  function isValidUserData(): {
    canCreateOrder: boolean;
    errorMessages: string[];
  } {
    let canCreateOrder = true;
    const errorMessages: string[] = [];

    if (!selectedTypeFeedback) {
      errorMessages.push(`Выберите тип обратной связи`);
      canCreateOrder = false;
    }
    if (
      selectedTypeFeedback &&
      !(validatePhone(username) || validateTelegram(username))
    ) {
      errorMessages.push(`Введите корректные данные для обратной связи`);
      canCreateOrder = false;
    }
    return { canCreateOrder, errorMessages };
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
            <FormFeedback itemsFormFeedbacks={itemsFormFeedback} />
          </div>
          <div className={Style.rightPanel}>
            <TotalPricePanel
              totalPrice={basket.totalPrice}
              totalQuantityProduct={basket.totalQuantity}
              onCreateOrder={createOrder}
            />
          </div>
        </>
      )}
    </div>
  );
};

export default OrderPage;
