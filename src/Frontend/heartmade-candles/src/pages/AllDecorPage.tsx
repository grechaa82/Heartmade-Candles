import React, { useState, useEffect } from "react";
import ProductsGrid from "../modules/ProductsGrid";
import { Decor } from "../types/Decor";
import { getDecors } from "../Api";

export interface AllDecorPageProps {}

const AllDecorsPage: React.FC<AllDecorPageProps> = () => {
  const [decorsData, setDecorsData] = useState<Decor[]>([]);

  useEffect(() => {
    async function fetchDecors() {
      const data = await getDecors();
      setDecorsData(data);
    }
    fetchDecors();
  }, []);

  return (
    <>
      <ProductsGrid data={decorsData} title="Декоры" pageUrl="decors" />
    </>
  );
};

export default AllDecorsPage;
