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
import { useCandleContext } from '../../contexts/CandleContext';

import Style from './CandleForm.module.css';
import { useConstructorContext } from '../../contexts/ConstructorContext';

export interface CandleFormProps {}

const CandleFormV2: FC<CandleFormProps> = ({}) => {
  const { candlesByType, configuredCandles, totalPrice, setConfiguredCandles } =
    useConstructorContext();
  const {
    candle,
    configuredCandle,
    priceConfiguredCandle,
    setCandle,
    setConfiguredCandle,
    fetchCandleById,
    customCandleBuilder,
    updateCustomCandle,
  } = useCandleContext();

  const [errors, setErrors] = useState<string[]>(
    customCandleBuilder.getErrors(),
  );
  const customCandle = customCandleBuilder.getCustomCandle();

  const handleNumberOfLayerState = (selectedNumberOfLayer: TagData) => {
    updateCustomCandle((builder) =>
      builder.setNumberOfLayer(
        convertTagDataToNumberOfLayer(selectedNumberOfLayer),
      ),
    );
  };

  const handleDecorState = (selectedDecor: ImageProduct) => {
    updateCustomCandle((builder) => builder.setDecor(selectedDecor));
  };

  const handleDeselectDecorState = (deselectedDecor: ImageProduct) => {
    customCandleBuilder.setDecor(undefined);
  };

  const handleAddCandleDetail = () => {};

  useEffect(() => {
    setErrors(customCandleBuilder.getErrors());
  }, [customCandleBuilder]);

  return (
    <>
      <div className={Style.candleFrom}>
        <div className={Style.mainInfo}>
          {/* <button
            className={Style.hideCandleForm}
            onClick={() => hideCandleForm()}
          >
            <IconArrowLeftLarge />
          </button> */}
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
        <div>
          {customCandleBuilder &&
            customCandleBuilder.errors &&
            customCandleBuilder.errors.map((value, index) => (
              <p key={index}>{value.toString()}</p>
            ))}
        </div>
        <div className={Style.configurationInfoBlock}>
          <div className={Style.priceBlock}>
            <span className={Style.priceTitle}>Свеча на</span>
            <span className={Style.price}>{priceConfiguredCandle} р</span>
          </div>
          <div className={Style.addBtn}>
            <ButtonWithIcon
              color="#2E67EA"
              text="Добавить"
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
