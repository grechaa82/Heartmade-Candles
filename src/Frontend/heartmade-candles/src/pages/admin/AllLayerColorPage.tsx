import React, { useState, useEffect } from "react";

import ProductsGrid from "../../modules/admin/ProductsGrid";
import { LayerColor } from "../../types/LayerColor";
import { LayerColorRequest } from "../../types/Requests/LayerColorRequest";
import CreateLayerColorPopUp from "../../components/admin/PopUp/CreateLayerColorPopUp";

import { LayerColorsApi } from "../../services/LayerColorsApi";

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
    await LayerColorsApi.create(layerColorRequest);
    const updatedLayerColors = await LayerColorsApi.getAll();
    setLayerColorData(updatedLayerColors);
  };

  const handleDeleteLayerColor = async (id: string) => {
    LayerColorsApi.delete(id);
    const updatedCandles = await LayerColorsApi.getAll();
    setLayerColorData(updatedCandles);
  };

  useEffect(() => {
    async function fetchLayerColors() {
      const data = await LayerColorsApi.getAll();
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
        deleteProduct={handleDeleteLayerColor}
      />
    </>
  );
};

export default AllLayerColorPage;
