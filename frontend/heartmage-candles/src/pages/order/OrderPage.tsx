import { FC, useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

import { OrderItemFilter } from '../../typesV2/OrderItemFilter';
import { OrderItem } from '../../typesV2/order/OrderItem';
import { CreateOrderRequest } from '../../typesV2/order/CreateOrderRequest';
import ListProductsCart from '../../modules/order/ListProductsCart';
import FormPersonalData, { ItemFormPersonalData } from '../../modules/order/FormPersonalData';
import FormFeedback, { ItemFormFeedback } from '../../modules/order/FormFeedback';
import TotalPricePanel from '../../modules/order/TotalPricePanel';

import { OrdersApi } from '../../services/OrdersApi';

import Style from './OrderPage.module.css';

function parseCandleDetailIdsWithQuantityString(strings: string[]): OrderItemFilter[] {
  const candleDetailIdsWithQuantities: OrderItemFilter[] = [];

  for (const str of strings) {
    if (str === '') {
      continue;
    }

    const candleDetailIdsWithQuantity: OrderItemFilter = {
      candleId: 0,
      quantity: 0,
    };

    const components = str.split('~');

    for (const component of components) {
      const [type, value] = component.split('-');

      switch (type) {
        case 'c':
          candleDetailIdsWithQuantity.candleId = parseInt(value);
          break;
        case 'd':
          candleDetailIdsWithQuantity.decorIds = mapParseToInt(value);
          break;
        case 'l':
          candleDetailIdsWithQuantity.layerColorIds = mapParseToInt(value);
          break;
        case 'n':
          candleDetailIdsWithQuantity.numberOfLayerIds = mapParseToInt(value);
          break;
        case 's':
          candleDetailIdsWithQuantity.smellIds = mapParseToInt(value);
          break;
        case 'w':
          candleDetailIdsWithQuantity.wickIds = mapParseToInt(value);
          break;
        case 'q':
          candleDetailIdsWithQuantity.quantity = parseInt(value);
          break;
      }
    }
    candleDetailIdsWithQuantities.push(candleDetailIdsWithQuantity);
  }

  return candleDetailIdsWithQuantities;
}

function mapParseToInt(value: string): number[] {
  return value.split('_').map(Number);
}

const OrderPage: FC = () => {
  const [arrayCandleDetailWithQuantityAndPrice, setArrayCandleDetailWithQuantityAndPrice] =
    useState<OrderItem[]>([]);
  const [configuredCandlesString, setConfiguredCandlesString] = useState<string | undefined>();
  const [firstName, setFirstName] = useState<string>('');
  const [lastName, setLastName] = useState<string>('');
  const [email, setEmail] = useState<string>('');
  const [phone, setPhone] = useState('');
  const [selectedTypeFeedback, setTypeFeedback] = useState<string>('');
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

  const itemFormPersonalData: ItemFormPersonalData[] = [
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
      title: 'Telegram',
      label: 'Введите имя пользователя',
      value: username,
      onChangeSelectedForm: setTypeFeedback,
      onChangeUsername: setUsername,
      isRequired: true,
      isSelected: selectedTypeFeedback === 'Telegram' ? true : false,
      validation: validateTelegramAndInstagram,
    },
    {
      title: 'Instagram',
      label: 'Введите имя пользователя',
      value: username,
      onChangeSelectedForm: setTypeFeedback,
      onChangeUsername: setUsername,
      isRequired: true,
      isSelected: selectedTypeFeedback === 'Instagram' ? true : false,
      validation: validateTelegramAndInstagram,
    },
    {
      title: 'Whatsapp',
      label: 'Введите имя пользователя',
      value: username,
      onChangeSelectedForm: setTypeFeedback,
      onChangeUsername: setUsername,
      isRequired: true,
      isSelected: selectedTypeFeedback === 'Whatsapp' ? true : false,
      validation: validatePhone,
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
        const arrayCandleDetailWithQuantityAndPrice = await OrdersApi.get(configuredCandlesString);
        setArrayCandleDetailWithQuantityAndPrice(arrayCandleDetailWithQuantityAndPrice);
      } catch (error) {
        console.error('Произошла ошибка при загрузке данных:', error);
      }
    }
  }, []);

  function calculatePrice(arrayCandleDetails: OrderItem[]) {
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
      !(selectedTypeFeedback.length < 2) &&
      !(validatePhone(username) || validateTelegramAndInstagram(username))
    ) {
      canCreateOrder = false;
    }

    if (canCreateOrder && configuredCandlesString) {
      var createOrderRequest: CreateOrderRequest = {
        configuredCandlesString: configuredCandlesString,
        orderItemFilters: parseCandleDetailIdsWithQuantityString(
          configuredCandlesString.split('.'),
        ),
        user: {
          firstName: firstName,
          lastName: lastName,
          phone: phone,
          email: email,
        },
        feedback: {
          typeFeedback: selectedTypeFeedback,
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
        <FormPersonalData itemsFormPersonalData={itemFormPersonalData} />
        <FormFeedback itemsFormFeedbacks={itemsFormFeedback} />
      </div>
      <div className={Style.rightPanel}>
        <TotalPricePanel
          totalPrice={calculatePrice(arrayCandleDetailWithQuantityAndPrice)}
          totalQuantityProduct={calculateTotalQuantity(arrayCandleDetailWithQuantityAndPrice)}
          onCreateOrder={createOrder}
        />
      </div>
    </div>
  );
};

export default OrderPage;
