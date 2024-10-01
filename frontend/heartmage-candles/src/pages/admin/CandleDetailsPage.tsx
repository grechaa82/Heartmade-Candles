import { FC, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

import MainInfoCandle from '../../modules/admin/MainInfoCandle';
import ProductsGrid from '../../modules/admin/ProductsGrid';
import TagsGrid from '../../modules/admin/TagsGrid';
import { NumberOfLayer } from '../../types/NumberOfLayer';
import { TagData } from '../../components/shared/Tag';
import AddProductPopUp from '../../components/admin/PopUp/AddProductPopUp';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useCandleByIdQuery from '../../hooks/useCandleByIdQuery';
import useNumberOfLayersQuery from '../../hooks/useNumberOfLayersQuery';
import useTypeCandlesQuery from '../../hooks/useTypeCandlesQuery';
import useSmellsQuery from '../../hooks/useSmellsQuery';
import useDecorQuery from '../../hooks/useDecorQuery';
import useLayerColorsQuery from '../../hooks/useLayerColorsQuery';
import useWicksQuery from '../../hooks/useWicksQuery';

import { TypeCandlesApi } from '../../services/TypeCandlesApi';

import Style from './CandleDetailsPage.module.css';

type CandleDetailsParams = {
  id: string;
};

const CandleDetailsPage: FC = () => {
  const { id } = useParams<CandleDetailsParams>();
  const {
    data: candleDetailData,
    isLoading,
    updateCandle,
    updateCandleNumberOfLayers,
    updateCandleDecors,
    updateCandleLayerColors,
    updateCandleSmells,
    updateCandleWicks,
  } = useCandleByIdQuery(id);
  const { data: typeCandleData } = useTypeCandlesQuery();
  const { data: numberOfLayersData } = useNumberOfLayersQuery();
  const { data: decorsData } = useDecorQuery();
  const { data: layerColorsData } = useLayerColorsQuery();
  const { data: smellsData } = useSmellsQuery();
  const { data: wicksData } = useWicksQuery();
  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const handleOnSaveNumberOfLayers = (
    updatedNumberOfLayers: TagData[],
  ): void => {
    const numberOfLayers: NumberOfLayer[] = updatedNumberOfLayers.map(
      (tagData) => ({
        id: tagData.id,
        number: parseInt(tagData.text),
      }),
    );

    updateCandleNumberOfLayers(numberOfLayers);
  };

  if (isLoading) {
    return <div>...Loading</div>;
  }

  return (
    <>
      <div className="candles">
        {candleDetailData && (
          <MainInfoCandle
            data={candleDetailData.candle}
            allTypeCandle={typeCandleData}
            onSave={updateCandle}
          />
        )}
      </div>
      {candleDetailData?.numberOfLayers && (
        <TagsGrid
          title="Количество слоев"
          tags={convertToTagData(candleDetailData.numberOfLayers)}
          allTags={convertToTagData(numberOfLayersData)}
          onSave={handleOnSaveNumberOfLayers}
        />
      )}
      {candleDetailData?.decors && (
        <ProductsGrid
          title="Декоры"
          data={candleDetailData.decors}
          renderPopUpComponent={(onClose) => (
            <AddProductPopUp
              onClose={onClose}
              title="Свеча и декоры"
              selectedData={candleDetailData.decors}
              allData={decorsData}
              onSave={updateCandleDecors}
            />
          )}
        />
      )}
      {candleDetailData?.layerColors && (
        <ProductsGrid
          title="Слои"
          data={candleDetailData.layerColors}
          renderPopUpComponent={(onClose) => (
            <AddProductPopUp
              onClose={onClose}
              title="Свеча и слои"
              selectedData={candleDetailData.layerColors}
              allData={layerColorsData}
              onSave={updateCandleLayerColors}
            />
          )}
        />
      )}
      {candleDetailData?.smells && (
        <ProductsGrid
          title="Запахи"
          data={candleDetailData.smells}
          renderPopUpComponent={(onClose) => (
            <AddProductPopUp
              onClose={onClose}
              title="Свеча и запахи"
              selectedData={candleDetailData.smells}
              allData={smellsData}
              onSave={updateCandleSmells}
            />
          )}
        />
      )}
      {candleDetailData?.wicks && (
        <ProductsGrid
          title="Фитили"
          data={candleDetailData.wicks}
          renderPopUpComponent={(onClose) => (
            <AddProductPopUp
              onClose={onClose}
              title="Свеча и фитили"
              selectedData={candleDetailData.wicks}
              allData={wicksData}
              onSave={updateCandleWicks}
            />
          )}
        />
      )}
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default CandleDetailsPage;

export function convertToTagData(numberOfLayers: NumberOfLayer[]): TagData[] {
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
