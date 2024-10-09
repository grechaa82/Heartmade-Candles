import { FC, useEffect, useState, useContext } from 'react';
import { useInView } from 'react-intersection-observer';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import CreateDecorPopUp from '../../modules/admin/PopUp/Decor/CreateDecorPopUp';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useDecorsQuery from '../../hooks/admin/useDecorsQuery';
import { AuthContext } from '../../contexts/AuthContext';

import { ImagesApi } from '../../services/ImagesApi';

import Style from './AllDecorPage.module.css';

export interface AllDecorPageProps {}

const AllDecorPage: FC<AllDecorPageProps> = () => {
  const { isAuth } = useContext(AuthContext);
  const {
    data,
    isLoading,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    createDecor,
    deleteDecor,
    updateIsActiveDecor,
  } = useDecorsQuery(21, isAuth);
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
      {isFetchingNextPage ? (
        <span>...Loading</span>
      ) : (
        hasNextPage && <div ref={ref}></div>
      )}
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default AllDecorPage;
