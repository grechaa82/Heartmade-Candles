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

  function addCandleToListProductsCart() {}

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
