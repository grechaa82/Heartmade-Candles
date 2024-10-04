import { FC, useEffect, useState } from 'react';
import { useInView } from 'react-intersection-observer';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import CreateLayerColorPopUp from '../../modules/admin/PopUp/LayerColor/CreateLayerColorPopUp';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useLayerColorsQuery from '../../hooks/useLayerColorsQuery';

import { ImagesApi } from '../../services/ImagesApi';

import Style from './AllLayerColorPage.module.css';

export interface AllLayerColorPageProps {}

const AllLayerColorPage: FC<AllLayerColorPageProps> = () => {
  const {
    data,
    isLoading,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    createLayerColor,
    deleteLayerColor,
    updateIsActiveLayerColor,
  } = useLayerColorsQuery();
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
    return <div>...Loading</div>;
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
