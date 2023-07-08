import React, { useState, useEffect } from "react";

import ProductsGrid from "../modules/ProductsGrid";
import { Decor } from "../types/Decor";
import { DecorRequest } from "../types/Requests/DecorRequest";
import CreateDecorPopUp from "../components/PopUp/CreateDecorPopUp";

import { DecorsApi } from "../services/DecorsApi";

export interface AllDecorPageProps {}

const AllDecorsPage: React.FC<AllDecorPageProps> = () => {
  const [decorsData, setDecorsData] = useState<Decor[]>([]);

  const handleCreateDecor = async (createdItem: Decor) => {
    const decorRequest: DecorRequest = {
      title: createdItem.title,
      description: createdItem.description,
      price: createdItem.price,
      imageURL: createdItem.imageURL,
      isActive: createdItem.isActive,
    };
    await DecorsApi.create(decorRequest);
    const updatedDecors = await DecorsApi.getAll();
    setDecorsData(updatedDecors);
  };

  const handleDeleteSmell = async (id: string) => {
    DecorsApi.delete(id);
    const updatedCandles = await DecorsApi.getAll();
    setDecorsData(updatedCandles);
  };

  useEffect(() => {
    async function fetchDecors() {
      const data = await DecorsApi.getAll();
      setDecorsData(data);
    }
    fetchDecors();
  }, []);

  return (
    <>
      <ProductsGrid
        data={decorsData}
        title="Декоры"
        pageUrl="decors"
        popUpComponent={
          <CreateDecorPopUp
            onClose={() => console.log("Popup closed")}
            title="Создать декор"
            onSave={handleCreateDecor}
          />
        }
        deleteProduct={handleDeleteSmell}
      />
    </>
  );
};

export default AllDecorsPage;
