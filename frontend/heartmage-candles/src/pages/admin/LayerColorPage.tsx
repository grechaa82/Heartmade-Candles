import { FC, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

import MainInfoLayerColor from '../../modules/admin/MainInfoLayerColor';
import { LayerColor } from '../../types/LayerColor';
import { LayerColorRequest } from '../../types/Requests/LayerColorRequest';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';

import { LayerColorsApi } from '../../services/LayerColorsApi';

import Style from './LayerColorPage.module.css';

type LayerColorParams = {
  id: string;
};

const LayerColorPage: FC = () => {
  const { id } = useParams<LayerColorParams>();
  const [layerColorData, setLayerColorData] = useState<LayerColor>();

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const handleChangesLayerColor = (updatedLayerColor: LayerColor) => {
    setLayerColorData((prevLayerColorData) => ({
      ...prevLayerColorData,
      ...updatedLayerColor,
    }));
  };

  const updateLayerColor = async (updatedItem: LayerColor) => {
    if (id) {
      const layerColorRequest: LayerColorRequest = {
        title: updatedItem.title,
        description: updatedItem.description,
        pricePerGram: updatedItem.pricePerGram,
        images: updatedItem.images,
        isActive: updatedItem.isActive,
      };

      const updatedLayerColorResponse = await LayerColorsApi.update(
        id,
        layerColorRequest
      );
      if (updatedLayerColorResponse.error) {
        setErrorMessage([
          ...errorMessage,
          updatedLayerColorResponse.error as string,
        ]);
      }
    }
  };

  useEffect(() => {
    async function fetchLayerColors() {
      if (id) {
        const layerColorResponse = await LayerColorsApi.getById(id);
        if (layerColorResponse.data && !layerColorResponse.error) {
          setLayerColorData(layerColorResponse.data);
        } else {
          setErrorMessage([
            ...errorMessage,
            layerColorResponse.error as string,
          ]);
        }
      }
    }

    fetchLayerColors();
  }, [id]);

  return (
    <>
      <div className="layerColors">
        {layerColorData && (
          <MainInfoLayerColor
            data={layerColorData}
            onChangesLayerColor={handleChangesLayerColor}
            onSave={updateLayerColor}
          />
        )}
      </div>
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default LayerColorPage;
