import { FC, useState } from 'react';

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
    createLayerColor,
    deleteLayerColor,
    updateIsActiveLayerColor,
  } = useLayerColorsQuery();
  const [errorMessage, setErrorMessage] = useState<string[]>([]);

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
        data={data}
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
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default AllLayerColorPage;
