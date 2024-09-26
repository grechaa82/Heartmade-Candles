import React, { useState, useEffect } from 'react';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import { Decor } from '../../types/Decor';
import { DecorRequest } from '../../types/Requests/DecorRequest';
import CreateDecorPopUp from '../../modules/admin/PopUp/Decor/CreateDecorPopUp';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';

import { DecorsApi } from '../../services/DecorsApi';
import { ImagesApi } from '../../services/ImagesApi';

import Style from './AllDecorPage.module.css';

export interface AllDecorPageProps {}

const AllDecorPage: React.FC<AllDecorPageProps> = () => {
  const [decorsData, setDecorsData] = useState<Decor[]>([]);

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const handleCreateDecor = async (createdItem: Decor) => {
    const decorRequest: DecorRequest = {
      title: createdItem.title,
      description: createdItem.description,
      price: createdItem.price,
      images: createdItem.images,
      isActive: createdItem.isActive,
    };
    const response = await DecorsApi.create(decorRequest);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedDecorsResponse = await DecorsApi.getAll();
      if (updatedDecorsResponse.data && !updatedDecorsResponse.error) {
        setDecorsData(updatedDecorsResponse.data);
      } else {
        setErrorMessage([
          ...errorMessage,
          updatedDecorsResponse.error as string,
        ]);
      }
    }
  };

  const handleDeleteDecor = async (id: string) => {
    const response = await DecorsApi.delete(id);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedDecorsResponse = await DecorsApi.getAll();
      if (updatedDecorsResponse.data && !updatedDecorsResponse.error) {
        setDecorsData(updatedDecorsResponse.data);
      } else {
        setErrorMessage([
          ...errorMessage,
          updatedDecorsResponse.error as string,
        ]);
      }
    }
  };

  const handleUpdateIsActiveDecor = async (id: string) => {
    const decor = decorsData.find((x) => x.id === parseInt(id));
    const newDecorRequest: DecorRequest = {
      title: decor.title,
      description: decor.description,
      price: decor.price,
      images: decor.images,
      isActive: !decor.isActive,
    };

    const response = await DecorsApi.update(
      decor.id.toString(),
      newDecorRequest,
    );
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedDecorsResponse = await DecorsApi.getAll();
      if (updatedDecorsResponse.data && !updatedDecorsResponse.error) {
        setDecorsData(updatedDecorsResponse.data);
      } else {
        setErrorMessage([
          ...errorMessage,
          updatedDecorsResponse.error as string,
        ]);
      }
    }
  };

  useEffect(() => {
    async function fetchDecors() {
      const decorsResponse = await DecorsApi.getAll();
      if (decorsResponse.data && !decorsResponse.error) {
        setDecorsData(decorsResponse.data);
      } else {
        setErrorMessage([...errorMessage, decorsResponse.error as string]);
      }
    }
    fetchDecors();
  }, []);

  const handleUploadImages = async (files: File[]) => {
    const imagesResponse = await ImagesApi.uploadImages(files);
    if (imagesResponse.data && !imagesResponse.error) {
      return imagesResponse.data;
    } else {
      return [];
    }
  };

  return (
    <>
      <ProductsGrid
        data={decorsData}
        title="Декоры"
        pageUrl="decors"
        renderPopUpComponent={(onClose) => (
          <CreateDecorPopUp
            onClose={onClose}
            title="Создать декор"
            onSave={handleCreateDecor}
            uploadImages={handleUploadImages}
          />
        )}
        deleteProduct={handleDeleteDecor}
        updateIsActiveProduct={handleUpdateIsActiveDecor}
      />
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default AllDecorPage;
