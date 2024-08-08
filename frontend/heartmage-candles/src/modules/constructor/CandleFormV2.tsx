import { FC, useState, useEffect } from 'react';

import { CandleDetail } from '../../typesV2/constructor/CandleDetail';
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
import { useCandleContext } from '../../contexts/CandleContext';
import { useConstructorContext } from '../../contexts/ConstructorContext';
import { CustomCandle } from '../../typesV2/constructor/CustomCandle';
import { calculateCustomCandlePrice } from '../../helpers/CalculatePrice';

import Style from './CandleForm.module.css';
import ProductsGridSelectorV2 from '../../components/constructor/ProductsGridSelectorV2';

export interface CandleFormProps {
  hideCandleForm: () => void;
  isEditing: boolean;
}

const CandleFormV2: FC<CandleFormProps> = ({ hideCandleForm, isEditing }) => {
  const { customCandles, setCustomCandles } = useConstructorContext();
  const { candle, customCandleBuilder, updateCustomCandleBuilder } =
    useCandleContext();

  const [errors, setErrors] = useState<string[]>(
    customCandleBuilder.getErrors(),
  );
  const [customCandle, setCustomCandle] = useState<CustomCandle>(
    customCandleBuilder.getCustomCandle(),
  );

  const handleNumberOfLayerState = (selectedNumberOfLayer: TagData) => {
    setCustomCandle(
      customCandleBuilder
        .setNumberOfLayer(convertTagDataToNumberOfLayer(selectedNumberOfLayer))
        .getCustomCandle(),
    );
  };

  const handleLayerColorState = (selectedLayerColor: ImageProduct) => {
    const newCustomCandle = customCandleBuilder
      .addLayerColor(selectedLayerColor)
      .getCustomCandle();
    setCustomCandle(newCustomCandle);
  };

  const handleDeselectLayerColorState = (
    deselectedLayerColor: ImageProduct,
  ) => {
    const updatedLayerColors =
      customCandleBuilder
        .getCustomCandle()
        .layerColors?.filter(
          (layerColor) => layerColor !== deselectedLayerColor,
        ) || [];

    setCustomCandle(
      customCandleBuilder.setLayerColor(updatedLayerColors).getCustomCandle(),
    );
  };

  const handleDecorState = (selectedDecor: ImageProduct) => {
    setCustomCandle(
      customCandleBuilder.setDecor(selectedDecor).getCustomCandle(),
    );
  };

  const handleDeselectDecorState = (deselectedDecor: ImageProduct) => {
    setCustomCandle(customCandleBuilder.setDecor(null).getCustomCandle());
  };

  const handleSmellState = (selectedSmell: TagData) => {
    setCustomCandle(
      customCandleBuilder
        .setSmell(convertTagDataToSmell(selectedSmell, candle))
        .getCustomCandle(),
    );
  };

  const handleDeselectSmellState = (deselectedSmell: TagData) => {
    setCustomCandle(customCandleBuilder.setSmell(null).getCustomCandle());
  };

  const handleWickState = (selectedWick: ImageProduct) => {
    setCustomCandle(
      customCandleBuilder.setWick(selectedWick).getCustomCandle(),
    );
  };

  const handleAddCandleDetail = () => {
    if (customCandleBuilder.checkCustomCandleAgainstCandleDetail(candle)) {
      const buildResult = customCandleBuilder.build();
      if (!buildResult.success || !buildResult.customCandle) {
        setErrors(buildResult.errors);
        return;
      }
      let newCandlesArray = customCandles;
      newCandlesArray.push(buildResult.customCandle);
      setCustomCandles(newCandlesArray);
      updateCustomCandleBuilder();
    }
    return;
  };

  useEffect(() => {
    setErrors(customCandleBuilder.getErrors());
  }, [
    customCandleBuilder,
    customCandleBuilder.getCustomCandle(),
    customCandleBuilder.errors,
  ]);

  return (
    <>
      <div className={Style.candleFrom}>
        <div className={Style.mainInfo}>
          {!isEditing && (
            <button
              className={Style.hideCandleForm}
              onClick={() => hideCandleForm()}
            >
              <IconArrowLeftLarge />
            </button>
          )}
          <p className={Style.title}>{candle.candle.title}</p>
        </div>
        <TagSelector
          title="Количество слоев *"
          data={convertNumberOfLayersToTagData(candle.numberOfLayers)}
          selectedData={
            customCandle?.numberOfLayer
              ? [
                  convertNumberOfLayerToTagData(
                    customCandleBuilder.customCandle.numberOfLayer,
                  ),
                ]
              : []
          }
          onSelectTag={handleNumberOfLayerState}
        />
        <ProductsGridSelectorV2
          title={'Цвета слоев *'}
          data={candle.layerColors ? candle.layerColors : []}
          selectedData={
            customCandle?.layerColors
              ? customCandle.layerColors
              : customCandleBuilder?.customCandle.layerColors
              ? customCandleBuilder.customCandle.layerColors
              : []
          }
          onSelectProduct={handleLayerColorState}
          onDeselectProduct={handleDeselectLayerColorState}
          withIndex={true}
        />
        {candle.decors && candle.decors.length > 0 && (
          <ProductsGridSelector
            title={'Декор'}
            data={candle.decors}
            selectedData={
              customCandle?.decor !== undefined
                ? [customCandleBuilder.customCandle.decor]
                : []
            }
            onSelectProduct={handleDecorState}
            onDeselectProduct={handleDeselectDecorState}
          />
        )}
        {candle.smells && candle.smells.length > 0 && (
          <TagSelector
            title="Запах"
            data={convertSmellsToTagData(candle.smells)}
            selectedData={
              customCandle?.smell
                ? [
                    convertSmellToTagData(
                      customCandleBuilder.customCandle.smell,
                    ),
                  ]
                : []
            }
            onSelectTag={handleSmellState}
            onDeselectTag={handleDeselectSmellState}
          />
        )}
        <ProductsGridSelectorV2
          title={'Фитиль *'}
          data={candle.wicks ? candle.wicks : []}
          selectedData={
            customCandle.wick ? [customCandleBuilder.customCandle.wick] : []
          }
          onSelectProduct={handleWickState}
        />
        <div>
          {errors.map((value, index) => (
            <p key={index}>{value.toString()}</p>
          ))}
        </div>
        <div className={Style.configurationInfoBlock}>
          <div className={Style.priceBlock}>
            <span className={Style.priceTitle}>Свеча на</span>
            <span className={Style.price}>
              {calculateCustomCandlePrice(customCandle)} р
            </span>
          </div>
          <div className={Style.addBtn}>
            <ButtonWithIcon
              color="#2E67EA"
              text={isEditing ? 'Изменить' : 'Добавить'}
              icon={IconPlusLarge}
              onClick={() => handleAddCandleDetail()}
            />
          </div>
        </div>
      </div>
    </>
  );
};

export default CandleFormV2;

export function convertNumberOfLayerToTagData(
  numberOfLayer: NumberOfLayer,
): TagData {
  const tagData: TagData = {
    id: numberOfLayer.id,
    text: `${numberOfLayer.number}`,
  };

  return tagData;
}

export function convertNumberOfLayersToTagData(
  numberOfLayers: NumberOfLayer[] | undefined,
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
  candleDetail: CandleDetail,
): Smell | undefined {
  const matchingSmell = candleDetail?.smells?.find(
    (smell) => smell.id === tagData.id,
  );
  return matchingSmell;
}
