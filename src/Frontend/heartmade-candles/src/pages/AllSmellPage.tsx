import React, { useState, useEffect } from "react";

import ProductsGrid from "../modules/ProductsGrid";
import { Smell } from "../types/Smell";
import { SmellRequest } from "../types/Requests/SmellRequest";
import CreateSmellPopUp from "../components/PopUp/CreateSmellPopUp";

import { getSmells, createSmell, deleteSmell } from "../Api";

export interface AllSmellPageProps {}

const AllSmellPage: React.FC<AllSmellPageProps> = () => {
  const [smellsData, setSmellsData] = useState<Smell[]>([]);

  const handleCreateSmell = async (createdItem: Smell) => {
    const smellRequest: SmellRequest = {
      title: createdItem.title,
      description: createdItem.description,
      price: createdItem.price,
      isActive: createdItem.isActive,
    };
    await createSmell(smellRequest);
    const updatedSmells = await getSmells();
    setSmellsData(updatedSmells);
  };

  const handleDeleteSmell = async (id: string) => {
    deleteSmell(id);
    const updatedCandles = await getSmells();
    setSmellsData(updatedCandles);
  };

  useEffect(() => {
    async function fetchSmells() {
      const data = await getSmells();
      setSmellsData(data);
    }
    fetchSmells();
  }, []);

  return (
    <>
      <ProductsGrid
        data={smellsData}
        title="Запахи"
        pageUrl="smells"
        popUpComponent={
          <CreateSmellPopUp
            onClose={() => console.log("Popup closed")}
            title="Создать запах"
            onSave={handleCreateSmell}
          />
        }
        deleteProduct={handleDeleteSmell}
      />
    </>
  );
};

export default AllSmellPage;
