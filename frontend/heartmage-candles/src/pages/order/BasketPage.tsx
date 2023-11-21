import { FC, useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

import { Basket } from '../../typesV2/order/Basket';
import { CreateOrderRequest } from '../../typesV2/order/CreateOrderRequest';
import ListProductsCart from '../../modules/order/ListProductsCart';
import FormPersonalData, { ItemFormPersonalData } from '../../modules/order/FormPersonalData';
import FormFeedback, { ItemFormFeedback } from '../../modules/order/FormFeedback';
import TotalPricePanel from '../../modules/order/TotalPricePanel';
import { feedbackType } from '../../typesV2/order/Feedback';
import IconTelegram from '../../UI/IconTelegram';
import IconInstagram from '../../UI/IconInstagram';
import IconWhatsapp from '../../UI/IconWhatsapp';
import ButtonWithIcon from '../../components/shared/ButtonWithIcon';
import IconArrowLeftLarge from '../../UI/IconArrowLeftLarge';
import ListErrorPopUp from '../../modules/constructor/ListErrorPopUp';
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

  const [firstName, setFirstName] = useState<string>('');
  const [lastName, setLastName] = useState<string>('');
  const [email, setEmail] = useState<string>('');
  const [phone, setPhone] = useState('');
  const [selectedTypeFeedback, setTypeFeedback] = useState<feedbackType>();
  const [username, setUsername] = useState<string>('');

  const navigate = useNavigate();

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const validateFirstNameAndLastName = (value: string) => {
    const regex = /^[a-zA-Zа-яА-Я]+$/;
    return regex.test(value);
  };

  const validateEmail = (value: string) => {
    return /^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$/.test(value);
  };

  const validatePhone = (value: string) => {
    return /^((8|\+7|7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?\d{3}[\- ]?\d{2}[\- ]?\d{2}?$/.test(value);
  };

  const validateTelegramAndInstagram = (value: string) => {
    const regex = /@[a-zA-Z0-9_]{1,32}$/;
    return regex.test(value);
  };

  const itemsFormPersonalData: ItemFormPersonalData[] = [
    {
      label: 'Имя',
      value: firstName,
      onChange: setFirstName,
      isRequired: true,
      validation: validateFirstNameAndLastName,
    },
    {
      label: 'Фамилия',
      value: lastName,
      onChange: setLastName,
      isRequired: true,
      validation: validateFirstNameAndLastName,
    },
    {
      label: 'Элекронная почта',
      value: email,
      onChange: setEmail,
      isRequired: false,
      validation: validateEmail,
    },
    {
      label: 'Номер телефона',
      value: phone,
      onChange: setPhone,
      isRequired: true,
      validation: validatePhone,
    },
  ];

  const itemsFormFeedback: ItemFormFeedback[] = [
    {
      title: feedbackType.Telegram,
      label: 'Введите имя пользователя',
      value: username,
      onChangeSelectedForm: setTypeFeedback,
      onChangeUsername: setUsername,
      isRequired: true,
      isSelected: selectedTypeFeedback === feedbackType.Telegram ? true : false,
      validation: validateTelegramAndInstagram,
      icon: IconTelegram,
    },
    {
      title: feedbackType.Instagram,
      label: 'Введите имя пользователя',
      value: username,
      onChangeSelectedForm: setTypeFeedback,
      onChangeUsername: setUsername,
      isRequired: true,
      isSelected: selectedTypeFeedback === feedbackType.Instagram ? true : false,
      validation: validateTelegramAndInstagram,
      icon: IconInstagram,
    },
    {
      title: feedbackType.Whatsapp,
      label: 'Введите имя пользователя',
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
    var { canCreateOrder, errorMessages }: { canCreateOrder: boolean; errorMessages: string[] } =
      isValidUserData();

    if (canCreateOrder && basket) {
      const createOrderRequest: CreateOrderRequest = {
        configuredCandlesString: getConfiguredCandlesFilter(),
        basketId: basket.id,
        user: {
          firstName: firstName,
          lastName: lastName,
          phone: phone,
          email: email,
        },
        feedback: {
          feedback: selectedTypeFeedback!,
          userName: username,
        },
      };

      const orderItemsResponse = await OrdersApi.createOrder(createOrderRequest);
      if (orderItemsResponse.data && !orderItemsResponse.error) {
        navigate(`/orders/${orderItemsResponse.data}/thank`);
      } else {
        setErrorMessage([...errorMessage, orderItemsResponse.error as string]);
      }
    } else {
      setErrorMessage((prev) => [...prev, ...errorMessages.flat()]);
    }
  }

  function isValidUserData(): { canCreateOrder: boolean; errorMessages: string[] } {
    let canCreateOrder = true;
    const errorMessages: string[] = [];

    if (!firstName) {
      errorMessages.push(`Заполните поле 'Имя'`);
    }
    if (firstName && !validateFirstNameAndLastName(firstName)) {
      errorMessages.push(`Поле 'Имя' введено неверно`);
      canCreateOrder = false;
    }

    if (!lastName) {
      errorMessages.push(`Заполните поле 'Фамилия'`);
    }
    if (lastName && !validateFirstNameAndLastName(lastName)) {
      errorMessages.push(`Поле 'Фамилия' введено неверно`);
      canCreateOrder = false;
    }
    if (!(email.length < 5) && !validateEmail(email)) {
      errorMessages.push(`Поле 'Электронная почта' введено неверно`);
      canCreateOrder = false;
    }

    if (!phone) {
      errorMessages.push(`Заполните поле 'Номер телефона'`);
    }
    if (phone && !validatePhone(phone)) {
      errorMessages.push(`Поле 'Номер телефона' введено неверно`);
      canCreateOrder = false;
    }

    if (!selectedTypeFeedback) {
      errorMessages.push(`Выберите тип обратной связи`);
      canCreateOrder = false;
    }
    if (
      selectedTypeFeedback &&
      !(validatePhone(username) || validateTelegramAndInstagram(username))
    ) {
      errorMessages.push(`Введите корректные данные для обратной связи`);
      canCreateOrder = false;
    }
    return { canCreateOrder, errorMessages };
  }

  const getConfiguredCandlesFilter = () => {
    if (basket) {
      return basket.items.map((item) => ParseToFilter(item.configuredCandleFilter)).join('.');
    }

    return '';
  };

  const handleNavigateToConstructor = () => {
    navigate(`/constructor?${getConfiguredCandlesFilter()}`);
  };

  return (
    <div className={Style.container}>
      <div className={Style.popUpNotification}>
        <ListErrorPopUp messages={errorMessage} />
      </div>
      {basket && (
        <>
          <div className={Style.leftPanel}>
            <div className={Style.backBtn}>
              <ButtonWithIcon
                text="Конструктор"
                onClick={handleNavigateToConstructor}
                icon={IconArrowLeftLarge}
                color="#777"
              />
            </div>
            <ListProductsCart products={basket.items} />
            <FormPersonalData itemsFormPersonalData={itemsFormPersonalData} />
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
