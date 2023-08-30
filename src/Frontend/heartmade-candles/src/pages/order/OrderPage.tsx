import { FC, useState } from 'react';

import { CandleDetailWithQuantity } from '../../typesV2/BaseProduct';
import IconChevronDownLarge from '../../UI/IconChevronDownLarge';

import Style from './OrderPage.module.css';

const OrderPage: FC = () => {
  const [showMoreInfo, setShowMoreInfo] = useState(false);

  const urlToImage = 'http://localhost:5000/StaticFiles/Images/';

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

  return (
    <div className={Style.container}>
      Order
      <div className={Style.orderList}>
        <div className={Style.orderItem}>
          <div className={Style.mainInfo}>
            <div className={Style.image}>
              <img
                src={urlToImage + fakeData[0].candle.images[0].fileName}
                alt={fakeData[0].candle.images[0].alternativeName}
              />
            </div>
            <div className={Style.info}>
              <div className={Style.titleBlock}>
                <p className={Style.title}>{fakeData[0].candle.title}</p>
                <button
                  className={Style.showMoreInfoBtn}
                  type="button"
                  onClick={() => setShowMoreInfo(!showMoreInfo)}
                >
                  Показать настройки
                  <IconChevronDownLarge color="#2e67ea" />
                </button>
              </div>
              <p className={Style.quantity}>{fakeData[0].quantity}</p>
              <p className={Style.price}>N?</p>
            </div>
          </div>
          {showMoreInfo && (
            <div className={Style.paramsBlock}>
              <div className={Style.params}>
                <p className={Style.paramsTitle}>Количество слоев</p>
                {fakeData[0].numberOfLayers && (
                  <p className={Style.paramsInfo}>{fakeData[0].numberOfLayers[0].number}</p>
                )}
              </div>
              <span className={Style.separator} />
              <div className={Style.params}>
                <p className={Style.paramsTitle}>Цвета слоев</p>
                <div className={Style.paramsListInfo}>
                  {fakeData[0].layerColors?.map((layerColor, index) => (
                    <div className={Style.paramsListInfoItem}>
                      <span>{index + 1}</span>
                      <p className={Style.paramsInfo}>{layerColor.title}</p>
                    </div>
                  ))}
                </div>
              </div>
              <span className={Style.separator} />
              <div className={Style.params}>
                <p className={Style.paramsTitle}>Декор</p>
                {fakeData[0].decors && (
                  <p className={Style.paramsInfo}>{fakeData[0].decors[0].title}</p>
                )}
              </div>
              <span className={Style.separator} />
              <div className={Style.params}>
                <p className={Style.paramsTitle}>Запах</p>
                {fakeData[0].smells && (
                  <p className={Style.paramsInfo}>{fakeData[0].smells[0].title}</p>
                )}
              </div>
              <span className={Style.separator} />
              <div className={Style.params}>
                <p className={Style.paramsTitle}>Фитиль</p>
                {fakeData[0].wicks && (
                  <p className={Style.paramsInfo}>{fakeData[0].wicks[0].title}</p>
                )}
              </div>
            </div>
          )}
        </div>
      </div>
      <div>{JSON.stringify(fakeData)}</div>
    </div>
  );
};

export default OrderPage;
