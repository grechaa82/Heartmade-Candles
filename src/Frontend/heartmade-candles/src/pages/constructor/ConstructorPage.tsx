import { FC, useState, useEffect } from 'react';

import ListProductsCart from '../../modules/constructor/ListProductsCart';
import CandleForm from '../../modules/constructor/CandleForm';
import { CandleDetail, CandleDetailWithQuantity, ImageProduct } from '../../typesV2/BaseProduct';
import CandleSelectionPanel from '../../modules/constructor/CandleSelectionPanel';
import { CandleTypeWithCandles } from '../../typesV2/CandleTypeWithCandles';

import Style from './ConstructorPage.module.css';

import { ConstructorApi } from '../../services/ConstructorApi';

const ConstructorPage: FC = () => {
  const [candleDetail, setCandleDetail] = useState<CandleDetail>();
  const [candleDetailWithQuantity, setCandleDetailWithQuantity] = useState<
    CandleDetailWithQuantity[]
  >([]);
  const [candleTypeWithCandles, setCandleTypeWithCandles] = useState<CandleTypeWithCandles[]>();

  async function showCandleForm(candleId: number) {
    try {
      const candleDetail = await ConstructorApi.getCandleById(candleId.toString());
      setCandleDetail(candleDetail);
    } catch (error) {
      console.error('Произошла ошибка при загрузке данных:', error);
    }
  }

  function hideCandleForm() {
    setCandleDetail(undefined);
  }

  function addCandleToListProductsCart() {
    const fakeCandleDetailWithQuantity: CandleDetailWithQuantity[] = [
      {
        candle: {
          id: 1,
          title: 'Example Candle',
          description: 'This is an example candle',
          isActive: true,
          price: 10,
          images: [
            {
              fileName: '93866add-a698-4418-acb3-3443634a537a.jpg',
              alternativeName: 'Example Candle',
            },
          ],
          weightGrams: 200,
          typeCandle: {
            id: 1,
            title: 'Type 1',
          },
          createdAt: '2022-01-01',
        },
        decors: [
          {
            id: 1,
            title: 'Decor 1',
            description: 'This is decor 1',
            isActive: true,
            price: 5,
            images: [
              {
                fileName: 'decor1.jpg',
                alternativeName: 'Decor 1',
              },
            ],
          },
        ],
        layerColors: [
          {
            id: 1,
            title: 'Color 1',
            description: 'This is color 1',
            isActive: true,
            price: 2,
            images: [
              {
                fileName: 'color1.jpg',
                alternativeName: 'Color 1',
              },
            ],
          },
        ],
        numberOfLayers: [
          {
            id: 1,
            number: 2,
          },
        ],
        smells: [
          {
            id: 1,
            title: 'Smell 1',
            description: 'This is smell 1',
            isActive: true,
            price: 3,
          },
        ],
        wicks: [
          {
            id: 1,
            title: 'Wick 1',
            description: 'This is wick 1',
            isActive: true,
            price: 2,
            images: [
              {
                fileName: 'wick1.jpg',
                alternativeName: 'Wick 1',
              },
            ],
          },
        ],
        quantity: 2,
      },
      {
        candle: {
          id: 1,
          title: 'Example Candle',
          description: 'This is an example candle',
          isActive: true,
          price: 10,
          images: [
            {
              fileName: '93866add-a698-4418-acb3-3443634a537a.jpg',
              alternativeName: 'Example Candle',
            },
          ],
          weightGrams: 200,
          typeCandle: {
            id: 1,
            title: 'Type 1',
          },
          createdAt: '2022-01-01',
        },
        decors: [
          {
            id: 1,
            title: 'Decor 1',
            description: 'This is decor 1',
            isActive: true,
            price: 5,
            images: [
              {
                fileName: 'decor1.jpg',
                alternativeName: 'Decor 1',
              },
            ],
          },
        ],
        layerColors: [
          {
            id: 1,
            title: 'Color 1',
            description: 'This is color 1',
            isActive: true,
            price: 2,
            images: [
              {
                fileName: 'color1.jpg',
                alternativeName: 'Color 1',
              },
            ],
          },
        ],
        numberOfLayers: [
          {
            id: 1,
            number: 2,
          },
        ],
        smells: [
          {
            id: 1,
            title: 'Smell 1',
            description: 'This is smell 1',
            isActive: true,
            price: 3,
          },
        ],
        wicks: [
          {
            id: 1,
            title: 'Wick 1',
            description: 'This is wick 1',
            isActive: true,
            price: 2,
            images: [
              {
                fileName: 'wick1.jpg',
                alternativeName: 'Wick 1',
              },
            ],
          },
        ],
        quantity: 2,
      },
    ];
    setCandleDetailWithQuantity(fakeCandleDetailWithQuantity);
  }

  useEffect(() => {
    async function fetchData() {
      try {
        const candles = await ConstructorApi.getCandles();
        setCandleTypeWithCandles(candles);
      } catch (error) {
        console.error('Произошла ошибка при загрузке данных:', error);
      }
    }

    fetchData();
  }, []);

  const handleChangeCandleDetailWithQuantity = (products: CandleDetailWithQuantity[]) => {
    setCandleDetailWithQuantity(products);
  };

  const handleSelectCandle = (candle: ImageProduct) => {
    showCandleForm(candle.id);
  };

  return (
    <>
      <div className={Style.container}>
        <div className={Style.leftPanel}>
          <ListProductsCart
            products={candleDetailWithQuantity}
            onChangeCandleDetailWithQuantity={handleChangeCandleDetailWithQuantity}
          />
        </div>
        <div className={Style.imagePanel}>
          <button onClick={() => showCandleForm(2)}>showCandleForm</button>
          <button onClick={() => hideCandleForm()}>hideCandleForm</button>
          <button onClick={() => addCandleToListProductsCart()}>addCandleToListProductsCart</button>
        </div>
        {candleDetail ? (
          <div className={Style.rightPanel}>
            <CandleForm candleDetailData={candleDetail} />
          </div>
        ) : (
          candleTypeWithCandles && (
            <CandleSelectionPanel
              data={candleTypeWithCandles}
              onSelectCandle={handleSelectCandle}
            />
          )
        )}
      </div>
    </>
  );
};

export default ConstructorPage;
