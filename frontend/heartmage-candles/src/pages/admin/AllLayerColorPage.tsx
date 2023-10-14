import React, { useState, useEffect } from 'react';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import { LayerColor } from '../../types/LayerColor';
import { LayerColorRequest } from '../../types/Requests/LayerColorRequest';
import CreateLayerColorPopUp from '../../components/admin/PopUp/CreateLayerColorPopUp';
import ListErrorPopUp from '../../modules/constructor/ListErrorPopUp';

import { LayerColorsApi } from '../../services/LayerColorsApi';

import Style from './AllLayerColorPage.module.css';

export interface AllLayerColorPageProps {}

const AllLayerColorPage: React.FC<AllLayerColorPageProps> = () => {
  const [layerColorData, setLayerColorData] = useState<LayerColor[]>([]);

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const handleCreateLayerColor = async (createdItem: LayerColor) => {
    const layerColorRequest: LayerColorRequest = {
      title: createdItem.title,
      description: createdItem.description,
      pricePerGram: createdItem.pricePerGram,
      images: createdItem.images,
      isActive: createdItem.isActive,
    };
    const response = await LayerColorsApi.create(layerColorRequest);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedLayerColorsResponse = await LayerColorsApi.getAll();
      if (updatedLayerColorsResponse.data && !updatedLayerColorsResponse.error) {
        setLayerColorData(updatedLayerColorsResponse.data);
      } else {
        setErrorMessage([...errorMessage, updatedLayerColorsResponse.error as string]);
      }
    }
  };

  const handleDeleteLayerColor = async (id: string) => {
    const response = await LayerColorsApi.delete(id);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedLayerColorsResponse = await LayerColorsApi.getAll();
      if (updatedLayerColorsResponse.data && !updatedLayerColorsResponse.error) {
        setLayerColorData(updatedLayerColorsResponse.data);
      } else {
        setErrorMessage([...errorMessage, updatedLayerColorsResponse.error as string]);
      }
    }
  };

  useEffect(() => {
    async function fetchLayerColors() {
      const layerColorsResponse = await LayerColorsApi.getAll();
      if (layerColorsResponse.data && !layerColorsResponse.error) {
        setLayerColorData(layerColorsResponse.data);
      } else {
        setErrorMessage([...errorMessage, layerColorsResponse.error as string]);
      }
    }
    fetchLayerColors();
  }, []);

  return (
    <>
      <ProductsGrid
        data={layerColorData}
        title="Слои"
        pageUrl="layerColors"
        popUpComponent={
          <CreateLayerColorPopUp
            onClose={() => console.log('Popup closed')}
            title="Создать слой"
            onSave={handleCreateLayerColor}
          />
        }
        deleteProduct={handleDeleteLayerColor}
      />
      <div className={Style.popUpNotification}>
        <ListErrorPopUp messages={errorMessage} />
      </div>
    </>
  );
};

export default AllLayerColorPage;
