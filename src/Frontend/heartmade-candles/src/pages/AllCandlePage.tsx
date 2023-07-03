import { FC, useState, useEffect } from "react";

import ProductsGrid from "../modules/ProductsGrid";
import { Candle } from "../types/Candle";
import { NumberOfLayer } from "../types/NumberOfLayer";
import { TypeCandle } from "../types/TypeCandle";
import TagsGrid from "../modules/TagsGrid";
import { convertToTagData } from "./CandleDetailsPage";
import { TagData } from "../components/Tag";
import CreateCandlePopUp from "../components/PopUp/CreateCandlePopUp";
import { CandleRequest } from "../types/Requests/CandleRequest";

import {
  getCandle,
  getNumberOfLayers,
  getTypeCandles,
  createCandle,
  deleteCandle,
} from "../Api";

export interface AllCandlePageProps {}

const AllCandlePage: FC<AllCandlePageProps> = () => {
  const [typeCandlesData, setTypeCandlesData] = useState<TypeCandle[]>([]);
  const [numberOfLayersData, setNumberOfLayersData] = useState<NumberOfLayer[]>(
    []
  );
  const [candlesData, setCandlesData] = useState<Candle[]>([]);

  const handleCreateCandle = async (createdItem: Candle) => {
    const candleRequest: CandleRequest = {
      title: createdItem.title,
      description: createdItem.description,
      price: createdItem.price,
      weightGrams: createdItem.weightGrams,
      imageURL: createdItem.imageURL,
      typeCandle: createdItem.typeCandle,
      isActive: createdItem.isActive,
    };
    await createCandle(candleRequest);
    const updatedCandles = await getCandle();
    setCandlesData(updatedCandles);
  };

  const handleDeleteCandle = async (id: string) => {
    deleteCandle(id);
    const updatedCandles = await getCandle();
    setCandlesData(updatedCandles);
  };

  useEffect(() => {
    async function fetchData() {
      try {
        const typeCandles = await getTypeCandles();
        setTypeCandlesData(typeCandles);

        const numberOfLayers = await getNumberOfLayers();
        setNumberOfLayersData(numberOfLayers);

        const candles = await getCandle();
        setCandlesData(candles);
      } catch (error) {
        console.error("Произошла ошибка при загрузке данных:", error);
      }
    }

    fetchData();
  }, []);

  return (
    <>
      <TagsGrid
        title="Типы свечей"
        tags={convertCandlesToTagData(typeCandlesData)}
      />
      <TagsGrid
        title="Количество слоев"
        tags={convertToTagData(numberOfLayersData)}
      />
      <ProductsGrid
        data={candlesData}
        title="Свечи"
        pageUrl="candles"
        popUpComponent={
          <CreateCandlePopUp
            onClose={() => console.log("Popup closed")}
            title="Создать свечу"
            typeCandlesArray={typeCandlesData}
            onSave={handleCreateCandle}
          />
        }
        deleteProduct={handleDeleteCandle}
      />
    </>
  );
};

export default AllCandlePage;

export function convertCandlesToTagData(candles: TypeCandle[]): TagData[] {
  return candles.map((candle) => ({
    id: candle.id,
    text: candle.title,
  }));
}
