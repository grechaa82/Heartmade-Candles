import React, { useState, useEffect } from "react";

import ProductsGrid from "../modules/ProductsGrid";
import { Smell } from "../types/Smell";
import { SmellRequest } from "../types/Requests/SmellRequest";
import CreateSmellPopUp from "../components/PopUp/CreateSmellPopUp";

import { SmellsApi } from "../services/SmellsApi";

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
    await SmellsApi.create(smellRequest);
    const updatedSmells = await SmellsApi.getAll();
    setSmellsData(updatedSmells);
  };

  const handleDeleteSmell = async (id: string) => {
    SmellsApi.delete(id);
    const updatedCandles = await SmellsApi.getAll();
    setSmellsData(updatedCandles);
  };

  useEffect(() => {
    async function fetchSmells() {
      const data = await SmellsApi.getAll();
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
