import { FC, useState } from 'react';

import {
  CandleDetail,
  Decor,
  LayerColor,
  NumberOfLayer,
  PriceProduct,
  Smell,
  Wick,
} from '../../typesV2/BaseProduct';
import ProductsGridSelector from '../../components/constructor/ProductsGridSelector';
import { ImageProduct } from '../../typesV2/BaseProduct';
import TagSelector from '../../components/constructor/TagSelector';
import { TagData } from '../../components/shared/Tag';

import Style from './CandleForm.module.css';

export interface CandleFormProps {
  candleDetailData: CandleDetail;
}

const CandleForm: FC<CandleFormProps> = ({ candleDetailData }) => {
  const [candleDetail, setCandleDetail] = useState<CandleDetail>(candleDetailData);
  const [candleDetailState, setCandleDetailState] = useState<CandleDetail>({
    candle: candleDetailData.candle,
  });
  console.log('CandleForm fassssssss', candleDetailState);

  const handleNumberOfLayerState = (selectedNumberOfLayer: TagData) => {
    console.log('handleNumberOfLayerState', selectedNumberOfLayer);
    const newCandleDetailState: CandleDetail = {
      ...candleDetailState,
      numberOfLayers: [
        ...(candleDetailState?.numberOfLayers || []),
        convertTagDataToNumberOfLayer(selectedNumberOfLayer),
      ],
    };
    setCandleDetailState(newCandleDetailState);
  };

  const handleLayerColorState = (selectedLayerColor: ImageProduct) => {
    const newCandleDetailState: CandleDetail = {
      ...candleDetailState,
      layerColors: [...(candleDetailState?.layerColors || []), selectedLayerColor as LayerColor],
    };
    setCandleDetailState(newCandleDetailState);
  };

  const handleDecorState = (selectedDecor: ImageProduct) => {
    const newCandleDetailState: CandleDetail = {
      ...candleDetailState,
      decors: [...(candleDetailState?.decors || []), selectedDecor as Decor],
    };
    setCandleDetailState(newCandleDetailState);
  };

  const handleSmellState = (selectedSmell: TagData) => {
    const selected = convertTagDataToSmell(selectedSmell, candleDetail);
    if (selected) {
      const newCandleDetailState: CandleDetail = {
        ...candleDetailState,
        smells: [...(candleDetailState?.smells || []), selected],
      };
      setCandleDetailState(newCandleDetailState);
    }
  };

  const handleWickState = (selectedWick: ImageProduct) => {
    const newCandleDetailState: CandleDetail = {
      ...candleDetailState,
      wicks: [...(candleDetailState?.wicks || []), selectedWick as Wick],
    };
    setCandleDetailState(newCandleDetailState);
  };

  return (
    <>
      <div className={Style.candleFrom}>
        <TagSelector
          title="Количество слоев"
          tags={convertNumberOfLayerToTagData(
            candleDetail.numberOfLayers ? candleDetail.numberOfLayers : [],
          )}
          handleSelectTag={handleNumberOfLayerState}
        />
        <ProductsGridSelector
          title={'Восковые слои'}
          data={candleDetail.layerColors ? candleDetail.layerColors : []}
          handleSelectProduct={handleLayerColorState}
        />
        <ProductsGridSelector
          title={'Декоры'}
          data={candleDetail.decors ? candleDetail.decors : []}
          handleSelectProduct={handleDecorState}
        />
        <TagSelector
          title="Запахи"
          tags={convertSmellToTagData(candleDetail.smells ? candleDetail.smells : [])}
          handleSelectTag={handleSmellState}
        />
        <ProductsGridSelector
          title={'Фитили'}
          data={candleDetail.wicks ? candleDetail.wicks : []}
          handleSelectProduct={handleWickState}
        />
      </div>
    </>
  );
};

export default CandleForm;

export function convertNumberOfLayerToTagData(numberOfLayers: NumberOfLayer[]): TagData[] {
  const tagDataArray: TagData[] = [];

  for (let i = 0; i < numberOfLayers.length; i++) {
    const numberOfLayer = numberOfLayers[i];
    const tagData: TagData = {
      id: numberOfLayer.id,
      text: `${numberOfLayer.number}`,
    };

    tagDataArray.push(tagData);
  }

  return tagDataArray;
}

export function convertSmellToTagData(smells: Smell[]): TagData[] {
  const tagDataArray: TagData[] = [];

  for (let i = 0; i < smells.length; i++) {
    const smell = smells[i];
    const tagData: TagData = {
      id: smell.id,
      text: smell.title,
    };

    tagDataArray.push(tagData);
  }

  return tagDataArray;
}

export function convertTagDataToNumberOfLayer(tagData: TagData): NumberOfLayer {
  return {
    id: tagData.id,
    number: parseInt(tagData.text),
  };
}

export function convertTagDataToSmell(
  tagData: TagData,
  candleDetail: CandleDetail,
): Smell | undefined {
  const matchingSmell = candleDetail?.smells?.find(
    (smell) => smell.id === tagData.id && smell.title === tagData.text,
  );
  return matchingSmell;
}
