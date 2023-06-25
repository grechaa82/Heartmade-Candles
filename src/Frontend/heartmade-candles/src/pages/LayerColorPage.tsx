import React, { useState, useEffect } from "react";
import ProductsGrid from "../modules/ProductsGrid";
import { LayerColor } from "../types/LayerColor";
import { getLayerColors } from "../Api";

export interface LayerColorPageProps {}

const LayerColorPage: React.FC<LayerColorPageProps> = () => {
  const [layerColorData, setLayerColorData] = useState<LayerColor[]>([]);

  useEffect(() => {
    async function fetchLayerColors() {
      const data = await getLayerColors();
      setLayerColorData(data);
    }
    fetchLayerColors();
  }, []);

  return (
    <>
      <ProductsGrid data={layerColorData} title="Слои" pageUrl="layerColors" />
    </>
  );
};

export default LayerColorPage;
