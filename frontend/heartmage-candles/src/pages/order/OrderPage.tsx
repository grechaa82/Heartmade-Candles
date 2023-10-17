import { FC, useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

import { OrderItemFilter } from '../../typesV2/shared/OrderItemFilter';
import { OrderItem } from '../../typesV2/order/OrderItem';
import { CreateOrderRequest } from '../../typesV2/order/CreateOrderRequest';
import { OrderItemFilterRequest } from '../../typesV2/order/OrderItemFilterRequest';
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

import { OrdersApi } from '../../services/OrdersApi';

import Style from './OrderPage.module.css';

const OrderPage: FC = () => {
  const [arrayCandleDetailWithQuantityAndPrice, setArrayCandleDetailWithQuantityAndPrice] =
    useState<OrderItem[]>([]);
  const [configuredCandlesString, setConfiguredCandlesString] = useState<string | undefined>();
  const [firstName, setFirstName] = useState<string>('');
  const [lastName, setLastName] = useState<string>('');
  const [email, setEmail] = useState<string>('');
  const [phone, setPhone] = useState('');
  const [selectedTypeFeedback, setTypeFeedback] = useState<feedbackType>();
  const [username, setUsername] = useState<string>('');

  const location = useLocation();
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
    if (!configuredCandlesString) {
      const searchParams = new URLSearchParams(location.search);

      let queryString = decodeURIComponent(searchParams.toString().replace(/=$/, ''));

      setConfiguredCandlesString(queryString);

      fetchData(queryString);
    }

    async function fetchData(configuredCandlesString: string) {
      const orderItemsResponse = await OrdersApi.get(configuredCandlesString);
      if (orderItemsResponse.data && !orderItemsResponse.error) {
        setArrayCandleDetailWithQuantityAndPrice(orderItemsResponse.data);
      } else {
        setErrorMessage([...errorMessage, orderItemsResponse.error as string]);
      }
    }
  }, []);

  function calculateTotalPrice(arrayCandleDetails: OrderItem[]) {
    let price = 0;
    arrayCandleDetails.map((item) => (price += item.price));
    return price;
  }

  function calculateTotalQuantity(arrayCandleDetails: OrderItem[]) {
    let totalQuantity = 0;
    arrayCandleDetails.map((item) => (totalQuantity += item.quantity));
    return totalQuantity;
  }

  async function createOrder() {
    var { canCreateOrder, errorMessages }: { canCreateOrder: boolean; errorMessages: string[] } =
      isValidUserData();

    if (canCreateOrder && configuredCandlesString) {
      const orderItemFilters = configuredCandlesString
        .split('.')
        .map((filter) => OrderItemFilter.parseToOrderItemFilter(filter));

      const orderItemFilterRequests: OrderItemFilterRequest[] = orderItemFilters.map(
        (orderItemFilter: OrderItemFilter) => {
          return {
            candleId: orderItemFilter.candleId,
            decorId: orderItemFilter.decorId ? orderItemFilter.decorId : 0,
            numberOfLayerId: orderItemFilter.numberOfLayerId,
            layerColorIds: orderItemFilter.layerColorIds,
            smellId: orderItemFilter.smellId ? orderItemFilter.smellId : 0,
            wickId: orderItemFilter.wickId,
            quantity: orderItemFilter.quantity,
          };
        },
      );

      const createOrderRequest: CreateOrderRequest = {
        configuredCandlesString: configuredCandlesString,
        orderItemFilters: orderItemFilterRequests,
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
      if (orderItemsResponse.error) {
        setErrorMessage([...errorMessage, orderItemsResponse.error]);
      }

      navigate('/orders/thank');
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

  const handleNavigateToConstructor = () => {
    navigate(`/constructor?${configuredCandlesString}`);
  };

  return (
    <div className={Style.container}>
      <div className={Style.popUpNotification}>
        <ListErrorPopUp messages={errorMessage} />
      </div>
      <div className={Style.leftPanel}>
        <div className={Style.backBtn}>
          <ButtonWithIcon
            text="Конструктор"
            onClick={handleNavigateToConstructor}
            icon={IconArrowLeftLarge}
            color="#777"
          />
        </div>
        <ListProductsCart products={arrayCandleDetailWithQuantityAndPrice} />
        <FormPersonalData itemsFormPersonalData={itemsFormPersonalData} />
        <FormFeedback itemsFormFeedbacks={itemsFormFeedback} />
      </div>
      <div className={Style.rightPanel}>
        <TotalPricePanel
          totalPrice={calculateTotalPrice(arrayCandleDetailWithQuantityAndPrice)}
          totalQuantityProduct={calculateTotalQuantity(arrayCandleDetailWithQuantityAndPrice)}
          onCreateOrder={createOrder}
        />
      </div>
    </div>
  );
};

export default OrderPage;
