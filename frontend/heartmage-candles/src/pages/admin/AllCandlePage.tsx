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
import useCandlesQuery from '../../hooks/admin/useCandlesQuery';
import useNumberOfLayersQuery from '../../hooks/admin/useNumberOfLayersQuery';
import useTypeCandlesQuery from '../../hooks/admin/useTypeCandlesQuery';
import { AuthContext } from '../../contexts/AuthContext';
import CandleFilter from '../../components/admin/CandleFilter';
import ProductsGridSkeleton from '../../modules/admin/ProductsGridSkeleton';
import TagsGridSkeleton from '../../modules/admin/TagsGridSkeleton';

import { ImagesApi } from '../../services/ImagesApi';

import Style from './AllCandlePage.module.css';

export interface AllCandlePageProps {}

const AllCandlePage: FC<AllCandlePageProps> = () => {
  const { isAuth } = useContext(AuthContext);
  const [selectedTypeFilter, setSelectedTypeFilter] =
    useState<TypeCandle>(null);
  const [pagination] = useState<PaginationSettings>({
    pageSize: 21,
    pageIndex: 0,
  });
  const {
    data: candlesData,
    fetchNextPage,
    isLoading,
    hasNextPage,
    isFetchingNextPage,
    createCandle,
    deleteCandle,
    updateIsActiveCandle,
  } = useCandlesQuery(selectedTypeFilter?.title, pagination.pageSize, isAuth);
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

  const handleOnChangeFilter = (typeCandle: TypeCandle) => {
    if (typeCandle.title === 'Все') {
      setSelectedTypeFilter(null);
    } else {
      setSelectedTypeFilter(typeCandle);
    }
  };

  return (
    <>
      {!isLoadingTypeCandle ? (
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
      ) : (
        <TagsGridSkeleton />
      )}
      {!isLoadingNumberOfLayers ? (
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
      ) : (
        <TagsGridSkeleton />
      )}
      {!isLoading ? (
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
            <CandleFilter
              typeCandles={[
                { id: 0, title: 'Все' },
                ...(typeCandlesData ?? []),
              ]}
              selectedTypeCandle={{
                id: selectedTypeFilter ? selectedTypeFilter.id : 0,
                title: selectedTypeFilter ? selectedTypeFilter.title : 'Все',
              }}
              onChange={handleOnChangeFilter}
            />
          )}
        />
      ) : (
        <ProductsGridSkeleton />
      )}

      {isFetchingNextPage ? (
        <span>Loading</span>
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
