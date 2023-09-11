import { FC, useState, useEffect } from 'react';

import {
  CandleDetails,
  Decor,
  LayerColor,
  NumberOfLayer,
  Smell,
  Wick,
  ImageProduct,
} from '../../typesV2/BaseProduct';
import ProductsGridSelector from '../../components/constructor/ProductsGridSelector';
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
  const [candleDetailState, setCandleDetailState] = useState<CandleDetails>();

  useEffect(() => {
    const newCandleDetailState: CandleDetails = {
      candle: candleDetailData.candle,
      numberOfLayers:
        candleDetailData.numberOfLayers?.length === 1
          ? [candleDetailData.numberOfLayers[0]]
          : undefined,
      wicks: candleDetailData.wicks?.length === 1 ? [candleDetailData.wicks[0]] : undefined,
    };

    setCandleDetailState(newCandleDetailState);
  }, []);

  const handleNumberOfLayerState = (selectedNumberOfLayer: TagData) => {
    if (candleDetailState) {
      const newCandleDetailState: CandleDetails = {
        ...candleDetailState,
        layerColors: [],
        numberOfLayers: [convertTagDataToNumberOfLayer(selectedNumberOfLayer)],
      };
      setCandleDetailState(newCandleDetailState);
    }
  };

  const handleLayerColorState = (selectedLayerColor: ImageProduct) => {
    if (
      candleDetailState &&
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
      if (candleDetailState) {
        setCandleDetailState({
          ...candleDetailState,
          layerColors: candleDetailState?.layerColors
            ? [...candleDetailState?.layerColors, layerColorToAdd]
            : [layerColorToAdd],
        });
      }
    }
  };

  const handleDeselectLayerColorState = (deselectedLayerColor: ImageProduct) => {
    if (candleDetailState) {
      const updatedLayerColors = candleDetailState.layerColors?.filter(
        (layerColor) => layerColor !== deselectedLayerColor,
      );
      setCandleDetailState({
        ...candleDetailState,
        layerColors: updatedLayerColors,
      });
    }
  };

  const handleDecorState = (selectedDecor: ImageProduct) => {
    if (candleDetailState) {
      const newCandleDetailState: CandleDetails = {
        ...candleDetailState,
        decors: [selectedDecor as Decor],
      };
      setCandleDetailState(newCandleDetailState);
    }
  };

  const handleDeselectDecorState = (deselectedDecor: ImageProduct) => {
    if (candleDetailState) {
      const updatedDecors = candleDetailState.decors?.filter((decor) => decor !== deselectedDecor);
      setCandleDetailState({ ...candleDetailState, decors: updatedDecors });
    }
  };

  const handleSmellState = (selectedSmell: TagData) => {
    if (candleDetailState) {
      const selected = convertTagDataToSmell(selectedSmell, candleDetailData);
      if (selected) {
        const newCandleDetailState: CandleDetails = {
          ...candleDetailState,
          smells: [selected],
        };
        setCandleDetailState(newCandleDetailState);
      }
    }
  };
  const handleWickState = (selectedWick: ImageProduct) => {
    if (candleDetailState) {
      const newCandleDetailState: CandleDetails = {
        ...candleDetailState,
        wicks: [selectedWick as Wick],
      };
      setCandleDetailState(newCandleDetailState);
    }
  };

  const handleAddCandleDetail = () => {
    if (candleDetailState) {
      calculatePriceCandleDetail(candleDetailState);
      addCandleDetail(candleDetailState);
    }
  };

  useEffect(() => {
    if (candleDetailState) {
      calculatePriceCandleDetail(candleDetailState);
    }
  }, [candleDetailState]);

  return (
    <>
      <div className={Style.candleFrom}>
        <TagSelector
          title="Количество слоев *"
          data={convertNumberOfLayerToTagData(
            candleDetailData.numberOfLayers ? candleDetailData.numberOfLayers : [],
          )}
          selectedData={convertNumberOfLayerToTagData(
            candleDetailState?.numberOfLayers ? candleDetailState.numberOfLayers : [],
          )}
          onSelectTag={handleNumberOfLayerState}
        />
        <ProductsGridSelector
          title={'Цвета слоев *'}
          data={candleDetailData.layerColors ? candleDetailData.layerColors : []}
          selectedData={candleDetailState?.layerColors ? candleDetailState.layerColors : []}
          onSelectProduct={handleLayerColorState}
          onDeselectProduct={handleDeselectLayerColorState}
          withIndex={true}
        />
        {candleDetailData.decors && candleDetailData.decors.length > 0 && (
          <ProductsGridSelector
            title={'Декор'}
            data={candleDetailData.decors}
            selectedData={candleDetailState?.decors ? candleDetailState.decors : []}
            onSelectProduct={handleDecorState}
            onDeselectProduct={handleDeselectDecorState}
          />
        )}
        {candleDetailData.smells && candleDetailData.smells.length > 0 && (
          <TagSelector
            title="Запах"
            data={convertSmellToTagData(candleDetailData.smells)}
            selectedData={convertSmellToTagData(
              candleDetailState?.smells ? candleDetailState.smells : [],
            )}
            onSelectTag={handleSmellState}
          />
        )}
        <ProductsGridSelector
          title={'Фитиль *'}
          data={candleDetailData.wicks ? candleDetailData.wicks : []}
          selectedData={candleDetailState?.wicks ? candleDetailState.wicks : []}
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
