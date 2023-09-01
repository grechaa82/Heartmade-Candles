import { FC, useState, useEffect } from 'react';

import {
  CandleDetails,
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
  candleDetailData: CandleDetails;
  addCandleDetail: (candleDetail: CandleDetails) => void;
  calculatePriceCandleDetail: (candleDetail: CandleDetails) => void;
}

const CandleForm: FC<CandleFormProps> = ({
  candleDetailData,
  addCandleDetail,
  calculatePriceCandleDetail,
}) => {
  const [candleDetail, setCandleDetail] = useState<CandleDetails>(candleDetailData);
  const [candleDetailState, setCandleDetailState] = useState<CandleDetails>({
    candle: candleDetailData.candle,
  });

  const handleNumberOfLayerState = (selectedNumberOfLayer: TagData) => {
    const newCandleDetailState: CandleDetails = {
      ...candleDetailState,
      layerColors: [],
      numberOfLayers: [convertTagDataToNumberOfLayer(selectedNumberOfLayer)],
    };
    setCandleDetailState(newCandleDetailState);
    calculatePriceCandleDetail(candleDetailState);
  };

  const handleLayerColorState = (selectedLayerColor: ImageProduct) => {
    if (
      candleDetailState.numberOfLayers &&
      candleDetailState.numberOfLayers[0].number === candleDetailState.layerColors?.length
    ) {
      const newLayerColors = [...candleDetailState.layerColors];
      newLayerColors[newLayerColors.length - 1] = selectedLayerColor as LayerColor;
      setCandleDetailState({
        ...candleDetailState,
        layerColors: newLayerColors,
      });
    } else {
      const layerColorToAdd = selectedLayerColor as LayerColor;
      setCandleDetailState({
        ...candleDetailState,
        layerColors: candleDetailState.layerColors
          ? [...candleDetailState.layerColors, layerColorToAdd]
          : [layerColorToAdd],
      });
    }

    calculatePriceCandleDetail(candleDetailState);
  };

  const handleDeselectLayerColorState = (deselectedLayerColor: ImageProduct) => {
    const updatedLayerColors = candleDetailState.layerColors?.filter(
      (layerColor) => layerColor !== deselectedLayerColor,
    );
    setCandleDetailState({
      ...candleDetailState,
      layerColors: updatedLayerColors,
    });
    calculatePriceCandleDetail(candleDetailState);
  };

  const handleDecorState = (selectedDecor: ImageProduct) => {
    const newCandleDetailState: CandleDetails = {
      ...candleDetailState,
      decors: [selectedDecor as Decor],
    };
    setCandleDetailState(newCandleDetailState);
    calculatePriceCandleDetail(candleDetailState);
  };

  const handleDeselectDecorState = (deselectedDecor: ImageProduct) => {
    const updatedDecors = candleDetailState.decors?.filter((decor) => decor !== deselectedDecor);
    setCandleDetailState({ ...candleDetailState, decors: updatedDecors });
    calculatePriceCandleDetail(candleDetailState);
  };

  const handleSmellState = (selectedSmell: TagData) => {
    const selected = convertTagDataToSmell(selectedSmell, candleDetail);
    if (selected) {
      const newCandleDetailState: CandleDetails = {
        ...candleDetailState,
        smells: [selected],
      };
      setCandleDetailState(newCandleDetailState);
      calculatePriceCandleDetail(newCandleDetailState);
    }
  };
  const handleWickState = (selectedWick: ImageProduct) => {
    const newCandleDetailState: CandleDetails = {
      ...candleDetailState,
      wicks: [selectedWick as Wick],
    };
    setCandleDetailState(newCandleDetailState);
    calculatePriceCandleDetail(candleDetailState);
  };

  const handleAddCandleDetail = () => {
    calculatePriceCandleDetail(candleDetailState);
    addCandleDetail(candleDetailState);
  };

  useEffect(() => {
    calculatePriceCandleDetail(candleDetailState);
  }, [candleDetailState]);

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
          title={'Цвета слоев'}
          data={candleDetail.layerColors ? candleDetail.layerColors : []}
          selectedData={candleDetailState.layerColors ? candleDetailState.layerColors : []}
          onSelectProduct={handleLayerColorState}
          onDeselectProduct={handleDeselectLayerColorState}
          withIndex={true}
        />
        <ProductsGridSelector
          title={'Декор'}
          data={candleDetail.decors ? candleDetail.decors : []}
          selectedData={candleDetailState.decors ? candleDetailState.decors : []}
          onSelectProduct={handleDecorState}
          onDeselectProduct={handleDeselectDecorState}
        />
        <TagSelector
          title="Запах"
          data={convertSmellToTagData(candleDetail.smells ? candleDetail.smells : [])}
          selectedData={convertSmellToTagData(
            candleDetailState.smells ? candleDetailState.smells : [],
          )}
          onSelectTag={handleSmellState}
        />
        <ProductsGridSelector
          title={'Фитиль'}
          data={candleDetail.wicks ? candleDetail.wicks : []}
          selectedData={candleDetailState.wicks ? candleDetailState.wicks : []}
          onSelectProduct={handleWickState}
        />
        <div className={Style.addBtn}>
          <ButtonWithIcon
            color="#2E67EA"
            text="Добавить свечу"
            icon={IconPlusLarge}
            onClick={() => handleAddCandleDetail()}
          />
        </div>
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
  candleDetail: CandleDetails,
): Smell | undefined {
  const matchingSmell = candleDetail?.smells?.find((smell) => smell.id === tagData.id);
  return matchingSmell;
}
