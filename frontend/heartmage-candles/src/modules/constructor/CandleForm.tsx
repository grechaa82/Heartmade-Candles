import { FC, useState, useEffect } from 'react';

import { CandleDetail } from '../../typesV2/constructor/CandleDetail';
import { ConfiguredCandleDetail } from '../../typesV2/constructor/ConfiguredCandleDetail';
import {
  NumberOfLayer,
  Smell,
  ImageProduct,
} from '../../typesV2/shared/BaseProduct';
import ProductsGridSelector from '../../components/constructor/ProductsGridSelector';
import TagSelector from '../../components/constructor/TagSelector';
import { TagData } from '../../components/shared/Tag';
import ButtonWithIcon from '../../components/shared/ButtonWithIcon';
import IconPlusLarge from '../../UI/IconPlusLarge';
import IconArrowLeftLarge from '../../UI/IconArrowLeftLarge';

import Style from './CandleForm.module.css';

export interface CandleFormProps {
  candleDetail: CandleDetail;
  addCandleDetail: (configuredCandleDetail: ConfiguredCandleDetail) => void;
  calculatePriceCandleDetail: (
    configuredCandleDetail: ConfiguredCandleDetail
  ) => void;
  hideCandleForm: () => void;
}

const CandleForm: FC<CandleFormProps> = ({
  candleDetail,
  addCandleDetail,
  calculatePriceCandleDetail,
  hideCandleForm,
}) => {
  const [configuredCandleDetailState, setConfiguredCandleDetailState] =
    useState<ConfiguredCandleDetail>(
      new ConfiguredCandleDetail(
        candleDetail.candle,
        1,
        candleDetail.numberOfLayers.length === 1
          ? candleDetail.numberOfLayers[0]
          : undefined,
        [],
        candleDetail.wicks.length === 1 ? candleDetail.wicks[0] : undefined,
        undefined,
        undefined
      )
    );

  const handleNumberOfLayerState = (selectedNumberOfLayer: TagData) => {
    setConfiguredCandleDetailState(
      new ConfiguredCandleDetail(
        configuredCandleDetailState.candle,
        configuredCandleDetailState.quantity,
        convertTagDataToNumberOfLayer(selectedNumberOfLayer),
        [],
        configuredCandleDetailState.wick,
        configuredCandleDetailState.decor,
        configuredCandleDetailState.smell
      )
    );
  };

  const handleLayerColorState = (selectedLayerColor: ImageProduct) => {
    if (
      configuredCandleDetailState.layerColors &&
      configuredCandleDetailState.numberOfLayer?.number ===
        configuredCandleDetailState.layerColors?.length
    ) {
      const newLayerColors = configuredCandleDetailState.layerColors;
      newLayerColors[newLayerColors.length - 1] = selectedLayerColor;

      setConfiguredCandleDetailState(
        new ConfiguredCandleDetail(
          configuredCandleDetailState.candle,
          configuredCandleDetailState.quantity,
          configuredCandleDetailState.numberOfLayer,
          newLayerColors,
          configuredCandleDetailState.wick,
          configuredCandleDetailState.decor,
          configuredCandleDetailState.smell
        )
      );

      return;
    }

    const layerColorToAdd = configuredCandleDetailState.layerColors
      ? configuredCandleDetailState.layerColors
      : [];
    layerColorToAdd.push(selectedLayerColor);

    setConfiguredCandleDetailState(
      new ConfiguredCandleDetail(
        configuredCandleDetailState.candle,
        configuredCandleDetailState.quantity,
        configuredCandleDetailState.numberOfLayer,
        layerColorToAdd,
        configuredCandleDetailState.wick,
        configuredCandleDetailState.decor,
        configuredCandleDetailState.smell
      )
    );
  };

  const handleDeselectLayerColorState = (
    deselectedLayerColor: ImageProduct
  ) => {
    const updatedLayerColors = configuredCandleDetailState.layerColors?.filter(
      (layerColor) => layerColor !== deselectedLayerColor
    );

    setConfiguredCandleDetailState(
      new ConfiguredCandleDetail(
        configuredCandleDetailState.candle,
        configuredCandleDetailState.quantity,
        configuredCandleDetailState.numberOfLayer,
        updatedLayerColors,
        configuredCandleDetailState.wick,
        configuredCandleDetailState.decor,
        configuredCandleDetailState.smell
      )
    );
  };

  const handleDecorState = (selectedDecor: ImageProduct) => {
    setConfiguredCandleDetailState(
      new ConfiguredCandleDetail(
        configuredCandleDetailState.candle,
        configuredCandleDetailState.quantity,
        configuredCandleDetailState.numberOfLayer,
        configuredCandleDetailState.layerColors,
        configuredCandleDetailState.wick,
        selectedDecor,
        configuredCandleDetailState.smell
      )
    );
  };

  const handleDeselectDecorState = (deselectedDecor: ImageProduct) => {
    setConfiguredCandleDetailState(
      new ConfiguredCandleDetail(
        configuredCandleDetailState.candle,
        configuredCandleDetailState.quantity,
        configuredCandleDetailState.numberOfLayer,
        configuredCandleDetailState.layerColors,
        configuredCandleDetailState.wick,
        undefined,
        configuredCandleDetailState.smell
      )
    );
  };

  const handleSmellState = (selectedSmell: TagData) => {
    setConfiguredCandleDetailState(
      new ConfiguredCandleDetail(
        configuredCandleDetailState.candle,
        configuredCandleDetailState.quantity,
        configuredCandleDetailState.numberOfLayer,
        configuredCandleDetailState.layerColors,
        configuredCandleDetailState.wick,
        configuredCandleDetailState.decor,
        convertTagDataToSmell(selectedSmell, candleDetail)
      )
    );
  };

  const handleDeselectSmellState = (deselectedSmell: TagData) => {
    setConfiguredCandleDetailState(
      new ConfiguredCandleDetail(
        configuredCandleDetailState.candle,
        configuredCandleDetailState.quantity,
        configuredCandleDetailState.numberOfLayer,
        configuredCandleDetailState.layerColors,
        configuredCandleDetailState.wick,
        configuredCandleDetailState.decor,
        undefined
      )
    );
  };

  const handleWickState = (selectedWick: ImageProduct) => {
    setConfiguredCandleDetailState(
      new ConfiguredCandleDetail(
        configuredCandleDetailState.candle,
        configuredCandleDetailState.quantity,
        configuredCandleDetailState.numberOfLayer,
        configuredCandleDetailState.layerColors,
        selectedWick,
        configuredCandleDetailState.decor,
        configuredCandleDetailState.smell
      )
    );
  };

  const handleAddCandleDetail = () => {
    addCandleDetail(configuredCandleDetailState);
  };

  useEffect(() => {
    calculatePriceCandleDetail(configuredCandleDetailState);
  }, [configuredCandleDetailState]);

  return (
    <>
      <div className={Style.candleFrom}>
        <div className={Style.mainInfo}>
          <button
            className={Style.hideCandleForm}
            onClick={() => hideCandleForm()}
          >
            <IconArrowLeftLarge />
          </button>
          <p>{configuredCandleDetailState.candle.title}</p>
        </div>
        <TagSelector
          title="Количество слоев *"
          data={convertNumberOfLayersToTagData(candleDetail.numberOfLayers)}
          selectedData={
            configuredCandleDetailState.numberOfLayer
              ? [
                  convertNumberOfLayerToTagData(
                    configuredCandleDetailState.numberOfLayer
                  ),
                ]
              : []
          }
          onSelectTag={handleNumberOfLayerState}
        />
        <ProductsGridSelector
          title={'Цвета слоев *'}
          data={candleDetail.layerColors ? candleDetail.layerColors : []}
          selectedData={
            configuredCandleDetailState?.layerColors
              ? configuredCandleDetailState.layerColors
              : []
          }
          onSelectProduct={handleLayerColorState}
          onDeselectProduct={handleDeselectLayerColorState}
          withIndex={true}
        />
        {candleDetail.decors && candleDetail.decors.length > 0 && (
          <ProductsGridSelector
            title={'Декор'}
            data={candleDetail.decors}
            selectedData={
              configuredCandleDetailState.decor
                ? [configuredCandleDetailState.decor]
                : []
            }
            onSelectProduct={handleDecorState}
            onDeselectProduct={handleDeselectDecorState}
          />
        )}
        {candleDetail.smells && candleDetail.smells.length > 0 && (
          <TagSelector
            title="Запах"
            data={convertSmellsToTagData(candleDetail.smells)}
            selectedData={
              configuredCandleDetailState.smell
                ? [convertSmellToTagData(configuredCandleDetailState.smell)]
                : []
            }
            onSelectTag={handleSmellState}
            onDeselectTag={handleDeselectSmellState}
          />
        )}
        <ProductsGridSelector
          title={'Фитиль *'}
          data={candleDetail.wicks ? candleDetail.wicks : []}
          selectedData={
            configuredCandleDetailState.wick
              ? [configuredCandleDetailState.wick]
              : []
          }
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

export function convertNumberOfLayerToTagData(
  numberOfLayer: NumberOfLayer
): TagData {
  const tagData: TagData = {
    id: numberOfLayer.id,
    text: `${numberOfLayer.number}`,
  };

  return tagData;
}

export function convertNumberOfLayersToTagData(
  numberOfLayers: NumberOfLayer[] | undefined
): TagData[] {
  if (!numberOfLayers) return [];

  const tagDataArray: TagData[] = [];

  for (let i = 0; i < numberOfLayers.length; i++) {
    const numberOfLayer = numberOfLayers[i];
    const tagData: TagData = convertNumberOfLayerToTagData(numberOfLayer);

    tagDataArray.push(tagData);
  }

  return tagDataArray;
}

export function convertSmellToTagData(smell: Smell): TagData {
  const tagData: TagData = {
    id: smell.id,
    text: smell.title + ' ' + smell.price + ' р',
  };

  return tagData;
}

export function convertSmellsToTagData(smells: Smell[] | undefined): TagData[] {
  if (!smells) return [];

  const tagDataArray: TagData[] = [];

  for (let i = 0; i < smells.length; i++) {
    const smell = smells[i];
    const tagData: TagData = convertSmellToTagData(smell);

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
  candleDetail: CandleDetail
): Smell | undefined {
  const matchingSmell = candleDetail?.smells?.find(
    (smell) => smell.id === tagData.id
  );
  return matchingSmell;
}
