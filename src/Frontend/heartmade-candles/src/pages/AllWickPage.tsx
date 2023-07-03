import React, { useState, useEffect } from "react";

import ProductsGrid from "../modules/ProductsGrid";
import { Wick } from "../types/Wick";
import { WickRequest } from "../types/Requests/WickRequest";
import CreateWickPopUp from "../components/PopUp/CreateWickPopUp";

import { getWicks, createWick, deleteWick } from "../Api";

export interface AllWickPageProps {}

const AllWickPage: React.FC<AllWickPageProps> = () => {
  const [wicksData, setWicksData] = useState<Wick[]>([]);

  const handleCreateWick = async (createdItem: Wick) => {
    const wickRequest: WickRequest = {
      title: createdItem.title,
      description: createdItem.description,
      price: createdItem.price,
      imageURL: createdItem.imageURL,
      isActive: createdItem.isActive,
    };
    await createWick(wickRequest);
    const updatedWicks = await getWicks();
    setWicksData(updatedWicks);
  };

  const handleDeleteWick = async (id: string) => {
    deleteWick(id);
    const updatedCandles = await getWicks();
    setWicksData(updatedCandles);
  };

  useEffect(() => {
    async function fetchWicks() {
      const data = await getWicks();
      setWicksData(data);
    }
    fetchWicks();
  }, []);

  return (
    <>
      <ProductsGrid
        data={wicksData}
        title="Фитили"
        pageUrl="wicks"
        popUpComponent={
          <CreateWickPopUp
            onClose={() => console.log("Popup closed")}
            title="Создать фитиль"
            onSave={handleCreateWick}
          />
        }
        deleteProduct={handleDeleteWick}
      />
    </>
  );
};

export default AllWickPage;
