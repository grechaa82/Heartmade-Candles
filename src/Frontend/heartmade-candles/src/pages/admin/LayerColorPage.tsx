import { FC, useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import MainInfoLayerColor from "../../modules/admin/MainInfoLayerColor";
import { LayerColor } from "../../types/LayerColor";
import { LayerColorRequest } from "../../types/Requests/LayerColorRequest";

import { LayerColorsApi } from "../../services/LayerColorsApi";

type LayerColorParams = {
  id: string;
};

const LayerColorPage: FC = () => {
  const { id } = useParams<LayerColorParams>();
  const [layerColorData, setLayerColorData] = useState<LayerColor>();

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
        imageURL: updatedItem.imageURL,
        isActive: updatedItem.isActive,
      };
      await LayerColorsApi.update(id, layerColorRequest);
    }
  };

  useEffect(() => {
    async function fetchLayerColors() {
      if (id) {
        const data = await LayerColorsApi.getById(id);
        setLayerColorData(data);
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
            handleChangesLayerColor={handleChangesLayerColor}
            onSave={updateLayerColor}
          />
        )}
      </div>
    </>
  );
};

export default LayerColorPage;
