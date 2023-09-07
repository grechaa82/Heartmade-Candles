import { FC, useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

import { CandleDetailWithQuantityAndPrice, OrderCreaterRequest } from '../../typesV2/BaseProduct';
import ListProductsCart from '../../modules/order/ListProductsCart';
import FormPersonalData, { ItemFormPersonalData } from '../../modules/order/FormPersonalData';
import FormFeedback, { ItemFormFeedback } from '../../modules/order/FormFeedback';
import TotalPricePanel from '../../modules/order/TotalPricePanel';

import { OrderApi } from '../../services/OrderApi';

import Style from './OrderPage.module.css';

const OrderPage: FC = () => {
  const [arrayCandleDetailWithQuantityAndPrice, setArrayCandleDetailWithQuantityAndPrice] =
    useState<CandleDetailWithQuantityAndPrice[]>([]);
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

  console.log(firstName, lastName, email, phone, selectedTypeFeedback, username);

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
        const arrayCandleDetailWithQuantityAndPrice = await OrderApi.index(configuredCandlesString);
        setArrayCandleDetailWithQuantityAndPrice(arrayCandleDetailWithQuantityAndPrice);
      } catch (error) {
        console.error('Произошла ошибка при загрузке данных:', error);
      }
    }
  }, []);

  function calculatePrice(arrayCandleDetails: CandleDetailWithQuantityAndPrice[]) {
    let price = 0;
    arrayCandleDetails.map((item) => (price += item.price));
    return price;
  }

  function calculateTotalQuantity(arrayCandleDetails: CandleDetailWithQuantityAndPrice[]) {
    let totalQuantity = 0;
    arrayCandleDetails.map((item) => (totalQuantity += item.quantity));
    return totalQuantity;
  }

  async function createOrder() {
    let canCreateOrder = true;

    if (!validateFirstNameAndLastName(firstName)) {
      canCreateOrder = false;
      console.log('1');
    }
    if (!validateFirstNameAndLastName(lastName)) {
      canCreateOrder = false;
      console.log('2');
    }
    if (!(email.length < 5) && !validateEmail(email)) {
      canCreateOrder = false;
      console.log('3');
    }
    if (!validatePhone(phone)) {
      canCreateOrder = false;
      console.log('4');
    }
    if (
      !(selectedTypeFeedback.length < 2) &&
      !(validatePhone(username) || validateTelegramAndInstagram(username))
    ) {
      canCreateOrder = false;
      console.log('5');
    }

    if (canCreateOrder && configuredCandlesString) {
      var orderCreaterRequest: OrderCreaterRequest = {
        configuredCandlesString: configuredCandlesString,
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
      const response = await OrderApi.createOrder(orderCreaterRequest);

      console.log(response);
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
