import { FC, useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import MainInfoLayerColor from "../modules/MainInfoLayerColor";
import { LayerColor } from "../types/LayerColor";
import { LayerColorRequest } from "../types/Requests/LayerColorRequest";

import { getLayerColorById, putLayerColor } from "../Api";

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

  const updateLayerColor = (updatedItem: LayerColor) => {
    if (id) {
      const layerColorRequest: LayerColorRequest = {
        title: updatedItem.title,
        description: updatedItem.description,
        pricePerGram: updatedItem.pricePerGram,
        imageURL: updatedItem.imageURL,
        isActive: updatedItem.isActive,
      };
      putLayerColor(id, layerColorRequest);
    }
  };

  useEffect(() => {
    async function fetchLayerColors() {
      if (id) {
        const data = await getLayerColorById(id);
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
