import { FC, useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

import { OrderItemFilter } from '../../typesV2/shared/OrderItemFilter';
import { OrderItem } from '../../typesV2/order/OrderItem';
import { CreateOrderRequest, OrderItemFilterRequest } from '../../typesV2/order/CreateOrderRequest';
import ListProductsCart from '../../modules/order/ListProductsCart';
import FormPersonalData, { ItemFormPersonalData } from '../../modules/order/FormPersonalData';
import FormFeedback, { ItemFormFeedback } from '../../modules/order/FormFeedback';
import TotalPricePanel from '../../modules/order/TotalPricePanel';
import { feedbackType } from '../../typesV2/order/Feedback';
import IconTelegram from '../../UI/IconTelegram';
import IconInstagram from '../../UI/IconInstagram';
import IconWhatsapp from '../../UI/IconWhatsapp';

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

      navigate('');
    }

    async function fetchData(configuredCandlesString: string) {
      try {
        const orderItems: OrderItem[] = await OrdersApi.get(configuredCandlesString);
        setArrayCandleDetailWithQuantityAndPrice(orderItems);
      } catch (error) {
        console.error('Произошла ошибка при загрузке данных:', error);
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
    let canCreateOrder = true;

    if (!validateFirstNameAndLastName(firstName)) {
      canCreateOrder = false;
    }
    if (!validateFirstNameAndLastName(lastName)) {
      canCreateOrder = false;
    }
    if (!(email.length < 5) && !validateEmail(email)) {
      canCreateOrder = false;
    }
    if (!validatePhone(phone)) {
      canCreateOrder = false;
    }
    if (
      !selectedTypeFeedback &&
      !(validatePhone(username) || validateTelegramAndInstagram(username))
    ) {
      canCreateOrder = false;
    }

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

      await OrdersApi.createOrder(createOrderRequest);
    }
  }

  return (
    <div className={Style.container}>
      <div className={Style.leftPanel}>
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
