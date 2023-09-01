import { FC, useEffect, useState } from 'react';

import {
  CandleDetailWithQuantity,
  CandleDetailWithQuantityAndPrice,
} from '../../typesV2/BaseProduct';
import ListProductsCart from '../../modules/order/ListProductsCart';
import FormPersonalData from '../../modules/order/FormPersonalData';
import FormFeedback from '../../modules/order/FormFeedback';
import TotalPricePanel from '../../modules/order/TotalPricePanel';

import Style from './OrderPage.module.css';
import { OrderApi } from '../../services/OrderApi';

const OrderPage: FC = () => {
  const fakeData: CandleDetailWithQuantity[] = [
    {
      candle: {
        id: 4,
        title: 'Торс мужчины',
        description: 'Торс накаченного мужчины',
        price: 20,
        weightGrams: 110,
        images: [
          { fileName: '0e47ab91-5051-476c-a668-e59032f6be54.jpg', alternativeName: 'торс-1.jpg' },
          { fileName: '80330117-2cb2-49b4-b028-4f1f4fc6fca3.jpg', alternativeName: 'торс-2.jpg' },
          { fileName: 'bec637c3-cf8f-42d3-af48-8360a0ebb6c0.jpg', alternativeName: 'торс-3.jpg' },
        ],
        typeCandle: {
          id: 2,
          title: 'Формовая',
        },
        createdAt: '2023-08-29 14:35:12.608025+00',
      },
      quantity: 1,
      decors: [
        {
          id: 2,
          title: 'Лепестки роз',
          description: 'Приятно дополнит свечу, сгорая будут отдавать легкий аромат розы.',
          price: 20,
          images: [
            {
              fileName: '93866add-a698-4418-acb3-3443634a537a.jpg',
              alternativeName: 'Тестовая картинка',
            },
          ],
        },
      ],
      layerColors: [
        {
          id: 1,
          title: 'Черный',
          description: '#191551',
          price: 1.1,
          images: [
            {
              fileName: '93866add-a698-4418-acb3-3443634a537a.jpg',
              alternativeName: 'Тестовая картинка',
            },
          ],
        },
        {
          id: 2,
          title: 'Черный',
          description: '#191551',
          price: 1.1,
          images: [
            {
              fileName: '93866add-a698-4418-acb3-3443634a537a.jpg',
              alternativeName: 'Тестовая картинка',
            },
          ],
        },
      ],
      numberOfLayers: [{ id: 1, number: 1 }],
      smells: [
        {
          id: 3,
          title: 'Зеленый чай',
          description: 'Напоминает легкую прогулку среди чайных плантаций',
          price: 23,
        },
      ],
      wicks: [
        {
          id: 1,
          title: 'Ниточка',
          description: 'Обыкновенная ващенная хлопковая нить',
          price: 10,
          images: [
            {
              fileName: '93866add-a698-4418-acb3-3443634a537a.jpg',
              alternativeName: 'Тестовая картинка',
            },
          ],
        },
      ],
    },
  ];

  const [totalPrice, setTotalPrice] = useState<number>(0);
  const [arrayCandleDetailWithQuantityAndPrice, setArrayCandleDetailWithQuantityAndPrice] =
    useState<CandleDetailWithQuantityAndPrice[]>([]);

  useEffect(() => {
    async function fetchData() {
      try {
        const arrayCandleDetailWithQuantityAndPrice = await OrderApi.index(
          'c-2~l-1_4_5~n-3~w-1~q-2.c-2~d-2~l-1_2_3_5~n-4~s-3~w-1~q-1.c-1~l-2~n-1~s-1~w-1~q-1.c-3~l-1_8~n-2~w-1~q-1',
        );
        setArrayCandleDetailWithQuantityAndPrice(arrayCandleDetailWithQuantityAndPrice);
      } catch (error) {
        console.error('Произошла ошибка при загрузке данных:', error);
      }
    }
    fetchData();
  }, []);

  function calculatePrice(arrayCandleDetails: CandleDetailWithQuantityAndPrice[]) {
    let price = 0;
    arrayCandleDetailWithQuantityAndPrice.map((item) => (price += item.price));
    return price;
  }

  function calculateTotalQuantity(arrayCandleDetails: CandleDetailWithQuantityAndPrice[]) {
    let totalQuantity = 0;
    arrayCandleDetails.map((item) => (totalQuantity += item.quantity));
    return totalQuantity;
  }

  return (
    <div className={Style.container}>
      <ListProductsCart products={arrayCandleDetailWithQuantityAndPrice} />
      <FormPersonalData />
      <FormFeedback />
      <div className={Style.rightPanelTotalPrice}>
        <TotalPricePanel
          totalPrice={calculatePrice(arrayCandleDetailWithQuantityAndPrice)}
          totalQuantityProduct={calculateTotalQuantity(arrayCandleDetailWithQuantityAndPrice)}
        />
      </div>
    </div>
  );
};

export default OrderPage;
