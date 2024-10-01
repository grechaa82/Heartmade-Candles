import { FC, useState } from 'react';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import CreateDecorPopUp from '../../modules/admin/PopUp/Decor/CreateDecorPopUp';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useDecorQuery from '../../hooks/useDecorQuery';

import { ImagesApi } from '../../services/ImagesApi';

import Style from './AllDecorPage.module.css';

export interface AllDecorPageProps {}

const AllDecorPage: FC<AllDecorPageProps> = () => {
  const { data, isLoading, createDecor, deleteDecor, updateIsActiveDecor } =
    useDecorQuery();
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
        title="Декоры"
        pageUrl="decors"
        renderPopUpComponent={(onClose) => (
          <CreateDecorPopUp
            onClose={onClose}
            title="Создать декор"
            onSave={createDecor}
            uploadImages={handleUploadImages}
          />
        )}
        deleteProduct={deleteDecor}
        updateIsActiveProduct={updateIsActiveDecor}
      />
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default AllDecorPage;
