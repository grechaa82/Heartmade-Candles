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
import ListErrorPopUp from '../../modules/constructor/ListErrorPopUp';

import { CandlesApi } from '../../services/CandlesApi';
import { NumberOfLayersApi } from '../../services/NumberOfLayersApi';
import { TypeCandlesApi } from '../../services/TypeCandlesApi';

import Style from './AllCandlePage.module.css';

export interface AllCandlePageProps {}

const AllCandlePage: FC<AllCandlePageProps> = () => {
  const [typeCandlesData, setTypeCandlesData] = useState<TypeCandle[]>([]);
  const [numberOfLayersData, setNumberOfLayersData] = useState<NumberOfLayer[]>([]);
  const [candlesData, setCandlesData] = useState<Candle[]>([]);

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

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

    const response = await CandlesApi.create(candleRequest);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedCandlesResponse = await CandlesApi.getAll();
      if (updatedCandlesResponse.data && !updatedCandlesResponse.error) {
        setCandlesData(updatedCandlesResponse.data);
      } else {
        setErrorMessage([...errorMessage, updatedCandlesResponse.error as string]);
      }
    }
  };

  const handleDeleteCandle = async (id: string) => {
    const response = await CandlesApi.delete(id);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedCandlesResponse = await CandlesApi.getAll();
      if (updatedCandlesResponse.data && !updatedCandlesResponse.error) {
        setCandlesData(updatedCandlesResponse.data);
      } else {
        setErrorMessage([...errorMessage, updatedCandlesResponse.error as string]);
      }
    }
  };

  const handleCreateNumberOfLayer = async (tag: TagData) => {
    const numberOfLayerRequest: NumberOfLayerRequest = {
      number: parseInt(tag.text),
    };

    const response = await NumberOfLayersApi.create(numberOfLayerRequest);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedNumberOfLayersResponse = await NumberOfLayersApi.getAll();
      if (updatedNumberOfLayersResponse.data && !updatedNumberOfLayersResponse.error) {
        setNumberOfLayersData(updatedNumberOfLayersResponse.data);
      } else {
        setErrorMessage([...errorMessage, updatedNumberOfLayersResponse.error as string]);
      }
    }
  };

  const handleCreateTypeCandle = async (tag: TagData) => {
    const typeCandleRequest: TypeCandleRequest = {
      title: tag.text,
    };

    const response = await TypeCandlesApi.create(typeCandleRequest);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedTypeCandlesResponse = await TypeCandlesApi.getAll();
      if (updatedTypeCandlesResponse.data && !updatedTypeCandlesResponse.error) {
        setTypeCandlesData(updatedTypeCandlesResponse.data);
      } else {
        setErrorMessage([...errorMessage, updatedTypeCandlesResponse.error as string]);
      }
    }
  };

  const handleDeleteNumberOfLayer = async (id: string) => {
    const response = await NumberOfLayersApi.delete(id);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedNumberOfLayersResponse = await NumberOfLayersApi.getAll();
      if (updatedNumberOfLayersResponse.data && !updatedNumberOfLayersResponse.error) {
        setNumberOfLayersData(updatedNumberOfLayersResponse.data);
      } else {
        setErrorMessage([...errorMessage, updatedNumberOfLayersResponse.error as string]);
      }
    }
  };

  const handleDeleteTypeCandle = async (id: string) => {
    const response = await TypeCandlesApi.delete(id);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedTypeCandlesResponse = await TypeCandlesApi.getAll();
      if (updatedTypeCandlesResponse.data && !updatedTypeCandlesResponse.error) {
        setTypeCandlesData(updatedTypeCandlesResponse.data);
      } else {
        setErrorMessage([...errorMessage, updatedTypeCandlesResponse.error as string]);
      }
    }
  };

  useEffect(() => {
    async function fetchData() {
      const candlesResponse = await CandlesApi.getAll();
      if (candlesResponse.data && !candlesResponse.error) {
        setCandlesData(candlesResponse.data);
      } else {
        setErrorMessage([...errorMessage, candlesResponse.error as string]);
      }

      const typeCandlesResponse = await TypeCandlesApi.getAll();
      if (typeCandlesResponse.data && !typeCandlesResponse.error) {
        setTypeCandlesData(typeCandlesResponse.data);
      } else {
        setErrorMessage([...errorMessage, typeCandlesResponse.error as string]);
      }

      const numberOfLayersResponse = await NumberOfLayersApi.getAll();
      if (numberOfLayersResponse.data && !numberOfLayersResponse.error) {
        setNumberOfLayersData(numberOfLayersResponse.data);
      } else {
        setErrorMessage([...errorMessage, numberOfLayersResponse.error as string]);
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
      <div className={Style.popUpNotification}>
        <ListErrorPopUp messages={errorMessage} />
      </div>
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
