import { FC, useState, useEffect } from 'react';
import { useInView } from 'react-intersection-observer';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import { Candle } from '../../types/Candle';
import { NumberOfLayer } from '../../types/NumberOfLayer';
import { TypeCandle } from '../../types/TypeCandle';
import TagsGrid from '../../modules/admin/TagsGrid';
import { convertToTagData } from './CandleDetailsPage';
import { TagData } from '../../components/shared/Tag';
import CreateCandlePopUp from '../../modules/admin/PopUp/Candle/CreateCandlePopUp';
import CreateTagPopUp from '../../modules/admin/PopUp/Tag/CreateTagPopUp';
import { NumberOfLayerRequest } from '../../types/Requests/NumberOfLayerRequest';
import { TypeCandleRequest } from '../../types/Requests/TypeCandleRequest';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import { PaginationSettings } from '../../typesV2/shared/PaginationSettings';
import ButtonDropdown, {
  optionData,
} from '../../components/shared/ButtonDropdown';
import useCandlesQuery from '../../hooks/useCandlesQuery';

import { NumberOfLayersApi } from '../../services/NumberOfLayersApi';
import { TypeCandlesApi } from '../../services/TypeCandlesApi';
import { ImagesApi } from '../../services/ImagesApi';

import Style from './AllCandlePage.module.css';

export interface AllCandlePageProps {}

const AllCandlePage: FC<AllCandlePageProps> = () => {
  const [typeCandlesData, setTypeCandlesData] = useState<TypeCandle[]>([]);
  const [numberOfLayersData, setNumberOfLayersData] = useState<NumberOfLayer[]>(
    [],
  );
  const [candlesData, setCandlesData] = useState<Candle[]>([]);
  const [typeFilter, setTypeFilter] = useState<string>(null);
  const [pagination] = useState<PaginationSettings>({
    pageSize: 6,
    pageIndex: 0,
  });
  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const handleCreateNumberOfLayer = async (tag: TagData) => {
    const numberOfLayerRequest: NumberOfLayerRequest = {
      number: parseInt(tag.text),
    };

    const response = await NumberOfLayersApi.create(numberOfLayerRequest);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedNumberOfLayersResponse = await NumberOfLayersApi.getAll();
      if (
        updatedNumberOfLayersResponse.data &&
        !updatedNumberOfLayersResponse.error
      ) {
        setNumberOfLayersData(updatedNumberOfLayersResponse.data);
      } else {
        setErrorMessage([
          ...errorMessage,
          updatedNumberOfLayersResponse.error as string,
        ]);
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
      if (
        updatedTypeCandlesResponse.data &&
        !updatedTypeCandlesResponse.error
      ) {
        setTypeCandlesData(updatedTypeCandlesResponse.data);
      } else {
        setErrorMessage([
          ...errorMessage,
          updatedTypeCandlesResponse.error as string,
        ]);
      }
    }
  };

  const handleDeleteNumberOfLayer = async (id: string) => {
    const response = await NumberOfLayersApi.delete(id);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedNumberOfLayersResponse = await NumberOfLayersApi.getAll();
      if (
        updatedNumberOfLayersResponse.data &&
        !updatedNumberOfLayersResponse.error
      ) {
        setNumberOfLayersData(updatedNumberOfLayersResponse.data);
      } else {
        setErrorMessage([
          ...errorMessage,
          updatedNumberOfLayersResponse.error as string,
        ]);
      }
    }
  };

  const handleDeleteTypeCandle = async (id: string) => {
    const response = await TypeCandlesApi.delete(id);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedTypeCandlesResponse = await TypeCandlesApi.getAll();
      if (
        updatedTypeCandlesResponse.data &&
        !updatedTypeCandlesResponse.error
      ) {
        setTypeCandlesData(updatedTypeCandlesResponse.data);
      } else {
        setErrorMessage([
          ...errorMessage,
          updatedTypeCandlesResponse.error as string,
        ]);
      }
    }
  };

  useEffect(() => {
    async function fetchData() {
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
        setErrorMessage([
          ...errorMessage,
          numberOfLayersResponse.error as string,
        ]);
      }
    }

    fetchData();
  }, []);

  const handleUploadImages = async (files: File[]) => {
    const imagesResponse = await ImagesApi.uploadImages(files);
    if (imagesResponse.data && !imagesResponse.error) {
      return imagesResponse.data;
    } else {
      return [];
    }
  };

  const optionDataCandleFilters: optionData[] = [{ id: 'Все', title: 'Все' }];
  typeCandlesData.map((item) => {
    const optionDataFilter: optionData = {
      id: item.id.toString(),
      title: item.title,
    };
    optionDataCandleFilters.push(optionDataFilter);
  });

  const handleOnChangeFilter = (selectedFilter: optionData) => {
    if (selectedFilter.title === 'Все') {
      setTypeFilter(null);
    } else {
      setTypeFilter(selectedFilter.title);
    }
  };

  const {
    data,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    createCandle,
    deleteCandle,
    updateIsActiveCandle,
  } = useCandlesQuery(typeFilter, pagination.pageSize);

  const { ref, inView, entry } = useInView({
    threshold: 0,
  });

  useEffect(() => {
    if (entry && inView) {
      fetchNextPage();
    }
  }, [entry]);

  return (
    <>
      <TagsGrid
        title="Типы свечей"
        withInput={false}
        tags={convertCandlesToTagData(typeCandlesData)}
        popUpComponent={
          <CreateTagPopUp
            onClose={() => console.log('Popup closed')}
            title="Сознать тип свечи"
            onSave={handleCreateTypeCandle}
          />
        }
        onDelete={handleDeleteTypeCandle}
      />
      <TagsGrid
        title="Количество слоев"
        withInput={false}
        tags={convertToTagData(numberOfLayersData)}
        popUpComponent={
          <CreateTagPopUp
            onClose={() => console.log('Popup closed')}
            title="Сознать количество слоев"
            onSave={handleCreateNumberOfLayer}
          />
        }
        onDelete={handleDeleteNumberOfLayer}
      />
      <ProductsGrid
        data={data?.pages.flat() || []}
        title="Свечи"
        pageUrl="candles"
        popUpComponent={
          <CreateCandlePopUp
            onClose={() => console.log('Popup closed')}
            title="Создать свечу"
            typeCandlesArray={typeCandlesData}
            onSave={createCandle}
            uploadImages={handleUploadImages}
          />
        }
        deleteProduct={deleteCandle}
        updateIsActiveProduct={updateIsActiveCandle}
        filterComponent={
          <ButtonDropdown
            text={'Тип свечей'}
            options={optionDataCandleFilters}
            selected={{
              id: typeFilter ? typeFilter : 'Все',
              title: typeFilter ? typeFilter : 'Все',
            }}
            onChange={handleOnChangeFilter}
          />
        }
      />
      {isFetchingNextPage ? (
        <span>loading...</span>
      ) : (
        hasNextPage && <div ref={ref}></div>
      )}
      <ListErrorPopUp messages={errorMessage} />
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
