import { FC, useState } from 'react';

import { CandleDetail } from '../../typesV2/BaseProduct';
import ProductsGrid from '../../components/constructor/ProductsGrid';
import { ImageProduct } from '../../typesV2/BaseProduct';
import { Decor } from '../../typesV2/BaseProduct';

import Style from './CandleForm.module.css';

interface CandleFormState {
  candle: number;
  numberOfLayerId: number;
  layerColorIds: number[];
  decorId: number;
  smellId: number;
  wickId: number;
  quantity: number;
}

export interface CandleFormProps {
  candleDetailData: CandleDetail;
}

const CandleForm: FC<CandleFormProps> = ({ candleDetailData }) => {
  const [candleDetail, setCandleDetail] = useState<CandleDetail>(candleDetailData);
  const [candleDetailState, setCandleDetailState] = useState<CandleDetail>({
    candle: candleDetailData.candle,
  });

  const handleNumberOfLayerState = (numberOfLayerId: number) => {};
  const handleLayerColorState = (layerColorIds: number[]) => {};
  const handleDecorState = (selectedDecor: ImageProduct) => {
    const newCandleDetailState: CandleDetail = {
      ...candleDetailState,
      decors: [...(candleDetailState?.decors || []), selectedDecor as Decor],
    };
    setCandleDetailState(newCandleDetailState);
  };
  const handleSmellState = (smellId: number) => {};
  const handleWickState = (wickId: number) => {};
  const handleQuantityState = (quantity: number) => {};
  return (
    <>
      <div>
        <ProductsGrid
          title={'Декор'}
          data={candleDetail.decors ? candleDetail.decors : []}
          handleSelectProduct={handleDecorState}
        />
      </div>
    </>
  );
};

export default CandleForm;
