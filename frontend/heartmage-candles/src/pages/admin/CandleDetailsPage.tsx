import { FC, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

import MainInfoCandle, {
  FetchTypeCandle,
} from '../../modules/admin/MainInfoCandle';
import { CandleDetail } from '../../types/CandleDetail';
import ProductsGrid, { FetchProducts } from '../../modules/admin/ProductsGrid';
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
  const [candleDetailData, setCandleDetailData] = useState<CandleDetail>();
  const [numberOfLayerTagData, setNumberOfLayerTagData] = useState<TagData[]>();

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const fetchTypeCandles: FetchTypeCandle = async () => {
    const typeCandlesResponse = await TypeCandlesApi.getAll();
    if (typeCandlesResponse.data && !typeCandlesResponse.error) {
      return typeCandlesResponse.data;
    } else {
      setErrorMessage([...errorMessage, typeCandlesResponse.error as string]);
      return [];
    }
  };

  const fetchDecors: FetchProducts<Decor> = async () => {
    const decorsResponse = await DecorsApi.getAll();
    if (decorsResponse.data && !decorsResponse.error) {
      return decorsResponse.data;
    } else {
      setErrorMessage([...errorMessage, decorsResponse.error as string]);
      return [];
    }
  };

  const fetchLayerColors: FetchProducts<LayerColor> = async () => {
    const layerColorsResponse = await LayerColorsApi.getAll();
    if (layerColorsResponse.data && !layerColorsResponse.error) {
      return layerColorsResponse.data;
    } else {
      setErrorMessage([...errorMessage, layerColorsResponse.error as string]);
      return [];
    }
  };

  const fetchNumberOfLayer = async (): Promise<NumberOfLayer[]> => {
    const numberOfLayersResponse = await NumberOfLayersApi.getAll();
    if (numberOfLayersResponse.data && !numberOfLayersResponse.error) {
      return numberOfLayersResponse.data;
    } else {
      setErrorMessage([
        ...errorMessage,
        numberOfLayersResponse.error as string,
      ]);
      return [];
    }
  };

  const fetchSmells: FetchProducts<Smell> = async () => {
    const smellsResponse = await SmellsApi.getAll();
    if (smellsResponse.data && !smellsResponse.error) {
      return smellsResponse.data;
    } else {
      setErrorMessage([...errorMessage, smellsResponse.error as string]);
      return [];
    }
  };

  const fetchWicks: FetchProducts<Wick> = async () => {
    const wicksResponse = await WicksApi.getAll();
    if (wicksResponse.data && !wicksResponse.error) {
      return wicksResponse.data;
    } else {
      setErrorMessage([...errorMessage, wicksResponse.error as string]);
      return [];
    }
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
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      decors: updatedDecors,
    };

    setCandleDetailData(newCandleDetailData);
  };

  const handleChangesLayerColors = (updatedItems: BaseProduct[]) => {
    const updatedLayerColors = updatedItems as LayerColor[];
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      layerColors: updatedLayerColors,
    };

    setCandleDetailData(newCandleDetailData);
  };

  const handleChangesNumberOfLayers = (
    updatedNumberOfLayers: TagData[]
  ): void => {
    const numberOfLayers: NumberOfLayer[] = updatedNumberOfLayers.map(
      (tagData) => ({
        id: tagData.id,
        number: parseInt(tagData.text),
      })
    );

    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      numberOfLayers: numberOfLayers,
    };

    setCandleDetailData(newCandleDetailData);
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

  const updateCandle = async (updatedItem: Candle) => {
    if (id) {
      const candleRequest = {
        title: updatedItem.title,
        description: updatedItem.description,
        price: updatedItem.price,
        weightGrams: updatedItem.weightGrams,
        images: updatedItem.images,
        typeCandle: updatedItem.typeCandle,
        isActive: updatedItem.isActive,
      };
      await CandlesApi.update(id, candleRequest);
    }
  };

  const updateCandleDecors = async (updatedItems: BaseProduct[]) => {
    if (id) {
      const updatedDecors = updatedItems as Decor[];
      const ids = updatedDecors.map((d) => d.id);

      const candleDecorsResponse = await CandlesApi.updateDecor(id, ids);
      if (candleDecorsResponse.data && !candleDecorsResponse.error) {
        setCandleDetailData(candleDecorsResponse.data);
      } else {
        setErrorMessage([
          ...errorMessage,
          candleDecorsResponse.error as string,
        ]);
      }
    }
  };

  const updateCandleLayerColors = async (updatedItems: BaseProduct[]) => {
    if (id) {
      const updatedLayerColors = updatedItems as LayerColor[];
      const ids = updatedLayerColors.map((l) => l.id);

      const candleLayerColorsResponse = await CandlesApi.updateLayerColor(
        id,
        ids
      );
      if (candleLayerColorsResponse.data && !candleLayerColorsResponse.error) {
        setCandleDetailData(candleLayerColorsResponse.data);
      } else {
        setErrorMessage([
          ...errorMessage,
          candleLayerColorsResponse.error as string,
        ]);
      }
    }
  };

  const updateCandleNumberOfLayers = async (updatedItems: TagData[]) => {
    if (id) {
      const ids = updatedItems.map((n) => n.id);

      const candleNumberOfLayersResponse = await CandlesApi.updateNumberOfLayer(
        id,
        ids
      );
      if (
        candleNumberOfLayersResponse.data &&
        !candleNumberOfLayersResponse.error
      ) {
        setCandleDetailData(candleNumberOfLayersResponse.data);
      } else {
        setErrorMessage([
          ...errorMessage,
          candleNumberOfLayersResponse.error as string,
        ]);
      }
    }
  };

  const updateCandleSmells = async (updatedItems: BaseProduct[]) => {
    if (id) {
      const updatedSmells = updatedItems as Smell[];
      const ids = updatedSmells.map((s) => s.id);

      const candleSmellsResponse = await CandlesApi.updateSmell(id, ids);
      if (candleSmellsResponse.data && !candleSmellsResponse.error) {
        setCandleDetailData(candleSmellsResponse.data);
      } else {
        setErrorMessage([
          ...errorMessage,
          candleSmellsResponse.error as string,
        ]);
      }
    }
  };

  const updateCandleWicks = async (updatedItems: BaseProduct[]) => {
    if (id) {
      const updatedWicks = updatedItems as Wick[];
      const ids = updatedWicks.map((w) => w.id);

      const candleWicksResponse = await CandlesApi.updateWick(id, ids);
      if (candleWicksResponse.data && !candleWicksResponse.error) {
        setCandleDetailData(candleWicksResponse.data);
      } else {
        setErrorMessage([...errorMessage, candleWicksResponse.error as string]);
      }
    }
  };

  useEffect(() => {
    async function fetchCandle() {
      if (id) {
        const candleResponse = await CandlesApi.getById(id);
        if (candleResponse.data && !candleResponse.error) {
          setCandleDetailData(candleResponse.data);
        } else {
          setErrorMessage([...errorMessage, candleResponse.error as string]);
        }
      }
    }

    async function fetchNumberOfLayers() {
      const data = await fetchNumberOfLayer();
      const tagData = convertToTagData(data);
      setNumberOfLayerTagData(tagData);
    }

    fetchCandle();
    fetchNumberOfLayers();
  }, [id]);

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
          allTags={numberOfLayerTagData || []}
          onSave={updateCandleNumberOfLayers}
          onChanges={handleChangesNumberOfLayers}
        />
      )}
      {candleDetailData?.decors && (
        <ProductsGrid
          title="Декоры"
          data={candleDetailData.decors}
          popUpComponent={
            <AddProductPopUp
              onClose={() => console.log('Popup closed')}
              title="Свеча и декоры"
              selectedData={candleDetailData.decors}
              setSelectedData={handleChangesDecors}
              fetchAllData={fetchDecors}
              onSave={updateCandleDecors}
            />
          }
        />
      )}
      {candleDetailData?.layerColors && (
        <ProductsGrid
          title="Слои"
          data={candleDetailData.layerColors}
          popUpComponent={
            <AddProductPopUp
              onClose={() => console.log('Popup closed')}
              title="Свеча и слои"
              selectedData={candleDetailData.layerColors}
              setSelectedData={handleChangesLayerColors}
              fetchAllData={fetchLayerColors}
              onSave={updateCandleLayerColors}
            />
          }
        />
      )}
      {candleDetailData?.smells && (
        <ProductsGrid
          title="Запахи"
          data={candleDetailData.smells}
          popUpComponent={
            <AddProductPopUp
              onClose={() => console.log('Popup closed')}
              title="Свеча и запахи"
              selectedData={candleDetailData.smells}
              setSelectedData={handleChangesSmells}
              fetchAllData={fetchSmells}
              onSave={updateCandleSmells}
            />
          }
        />
      )}
      {candleDetailData?.wicks && (
        <ProductsGrid
          title="Фитили"
          data={candleDetailData.wicks}
          popUpComponent={
            <AddProductPopUp
              onClose={() => console.log('Popup closed')}
              title="Свеча и фитили"
              selectedData={candleDetailData.wicks}
              setSelectedData={handleChangesWicks}
              fetchAllData={fetchWicks}
              onSave={updateCandleWicks}
            />
          }
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
