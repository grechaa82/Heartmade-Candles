import React, { useState, useEffect } from 'react';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import { Wick } from '../../types/Wick';
import { WickRequest } from '../../types/Requests/WickRequest';
import CreateWickPopUp from '../../components/admin/PopUp/CreateWickPopUp';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';

import { WicksApi } from '../../services/WicksApi';
import { ImagesApi } from '../../services/ImagesApi';

import Style from './AllWickPage.module.css';

export interface AllWickPageProps {}

const AllWickPage: React.FC<AllWickPageProps> = () => {
  const [wicksData, setWicksData] = useState<Wick[]>([]);

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const handleCreateWick = async (createdItem: Wick) => {
    const wickRequest: WickRequest = {
      title: createdItem.title,
      description: createdItem.description,
      price: createdItem.price,
      images: createdItem.images,
      isActive: createdItem.isActive,
    };
    const response = await WicksApi.create(wickRequest);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedWicksResponse = await WicksApi.getAll();
      if (updatedWicksResponse.data && !updatedWicksResponse.error) {
        setWicksData(updatedWicksResponse.data);
      } else {
        setErrorMessage([
          ...errorMessage,
          updatedWicksResponse.error as string,
        ]);
      }
    }
  };

  const handleDeleteWick = async (id: string) => {
    const response = await WicksApi.delete(id);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedWicksResponse = await WicksApi.getAll();
      if (updatedWicksResponse.data && !updatedWicksResponse.error) {
        setWicksData(updatedWicksResponse.data);
      } else {
        setErrorMessage([
          ...errorMessage,
          updatedWicksResponse.error as string,
        ]);
      }
    }
  };

  const handleUpdateIsActiveWick = async (id: string) => {
    const wick = wicksData.find((x) => x.id === parseInt(id));
    const newWickRequest: WickRequest = {
      title: wick.title,
      description: wick.description,
      price: wick.price,
      images: wick.images,
      isActive: !wick.isActive,
    };

    const response = await WicksApi.update(wick.id.toString(), newWickRequest);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedWicksResponse = await WicksApi.getAll();
      if (updatedWicksResponse.data && !updatedWicksResponse.error) {
        setWicksData(updatedWicksResponse.data);
      } else {
        setErrorMessage([
          ...errorMessage,
          updatedWicksResponse.error as string,
        ]);
      }
    }
  };

  useEffect(() => {
    async function fetchWicks() {
      const wicksResponse = await WicksApi.getAll();
      if (wicksResponse.data && !wicksResponse.error) {
        setWicksData(wicksResponse.data);
      } else {
        setErrorMessage([...errorMessage, wicksResponse.error as string]);
      }
    }
    fetchWicks();
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
        data={wicksData}
        title="Фитили"
        pageUrl="wicks"
        popUpComponent={
          <CreateWickPopUp
            onClose={() => console.log('Popup closed')}
            title="Создать фитиль"
            onSave={handleCreateWick}
            uploadImages={handleUploadImages}
          />
        }
        deleteProduct={handleDeleteWick}
        updateIsActiveProduct={handleUpdateIsActiveWick}
      />
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default AllWickPage;
