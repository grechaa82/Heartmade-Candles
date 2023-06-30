import React, { useState, useEffect } from "react";
import ProductsGrid from "../modules/ProductsGrid";
import { Wick } from "../types/Wick";
import { getWicks } from "../Api";

export interface AllWickPageProps {}

const AllWickPage: React.FC<AllWickPageProps> = () => {
  const [wicksData, setWicksData] = useState<Wick[]>([]);

  useEffect(() => {
    async function fetchWicks() {
      const data = await getWicks();
      setWicksData(data);
    }
    fetchWicks();
  }, []);

  return (
    <>
      <ProductsGrid data={wicksData} title="Фитили" pageUrl="wicks" />
    </>
  );
};

export default AllWickPage;
