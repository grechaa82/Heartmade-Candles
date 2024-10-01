import { FC, useState } from 'react';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import CreateWickPopUp from '../../modules/admin/PopUp/Wick/CreateWickPopUp';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useWicksQuery from '../../hooks/useWicksQuery';

import { ImagesApi } from '../../services/ImagesApi';

import Style from './AllWickPage.module.css';

export interface AllWickPageProps {}

const AllWickPage: FC<AllWickPageProps> = () => {
  const { data, isLoading, createWick, deleteWick, updateIsActiveWick } =
    useWicksQuery();
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
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default AllWickPage;
