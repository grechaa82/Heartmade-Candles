import { FC, useState, useEffect } from "react";

import ProductsGrid from "../modules/ProductsGrid";
import { Candle } from "../types/Candle";
import { NumberOfLayer } from "../types/NumberOfLayer";
import { TypeCandle } from "../types/TypeCandle";
import TagsGrid from "../modules/TagsGrid";
import { getCandle, getNumberOfLayers, getTypeCandles } from "../Api";
import { convertToTagData } from "./CandleDetailsPage";
import { TagData } from "../components/Tag";
import CreateCandlePopUp from "../components/PopUp/CreateCandlePopUp";
import { CandleRequest } from "../types/Requests/CandleRequest";

import { createCandle } from "../Api";

export interface CandlePageProps {}

const CandlePage: FC<CandlePageProps> = () => {
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
      />
    </>
  );
};

export default CandlePage;

export function convertCandlesToTagData(candles: TypeCandle[]): TagData[] {
  return candles.map((candle) => ({
    id: candle.id,
    text: candle.title,
  }));
}
