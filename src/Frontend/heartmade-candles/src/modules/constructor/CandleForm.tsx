import { FC, useState } from 'react';

import {
  CandleDetail,
  Decor,
  LayerColor,
  NumberOfLayer,
  Smell,
  Wick,
} from '../../typesV2/BaseProduct';
import ProductsGridSelector from '../../components/constructor/ProductsGridSelector';
import { ImageProduct } from '../../typesV2/BaseProduct';
import TagSelector from '../../components/constructor/TagSelector';
import { TagData } from '../../components/shared/Tag';
import ButtonWithIcon from '../../components/shared/ButtonWithIcon';
import IconPlusLarge from '../../UI/IconPlusLarge';

import Style from './CandleForm.module.css';

export interface CandleFormProps {
  candleDetailData: CandleDetail;
  addCandleDetail: (candleDetail: CandleDetail, price: number) => void;
}

const CandleForm: FC<CandleFormProps> = ({ candleDetailData, addCandleDetail }) => {
  const [candleDetail, setCandleDetail] = useState<CandleDetail>(candleDetailData);
  const [candleDetailState, setCandleDetailState] = useState<CandleDetail>({
    candle: candleDetailData.candle,
  });
  const [price, setPrice] = useState<number>();

  const handleNumberOfLayerState = (selectedNumberOfLayer: TagData) => {
    const newCandleDetailState: CandleDetail = {
      ...candleDetailState,
      layerColors: [],
      numberOfLayers: [convertTagDataToNumberOfLayer(selectedNumberOfLayer)],
    };
    setCandleDetailState(newCandleDetailState);
  };

  const handleLayerColorState = (selectedLayerColor: ImageProduct) => {
    const numberOfLayers = candleDetailState.numberOfLayers;
    const layerColors = candleDetailState.layerColors || [];

    if (numberOfLayers && numberOfLayers[0]?.number === layerColors.length) {
      const newLayerColors = [...layerColors];
      newLayerColors[newLayerColors.length - 1] = selectedLayerColor as LayerColor;
      setCandleDetailState({ ...candleDetailState, layerColors: newLayerColors });
    } else {
      setCandleDetailState({
        ...candleDetailState,
        layerColors: [...layerColors, selectedLayerColor as LayerColor],
      });
    }
  };

  const handleDeselectLayerColorState = (deselectedLayerColor: ImageProduct) => {
    const updatedLayerColors = candleDetailState.layerColors?.filter(
      (layerColor) => layerColor !== deselectedLayerColor,
    );
    setCandleDetailState({ ...candleDetailState, layerColors: updatedLayerColors });
  };

  const handleDecorState = (selectedDecor: ImageProduct) => {
    const newCandleDetailState: CandleDetail = {
      ...candleDetailState,
      decors: [selectedDecor as Decor],
    };
    setCandleDetailState(newCandleDetailState);
  };

  const handleDeselectDecorState = (deselectedDecor: ImageProduct) => {
    const updatedDecors = candleDetailState.decors?.filter((decor) => decor !== deselectedDecor);
    setCandleDetailState({ ...candleDetailState, decors: updatedDecors });
  };

  const handleSmellState = (selectedSmell: TagData) => {
    const selected = convertTagDataToSmell(selectedSmell, candleDetail);
    if (selected) {
      const newCandleDetailState: CandleDetail = {
        ...candleDetailState,
        smells: [selected],
      };
      setCandleDetailState(newCandleDetailState);
    }
  };

  const handleWickState = (selectedWick: ImageProduct) => {
    const newCandleDetailState: CandleDetail = {
      ...candleDetailState,
      wicks: [selectedWick as Wick],
    };
    setCandleDetailState(newCandleDetailState);
  };

  const calculatePrice = (): number => {
    let totalPrice: number = candleDetailState.candle.price;

    if (candleDetailState.decors) {
      for (const decor of candleDetailState.decors) {
        totalPrice += decor.price;
      }
    }

    if (candleDetailState.numberOfLayers && candleDetailState.layerColors) {
      const numberOfLayer = candleDetailState.numberOfLayers[0].number;
      const weightGrams = candleDetailState.candle.weightGrams;
      const gramsInLayer = weightGrams / numberOfLayer;

      for (const layerColor of candleDetailState.layerColors) {
        totalPrice += gramsInLayer * layerColor.price;
      }
    }

    if (candleDetailState.smells) {
      for (const smell of candleDetailState.smells) {
        totalPrice += smell.price;
      }
    }

    if (candleDetailState.wicks) {
      for (const wick of candleDetailState.wicks) {
        totalPrice += wick.price;
      }
    }

    setPrice(totalPrice);
    return totalPrice;
  };

  const handleAddCandleDetail = () => {
    const price = calculatePrice();
    addCandleDetail(candleDetailState, price);
  };

  return (
    <>
      <div className={Style.candleFrom}>
        <TagSelector
          title="Количество слоев"
          data={convertNumberOfLayerToTagData(
            candleDetail.numberOfLayers ? candleDetail.numberOfLayers : [],
          )}
          selectedData={convertNumberOfLayerToTagData(
            candleDetailState.numberOfLayers ? candleDetailState.numberOfLayers : [],
          )}
          onSelectTag={handleNumberOfLayerState}
        />
        <ProductsGridSelector
          title={'Восковые слои'}
          data={candleDetail.layerColors ? candleDetail.layerColors : []}
          selectedData={candleDetailState.layerColors ? candleDetailState.layerColors : []}
          onSelectProduct={handleLayerColorState}
          onDeselectProduct={handleDeselectLayerColorState}
          withIndex={true}
        />
        <ProductsGridSelector
          title={'Декоры'}
          data={candleDetail.decors ? candleDetail.decors : []}
          selectedData={candleDetailState.decors ? candleDetailState.decors : []}
          onSelectProduct={handleDecorState}
          onDeselectProduct={handleDeselectDecorState}
        />
        <TagSelector
          title="Запахи"
          data={convertSmellToTagData(candleDetail.smells ? candleDetail.smells : [])}
          selectedData={convertSmellToTagData(
            candleDetailState.smells ? candleDetailState.smells : [],
          )}
          onSelectTag={handleSmellState}
        />
        <ProductsGridSelector
          title={'Фитили'}
          data={candleDetail.wicks ? candleDetail.wicks : []}
          selectedData={candleDetailState.wicks ? candleDetailState.wicks : []}
          onSelectProduct={handleWickState}
        />
        <ButtonWithIcon
          text="Добавить свечу"
          icon={IconPlusLarge}
          onClick={() => handleAddCandleDetail()}
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
      text: smell.title + ' ' + smell.price + ' р',
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
  const splitTitle = tagData.text.split(' ');
  const matchingSmell = candleDetail?.smells?.find(
    (smell) => smell.id === tagData.id && smell.title === splitTitle[0],
  );
  return matchingSmell;
}
