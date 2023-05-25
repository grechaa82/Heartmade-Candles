import React, { useState, useEffect } from 'react';
import ProductsGrid from '../modules/ProductsGrid';
import { LayerColor } from '../types/LayerColor';

export interface LayerColorPageProps {}

const LayerColorPage: React.FC<LayerColorPageProps> = () => {
  const [layerColorData, setLayerColorData] = useState<LayerColor[]>([]);

  useEffect(() => {
    async function fetchLayerColors() {
      const response = await fetch(`http://localhost:5000/api/admin/layerColors/`);
      const data = await response.json();
      setLayerColorData(data);
    }
    fetchLayerColors();
  }, []);

  return (
    <div>
      <ProductsGrid data={layerColorData} title="Слои" />
    </div>
  );
};

export default LayerColorPage;
