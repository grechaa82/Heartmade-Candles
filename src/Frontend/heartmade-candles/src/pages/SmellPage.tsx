import React, { useState, useEffect } from 'react';
import ProductsGrid from '../modules/ProductsGrid';
import { Smell } from '../types/Smell';

export interface SmellPageProps {}

const SmellPage: React.FC<SmellPageProps> = () => {
  const [smellsData, setSmellsData] = useState<Smell[]>([]);

  useEffect(() => {
    async function fetchSmells() {
      const response = await fetch(`http://localhost:5000/api/admin/smells/`);
      const data = await response.json();
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
