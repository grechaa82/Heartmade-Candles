import { FC, useState, useEffect } from "react";
import ProductsGrid from "../modules/ProductsGrid";
import { Candle } from "../types/Candle";
import { NumberOfLayer } from "../types/NumberOfLayer";
import { TypeCandle } from "../types/TypeCandle";
import TagsGrid from "../modules/TagsGrid";
import { getCandle, getNumberOfLayers, getTypeCandles } from "../Api";
import { convertToTagData } from "./CandleDetailsPage";
import { TagData } from "../components/Tag";

export interface CandlePageProps {}

const CandlePage: FC<CandlePageProps> = () => {
  const [typeCandlesData, setTypeCandlesData] = useState<TypeCandle[]>([]);
  const [numberOfLayersData, setNumberOfLayersData] = useState<NumberOfLayer[]>(
    []
  );
  const [candlesData, setCandlesData] = useState<Candle[]>([]);

  useEffect(() => {
    async function fetchTypeCandles() {
      try {
        const data = await getTypeCandles();
        setTypeCandlesData(data);
      } catch (error) {
        console.error("Произошла ошибка при загрузке типов свечей:", error);
      }
    }

    async function fetchNumberOfLayers() {
      try {
        const data = await getNumberOfLayers();
        setNumberOfLayersData(data);
      } catch (error) {
        console.error("Произошла ошибка при загрузке количества слоев:", error);
      }
    }

    async function fetchCandles() {
      try {
        const data = await getCandle();
        setCandlesData(data);
      } catch (error) {
        console.error("Произошла ошибка при загрузке свечей:", error);
      }
    }

    fetchTypeCandles();
    fetchNumberOfLayers();
    fetchCandles();
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
      <ProductsGrid data={candlesData} title="Свечи" />
    </>
  );
};

export default CandlePage;

export function convertCandlesToTagData(candles: TypeCandle[]): TagData[] {
  const tagDataArray: TagData[] = [];

  for (let i = 0; i < candles.length; i++) {
    const candle = candles[i];
    const tagData: TagData = {
      id: candle.id,
      text: candle.title,
    };

    tagDataArray.push(tagData);
  }

  return tagDataArray;
}
