import React, { useState, useEffect } from 'react';
import ProductsGrid from '../modules/ProductsGrid';
import { Wick } from '../types/Wick';

export interface WickPageProps {}

const WickPage: React.FC<WickPageProps> = () => {
  const [wicksData, setWicksData] = useState<Wick[]>([]);

  useEffect(() => {
    async function fetchWicks() {
      const response = await fetch(`http://localhost:5000/api/admin/wicks/`);
      const data = await response.json();
      setWicksData(data);
    }
    fetchWicks();
  }, []);

  return (
    <div>
      <ProductsGrid data={wicksData} title="Фитили" />
    </div>
  );
};

export default WickPage;
