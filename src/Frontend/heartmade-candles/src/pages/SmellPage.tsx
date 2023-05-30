import React, { useState, useEffect } from 'react';
import ProductsGrid from '../modules/ProductsGrid';
import { Smell } from '../types/Smell';
import { getSmells } from '../Api';

export interface SmellPageProps {}

const SmellPage: React.FC<SmellPageProps> = () => {
  const [smellsData, setSmellsData] = useState<Smell[]>([]);

  useEffect(() => {
    async function fetchSmells() {
      const data = await getSmells();
      setSmellsData(data);
    }
    fetchSmells();
  }, []);

  return (
    <div>
      <ProductsGrid data={smellsData} title="Запахи" />
    </div>
  );
};

export default SmellPage;
