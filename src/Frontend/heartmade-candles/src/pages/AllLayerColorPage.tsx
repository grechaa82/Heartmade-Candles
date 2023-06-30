import React, { useState, useEffect } from "react";

import ProductsGrid from "../modules/ProductsGrid";
import { LayerColor } from "../types/LayerColor";
import { LayerColorRequest } from "../types/Requests/LayerColorRequest";
import CreateLayerColorPopUp from "../components/PopUp/CreateLayerColorPopUp";

import { getLayerColors, createLayerColor } from "../Api";

export interface AllLayerColorPageProps {}

const AllLayerColorPage: React.FC<AllLayerColorPageProps> = () => {
  const [layerColorData, setLayerColorData] = useState<LayerColor[]>([]);

  const handleCreateLayerColor = async (createdItem: LayerColor) => {
    const layerColorRequest: LayerColorRequest = {
      title: createdItem.title,
      description: createdItem.description,
      pricePerGram: createdItem.pricePerGram,
      imageURL: createdItem.imageURL,
      isActive: createdItem.isActive,
    };
    await createLayerColor(layerColorRequest);
    const updatedLayerColors = await getLayerColors();
    setLayerColorData(updatedLayerColors);
  };

  useEffect(() => {
    async function fetchLayerColors() {
      const data = await getLayerColors();
      setLayerColorData(data);
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
            onClose={() => console.log("Popup closed")}
            title="Создать слой"
            onSave={handleCreateLayerColor}
          />
        }
      />
    </>
  );
};

export default AllLayerColorPage;
