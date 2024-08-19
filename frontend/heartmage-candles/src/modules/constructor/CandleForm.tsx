import { FC, useState, useEffect } from 'react';

import { CandleDetail } from '../../typesV2/constructor/CandleDetail';
import {
  NumberOfLayer,
  Smell,
  ImageProduct,
} from '../../typesV2/shared/BaseProduct';
import TagSelector from '../../components/constructor/TagSelector';
import { TagData } from '../../components/shared/Tag';
import IconArrowLeftLarge from '../../UI/IconArrowLeftLarge';
import { useCandleContext } from '../../contexts/CandleContext';
import { useConstructorContext } from '../../contexts/ConstructorContext';
import { CustomCandle } from '../../typesV2/constructor/CustomCandle';
import { calculateCustomCandlePrice } from '../../helpers/CalculatePrice';
import ProductsGridSelector from '../../components/constructor/ProductsGridSelector';
import { CustomCandleBuilder } from '../../typesV2/constructor/CustomCandleBuilder';
import Button from '../../components/shared/Button';

import Style from './CandleForm.module.css';

export interface CandleFormProps {
  hideCandleForm: () => void;
  isEditing: boolean;
}

const CandleForm: FC<CandleFormProps> = ({ hideCandleForm, isEditing }) => {
  const { customCandles, setCustomCandles } = useConstructorContext();
  const { candle, customCandleBuilder, updateCustomCandleBuilder } =
    useCandleContext();

  const [customCandle, setCustomCandle] = useState<CustomCandle>(
    customCandleBuilder.customCandle,
  );
  const [errors, setErrors] = useState<string[]>(
    customCandleBuilder.getErrors(),
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
          (layerColor) => layerColor.id !== deselectedLayerColor.id,
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
    const newCustomCandle =
      CustomCandleBuilder.checkCustomCandleAgainstCandleDetail(
        customCandleBuilder.getCustomCandle(),
        candle,
      );

    if (!newCustomCandle.isValid || newCustomCandle.errors.length > 0) {
      setErrors(newCustomCandle.errors);
      return;
    } else {
      const buildResult = customCandleBuilder.build();
      if (!buildResult.success || !buildResult.customCandle) {
        setErrors(buildResult.errors);
        return;
      }
      let newCandlesArray = customCandles;
      newCandlesArray.push(buildResult.customCandle);
      setCustomCandles(newCandlesArray);
      updateCustomCandleBuilder();
      hideCandleForm();
    }
  };

  useEffect(() => {
    const currentErrors = customCandleBuilder.customCandle.errors || [];
    setErrors(currentErrors);
  }, [customCandleBuilder.customCandle.errors]);

  useEffect(() => {
    if (isEditing) {
      const currentLayerColors = customCandle.layerColors || [];
      const candleLayerColors = candle.layerColors || [];
      const errors = customCandle.errors;

      const updatedLayerColors = currentLayerColors.filter((color) =>
        candleLayerColors.some((c) => c.id === color.id),
      );

      const newCustomCandle = customCandleBuilder
        .setLayerColor(updatedLayerColors)
        .setErrors(errors)
        .getCustomCandle();

      setCustomCandle(newCustomCandle);
    }
  }, []);

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
        <ProductsGridSelector
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
        <ProductsGridSelector
          title={'Фитиль *'}
          data={candle.wicks ? candle.wicks : []}
          selectedData={
            customCandleBuilder.customCandle.wick
              ? [customCandleBuilder.customCandle.wick]
              : []
          }
          onSelectProduct={handleWickState}
        />
        {errors.length > 0 && (
          <ul className={Style.errorBlock}>
            {errors.map((value, index) => (
              <li key={index} className={Style.errorMessage}>
                <div className={Style.ellipseError}></div>
                <div>{value.toString()}</div>
              </li>
            ))}
          </ul>
        )}
        <div className={Style.configurationInfoBlock}>
          <div className={Style.priceBlock}>
            <span className={Style.priceTitle}>Свеча на</span>
            <span className={Style.price}>
              {Math.round(
                calculateCustomCandlePrice(customCandle),
              ).toLocaleString('ru-RU', { useGrouping: true })}{' '}
              Р
            </span>
          </div>
          <div className={Style.addBtn}>
            <Button
              text={isEditing ? 'Изменить' : 'Добавить'}
              onClick={() => handleAddCandleDetail()}
              className={customCandle.isValid ? Style.valid : Style.invalid}
            />
          </div>
        </div>
      </div>
    </>
  );
};

export default CandleForm;

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
