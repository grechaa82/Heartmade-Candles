import React, { useState, useEffect } from 'react';
import ProductsGrid from '../modules/ProductsGrid';
import { Decor } from '../types/Decor';

export interface DecorPageProps {}

const DecorPage: React.FC<DecorPageProps> = () => {
  const [decorsData, setDecorsData] = useState<Decor[]>([]);

  useEffect(() => {
    async function fetchDecors() {
      const response = await fetch(`http://localhost:5000/api/admin/decors/`);
      const data = await response.json();
      setDecorsData(data);
    }
    fetchDecors();
  }, []);

  return (
    <div>
      <ProductsGrid data={decorsData} title="Декоры" />
    </div>
  );
};

export default DecorPage;
