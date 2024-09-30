import { FC, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

import MainInfoCandle, {
  FetchTypeCandle,
} from '../../modules/admin/MainInfoCandle';
import { CandleDetail } from '../../types/CandleDetail';
import ProductsGrid from '../../modules/admin/ProductsGrid';
import TagsGrid from '../../modules/admin/TagsGrid';
import { Candle } from '../../types/Candle';
import { Decor } from '../../types/Decor';
import { NumberOfLayer } from '../../types/NumberOfLayer';
import { LayerColor } from '../../types/LayerColor';
import { Smell } from '../../types/Smell';
import { Wick } from '../../types/Wick';
import { BaseProduct } from '../../types/BaseProduct';
import { TagData } from '../../components/shared/Tag';
import AddProductPopUp from '../../components/admin/PopUp/AddProductPopUp';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useCandleByIdQuery from '../../hooks/useCandleByIdQuery';
import useNumberOfLayersQuery from '../../hooks/useNumberOfLayersQuery';
import useTypeCandlesQuery from '../../hooks/useTypeCandlesQuery';

import { CandlesApi } from '../../services/CandlesApi';
import { DecorsApi } from '../../services/DecorsApi';
import { LayerColorsApi } from '../../services/LayerColorsApi';
import { NumberOfLayersApi } from '../../services/NumberOfLayersApi';
import { SmellsApi } from '../../services/SmellsApi';
import { TypeCandlesApi } from '../../services/TypeCandlesApi';
import { WicksApi } from '../../services/WicksApi';

import Style from './CandleDetailsPage.module.css';

type CandleDetailsParams = {
  id: string;
};

const CandleDetailsPage: FC = () => {
  const { id } = useParams<CandleDetailsParams>();
  const [_1, setCandleDetailData] = useState<CandleDetail>();
  // const [_2, setNumberOfLayerTagData] = useState<TagData[]>();

  const {
    data: candleDetailData,
    isLoading,
    isSuccess,
    error,
    updateCandle,
    updateCandleNumberOfLayers,
    updateCandleDecors,
  } = useCandleByIdQuery(id);
  const { data: numberOfLayersData } = useNumberOfLayersQuery();
  // const numberOfLayerTagData = convertToTagData(numberOfLayersData);

  const fetchTypeCandles: FetchTypeCandle = async () => {
    return await TypeCandlesApi.getAll();
  };

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const fetchDecors = async () => {
    return (await DecorsApi.getAll()).data;
  };

  const fetchLayerColors = async () => {
    return (await LayerColorsApi.getAll()).data;
  };

  // const fetchNumberOfLayer = async (): Promise<NumberOfLayer[]> => {
  //   const numberOfLayersResponse = await NumberOfLayersApi.getAll();
  //   if (numberOfLayersResponse.data && !numberOfLayersResponse.error) {
  //     return numberOfLayersResponse.data;
  //   } else {
  //     setErrorMessage([
  //       ...errorMessage,
  //       numberOfLayersResponse.error as string,
  //     ]);
  //     return [];
  //   }
  // };

  const fetchSmells = async () => {
    return (await SmellsApi.getAll()).data;
  };

  const fetchWicks = async () => {
    return (await WicksApi.getAll()).data;
  };

  const handleChangesCandle = (updatedcandle: Candle) => {
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData,
      candle: {
        ...candleDetailData?.candle,
        ...updatedcandle,
      },
    };

    setCandleDetailData(newCandleDetailData);
  };

  const handleChangesDecors = (updatedItems: BaseProduct[]) => {
    const updatedDecors = updatedItems as Decor[];
    updateCandleDecors(updatedDecors);
  };

  const handleChangesLayerColors = (updatedItems: BaseProduct[]) => {
    const updatedLayerColors = updatedItems as LayerColor[];
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      layerColors: updatedLayerColors,
    };

    setCandleDetailData(newCandleDetailData);
  };

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

  const handleChangesSmells = (updatedItems: BaseProduct[]) => {
    const updatedSmells = updatedItems as Smell[];
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      smells: updatedSmells,
    };

    setCandleDetailData(newCandleDetailData);
  };

  const handleChangesWicks = (updatedItems: BaseProduct[]) => {
    const updatedWicks = updatedItems as Wick[];
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      wicks: updatedWicks,
    };

    setCandleDetailData(newCandleDetailData);
  };

  // const updateCandleDecors = async (updatedItems: BaseProduct[]) => {
  //   if (id) {
  //     const updatedDecors = updatedItems as Decor[];
  //     const ids = updatedDecors.map((d) => d.id);

  //     const candleDecorsResponse = await CandlesApi.updateDecor(id, ids);
  //     if (candleDecorsResponse.error) {
  //       setErrorMessage([
  //         ...errorMessage,
  //         candleDecorsResponse.error as string,
  //       ]);
  //     }
  //   }
  // };

  // const updateCandleLayerColors = async (updatedItems: BaseProduct[]) => {
  //   if (id) {
  //     const updatedLayerColors = updatedItems as LayerColor[];
  //     const ids = updatedLayerColors.map((l) => l.id);

  //     const candleLayerColorsResponse = await CandlesApi.updateLayerColor(
  //       id,
  //       ids,
  //     );
  //     if (candleLayerColorsResponse.error) {
  //       setErrorMessage([
  //         ...errorMessage,
  //         candleLayerColorsResponse.error as string,
  //       ]);
  //     }
  //   }
  // };

  // const updateCandleNumberOfLayers = async (updatedItems: TagData[]) => {
  //   if (id) {
  //     const ids = updatedItems.map((n) => n.id);

  //     const candleNumberOfLayersResponse = await CandlesApi.updateNumberOfLayer(
  //       id,
  //       ids,
  //     );
  //     if (candleNumberOfLayersResponse.error) {
  //       setErrorMessage([
  //         ...errorMessage,
  //         candleNumberOfLayersResponse.error as string,
  //       ]);
  //     }
  //   }
  // };

  // const updateCandleSmells = async (updatedItems: BaseProduct[]) => {
  //   if (id) {
  //     const updatedSmells = updatedItems as Smell[];
  //     const ids = updatedSmells.map((s) => s.id);

  //     const candleSmellsResponse = await CandlesApi.updateSmell(id, ids);
  //     if (candleSmellsResponse.error) {
  //       setErrorMessage([
  //         ...errorMessage,
  //         candleSmellsResponse.error as string,
  //       ]);
  //     }
  //   }
  // };

  // const updateCandleWicks = async (updatedItems: BaseProduct[]) => {
  //   if (id) {
  //     const updatedWicks = updatedItems as Wick[];
  //     const ids = updatedWicks.map((w) => w.id);

  //     const candleWicksResponse = await CandlesApi.updateWick(id, ids);
  //     if (candleWicksResponse.error) {
  //       setErrorMessage([...errorMessage, candleWicksResponse.error as string]);
  //     }
  //   }
  // };

  // useEffect(() => {
  //   async function fetchNumberOfLayers() {
  //     const data = await fetchNumberOfLayer();
  //     const tagData = convertToTagData(data);
  //     setNumberOfLayerTagData(tagData);
  //   }

  //   fetchNumberOfLayers();
  // }, [id]);

  if (isLoading) {
    return <div>...isLoading</div>;
  }

  return (
    <>
      <div className="candles">
        {candleDetailData && (
          <MainInfoCandle
            data={candleDetailData.candle}
            fetchTypeCandles={fetchTypeCandles}
            onChangesCandle={handleChangesCandle}
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
          // onChanges={handleChangesNumberOfLayers}
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
              setSelectedData={handleChangesDecors}
              fetchAllData={fetchDecors}
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
              setSelectedData={handleChangesLayerColors}
              fetchAllData={fetchLayerColors}
              onSave={() => console.log('updateCandleLayerColors')}
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
              setSelectedData={handleChangesSmells}
              fetchAllData={fetchSmells}
              onSave={() => console.log('updateCandleSmells')}
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
              setSelectedData={handleChangesWicks}
              fetchAllData={fetchWicks}
              onSave={() => console.log('updateCandleWicks')}
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
