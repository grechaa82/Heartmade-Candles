import { FC, useEffect, useState, useContext } from 'react';
import { useInView } from 'react-intersection-observer';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import CreateLayerColorPopUp from '../../modules/admin/PopUp/LayerColor/CreateLayerColorPopUp';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useLayerColorsQuery from '../../hooks/admin/useLayerColorsQuery';
import { AuthContext } from '../../contexts/AuthContext';
import ProductsGridSkeleton from '../../modules/admin/ProductsGridSkeleton';

import { ImagesApi } from '../../services/ImagesApi';

import Style from './AllLayerColorPage.module.css';

export interface AllLayerColorPageProps {}

const AllLayerColorPage: FC<AllLayerColorPageProps> = () => {
  const { isAuth } = useContext(AuthContext);
  const {
    data,
    isLoading,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    createLayerColor,
    deleteLayerColor,
    updateIsActiveLayerColor,
  } = useLayerColorsQuery(21, isAuth);
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
        title="Слои"
        pageUrl="layerColors"
        renderPopUpComponent={(onClose) => (
          <CreateLayerColorPopUp
            onClose={onClose}
            title="Создать слой"
            onSave={createLayerColor}
            uploadImages={handleUploadImages}
          />
        )}
        deleteProduct={deleteLayerColor}
        updateIsActiveProduct={updateIsActiveLayerColor}
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

export default AllLayerColorPage;
