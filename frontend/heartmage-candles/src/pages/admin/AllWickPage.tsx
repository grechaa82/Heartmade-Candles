import { FC, useEffect, useState, useContext } from 'react';
import { useInView } from 'react-intersection-observer';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import CreateWickPopUp from '../../modules/admin/PopUp/Wick/CreateWickPopUp';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useWicksQuery from '../../hooks/admin/useWicksQuery';
import { AuthContext } from '../../contexts/AuthContext';
import ProductsGridSkeleton from '../../modules/admin/ProductsGridSkeleton';

import { ImagesApi } from '../../services/ImagesApi';

import Style from './AllWickPage.module.css';

export interface AllWickPageProps {}

const AllWickPage: FC<AllWickPageProps> = () => {
  const { isAuth } = useContext(AuthContext);
  const {
    data,
    isLoading,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    createWick,
    deleteWick,
    updateIsActiveWick,
  } = useWicksQuery(21, isAuth);
  const [errorMessage, setErrorMessage] = useState<string[]>([]);
  const { ref, inView, entry } = useInView({
    threshold: 0,
  });

  useEffect(() => {
    if (entry && inView) {
      fetchNextPage();
    }
  }, [entry]);

  const handleUploadImages = async (files: File[]) => {
    const imagesResponse = await ImagesApi.uploadImages(files);
    if (imagesResponse.data && !imagesResponse.error) {
      return imagesResponse.data;
    } else {
      return [];
    }
  };

  if (isLoading) {
    return <ProductsGridSkeleton />;
  }

  return (
    <>
      <ProductsGrid
        data={data?.pages.flat() || []}
        title="Фитили"
        pageUrl="wicks"
        renderPopUpComponent={(onClose) => (
          <CreateWickPopUp
            onClose={onClose}
            title="Создать фитиль"
            onSave={createWick}
            uploadImages={handleUploadImages}
          />
        )}
        deleteProduct={deleteWick}
        updateIsActiveProduct={updateIsActiveWick}
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

export default AllWickPage;
