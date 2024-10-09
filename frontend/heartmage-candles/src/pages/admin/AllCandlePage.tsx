import { FC, useState, useEffect, useContext } from 'react';
import { useInView } from 'react-intersection-observer';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import { TypeCandle } from '../../types/TypeCandle';
import TagsGrid from '../../modules/admin/TagsGrid';
import { convertToTagData } from './CandleDetailsPage';
import { TagData } from '../../components/shared/Tag';
import CreateCandlePopUp from '../../modules/admin/PopUp/Candle/CreateCandlePopUp';
import CreateTagPopUp from '../../modules/admin/PopUp/Tag/CreateTagPopUp';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import { PaginationSettings } from '../../typesV2/shared/PaginationSettings';
import ButtonDropdown, {
  optionData,
} from '../../components/shared/ButtonDropdown';
import useCandlesQuery from '../../hooks/admin/useCandlesQuery';
import useNumberOfLayersQuery from '../../hooks/admin/useNumberOfLayersQuery';
import useTypeCandlesQuery from '../../hooks/admin/useTypeCandlesQuery';
import { AuthContext } from '../../contexts/AuthContext';

import { ImagesApi } from '../../services/ImagesApi';

import Style from './AllCandlePage.module.css';

export interface AllCandlePageProps {}

const AllCandlePage: FC<AllCandlePageProps> = () => {
  const { isAuth } = useContext(AuthContext);
  const [typeFilter, setTypeFilter] = useState<string>(null);
  const [pagination] = useState<PaginationSettings>({
    pageSize: 21,
    pageIndex: 0,
  });
  const {
    data: candlesData,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    createCandle,
    deleteCandle,
    updateIsActiveCandle,
  } = useCandlesQuery(typeFilter, pagination.pageSize, isAuth);
  const {
    data: numberOfLayersData,
    isLoading: isLoadingNumberOfLayers,
    createNumberOfLayer,
    deleteNumberOfLayer,
  } = useNumberOfLayersQuery(isAuth);
  const {
    data: typeCandlesData,
    isLoading: isLoadingTypeCandle,
    createTypeCandle,
    deleteTypeCandle,
  } = useTypeCandlesQuery(isAuth);
  const [errorMessage] = useState<string[]>([]);
  const { ref, inView, entry } = useInView({
    threshold: 0,
  });

  useEffect(() => {
    if (entry && inView) {
      fetchNextPage();
    }
  }, [entry]);

  const handleCreateNumberOfLayer = async (tag: TagData) => {
    createNumberOfLayer({
      id: 0,
      number: parseInt(tag.text),
    });
  };

  const handleCreateTypeCandle = async (tag: TagData) => {
    createTypeCandle({
      id: 0,
      title: tag.text,
    });
  };

  const handleUploadImages = async (files: File[]) => {
    const imagesResponse = await ImagesApi.uploadImages(files);
    if (imagesResponse.data && !imagesResponse.error) {
      return imagesResponse.data;
    } else {
      return [];
    }
  };

  const handleOnChangeFilter = (selectedFilter: optionData) => {
    if (selectedFilter.title === 'Все') {
      setTypeFilter(null);
    } else {
      setTypeFilter(selectedFilter.title);
    }
  };

  return (
    <>
      {!isLoadingTypeCandle && (
        <TagsGrid
          title="Типы свечей"
          withInput={false}
          tags={convertCandlesToTagData(typeCandlesData ?? [])}
          popUpComponent={
            <CreateTagPopUp
              onClose={() => console.log('Popup closed')}
              title="Сознать тип свечи"
              onSave={handleCreateTypeCandle}
            />
          }
          onDelete={deleteTypeCandle}
        />
      )}
      {!isLoadingNumberOfLayers && (
        <TagsGrid
          title="Количество слоев"
          withInput={false}
          tags={convertToTagData(numberOfLayersData ?? [])}
          popUpComponent={
            <CreateTagPopUp
              onClose={() => console.log('Popup closed')}
              title="Сознать количество слоев"
              onSave={handleCreateNumberOfLayer}
            />
          }
          onDelete={deleteNumberOfLayer}
        />
      )}
      <ProductsGrid
        data={candlesData?.pages.flat() || []}
        title="Свечи"
        pageUrl="candles"
        renderPopUpComponent={(onClose) => (
          <CreateCandlePopUp
            onClose={onClose}
            title="Создать свечу"
            typeCandlesArray={typeCandlesData}
            onSave={createCandle}
            uploadImages={handleUploadImages}
          />
        )}
        deleteProduct={deleteCandle}
        updateIsActiveProduct={updateIsActiveCandle}
        renderFilterComponent={() => (
          <ButtonDropdown
            text={'Тип свечей'}
            options={convertCandlesToOptionData(typeCandlesData ?? [])}
            selected={{
              id: typeFilter ? typeFilter : 'Все',
              title: typeFilter ? typeFilter : 'Все',
            }}
            onChange={handleOnChangeFilter}
          />
        )}
      />
      {isFetchingNextPage ? (
        <span>...Loading</span>
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

export function convertCandlesToOptionData(
  typeCandles: TypeCandle[],
): optionData[] {
  const optionDataCandleFilters: optionData[] = [{ id: 'Все', title: 'Все' }];
  if (typeCandles.length > 0) {
    typeCandles.flatMap((item) => {
      const optionDataFilter: optionData = {
        id: item.id.toString(),
        title: item.title,
      };
      optionDataCandleFilters.push(optionDataFilter);
    });
  }
  return optionDataCandleFilters;
}
