import { FC, useState, useEffect } from 'react';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import { Candle } from '../../types/Candle';
import { NumberOfLayer } from '../../types/NumberOfLayer';
import { TypeCandle } from '../../types/TypeCandle';
import TagsGrid from '../../modules/admin/TagsGrid';
import { convertToTagData } from './CandleDetailsPage';
import { TagData } from '../../components/shared/Tag';
import CreateCandlePopUp from '../../components/admin/PopUp/CreateCandlePopUp';
import { CandleRequest } from '../../types/Requests/CandleRequest';
import CreateTagPopUp from '../../components/admin/PopUp/CreateTagPopUp';
import { NumberOfLayerRequest } from '../../types/Requests/NumberOfLayerRequest';
import { TypeCandleRequest } from '../../types/Requests/TypeCandleRequest';

import { CandlesApi } from '../../services/CandlesApi';
import { NumberOfLayersApi } from '../../services/NumberOfLayersApi';
import { TypeCandlesApi } from '../../services/TypeCandlesApi';

export interface AllCandlePageProps {}

const AllCandlePage: FC<AllCandlePageProps> = () => {
  const [typeCandlesData, setTypeCandlesData] = useState<TypeCandle[]>([]);
  const [numberOfLayersData, setNumberOfLayersData] = useState<NumberOfLayer[]>([]);
  const [candlesData, setCandlesData] = useState<Candle[]>([]);

  const handleCreateCandle = async (createdItem: Candle) => {
    const candleRequest: CandleRequest = {
      title: createdItem.title,
      description: createdItem.description,
      price: createdItem.price,
      weightGrams: createdItem.weightGrams,
      images: createdItem.images,
      typeCandle: createdItem.typeCandle,
      isActive: createdItem.isActive,
    };
    await CandlesApi.create(candleRequest);
    const updatedCandles = await CandlesApi.getAll();
    setCandlesData(updatedCandles);
  };

  const handleDeleteCandle = async (id: string) => {
    await CandlesApi.delete(id);
    const updatedCandles = await CandlesApi.getAll();
    setCandlesData(updatedCandles);
  };

  const handleCreateNumberOfLayer = async (tag: TagData) => {
    const numberOfLayerRequest: NumberOfLayerRequest = {
      number: parseInt(tag.text),
    };
    await NumberOfLayersApi.create(numberOfLayerRequest);
    const updatedNumberOfLayers = await NumberOfLayersApi.getAll();
    setNumberOfLayersData(updatedNumberOfLayers);
  };

  const handleCreateTypeCandle = async (tag: TagData) => {
    const typeCandleRequest: TypeCandleRequest = {
      title: tag.text,
    };
    await TypeCandlesApi.create(typeCandleRequest);
    const updatedTypeCandles = await TypeCandlesApi.getAll();
    setTypeCandlesData(updatedTypeCandles);
  };

  const handleDeleteNumberOfLayer = async (id: string) => {
    await NumberOfLayersApi.delete(id);
    const updatedNumberOfLayers = await NumberOfLayersApi.getAll();
    setNumberOfLayersData(updatedNumberOfLayers);
  };

  const handleDeleteTypeCandle = async (id: string) => {
    await TypeCandlesApi.delete(id);
    const updatedTypeCandles = await TypeCandlesApi.getAll();
    setTypeCandlesData(updatedTypeCandles);
  };

  useEffect(() => {
    async function fetchData() {
      try {
        const typeCandles = await TypeCandlesApi.getAll();
        setTypeCandlesData(typeCandles);

        const numberOfLayers = await NumberOfLayersApi.getAll();
        setNumberOfLayersData(numberOfLayers);

        const candles = await CandlesApi.getAll();
        setCandlesData(candles);
      } catch (error) {
        console.error('Произошла ошибка при загрузке данных:', error);
      }
    }

    fetchData();
  }, []);

  return (
    <>
      <TagsGrid
        title="Типы свечей"
        tags={convertCandlesToTagData(typeCandlesData)}
        popUpComponent={
          <CreateTagPopUp
            onClose={() => console.log('Popup closed')}
            title="Сознать тип свечи"
            isNumber={true}
            onSave={handleCreateTypeCandle}
          />
        }
        onDelete={handleDeleteTypeCandle}
      />
      <TagsGrid
        title="Количество слоев"
        tags={convertToTagData(numberOfLayersData)}
        popUpComponent={
          <CreateTagPopUp
            onClose={() => console.log('Popup closed')}
            title="Сознать количество слоев"
            isNumber={true}
            onSave={handleCreateNumberOfLayer}
          />
        }
        onDelete={handleDeleteNumberOfLayer}
      />
      <ProductsGrid
        data={candlesData}
        title="Свечи"
        pageUrl="candles"
        popUpComponent={
          <CreateCandlePopUp
            onClose={() => console.log('Popup closed')}
            title="Создать свечу"
            typeCandlesArray={typeCandlesData}
            onSave={handleCreateCandle}
          />
        }
        deleteProduct={handleDeleteCandle}
      />
    </>
  );
};

export default AllCandlePage;

export function convertCandlesToTagData(candles: TypeCandle[]): TagData[] {
  return candles.map((candle) => ({
    id: candle.id,
    text: candle.title,
  }));
}
