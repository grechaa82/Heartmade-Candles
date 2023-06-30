import React, { useState, useEffect } from "react";

import ProductsGrid from "../modules/ProductsGrid";
import { Decor } from "../types/Decor";
import { DecorRequest } from "../types/Requests/DecorRequest";
import CreateDecorPopUp from "../components/PopUp/CreateDecorPopUp";

import { createDecor, getDecors } from "../Api";

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
    await createDecor(decorRequest);
    const updatedDecors = await getDecors();
    setDecorsData(updatedDecors);
  };

  useEffect(() => {
    async function fetchDecors() {
      const data = await getDecors();
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
      />
    </>
  );
};

export default AllDecorsPage;
