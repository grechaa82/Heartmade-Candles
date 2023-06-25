import React, { useState, useEffect } from "react";
import ProductsGrid from "../modules/ProductsGrid";
import { Wick } from "../types/Wick";
import { getWicks } from "../Api";

export interface WickPageProps {}

const WickPage: React.FC<WickPageProps> = () => {
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

export default WickPage;
