import { FC, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import MainInfoCandles, { FetchTypeCandles } from "../modules/MainInfoCandles";
import { CandleDetail } from "../types/CandleDetail";
import ProductsGrid, { FetchProducts } from "../modules/ProductsGrid";
import TagsGrid from "../modules/TagsGrid";
import {
  getCandleById,
  getDecors,
  getLayerColors,
  getNumberOfLayers,
  getSmells,
  getTypeCandles,
  getWicks,
  putCandle,
  putCandleDecors,
  putCandleLayerColors,
  putCandleNumberOfLayer,
  putCandleSmells,
  putCandleWicks,
} from "../Api";
import { Candle } from "../types/Candle";
import { Decor } from "../types/Decor";
import { NumberOfLayer } from "../types/NumberOfLayer";
import { LayerColor } from "../types/LayerColor";
import { Smell } from "../types/Smell";
import { Wick } from "../types/Wick";
import { BaseProduct } from "../types/BaseProduct";

import Style from "./CandleDetailsPage.module.css";
import { TagData } from "../components/Tag";

type CandleDetailsParams = {
  id: string;
};

const CandleDetailsPage: FC = () => {
  const { id } = useParams<CandleDetailsParams>();
  const [candleDetailData, setCandleDetailData] = useState<CandleDetail>();
  const [numberOfLayerTagData, setNumberOfLayerTagData] = useState<TagData[]>();

  const fetchTypeCandles: FetchTypeCandles = async () => {
    try {
      const data = await getTypeCandles();
      return data;
    } catch (error) {
      console.error("Произошла ошибка при загрузке типов свечей:", error);
      return [];
    }
  };

  const fetchDecors: FetchProducts<Decor> = async () => {
    try {
      const data = await getDecors();
      return data;
    } catch (error) {
      console.error("Произошла ошибка при загрузке типов свечей:", error);
      return [];
    }
  };

  const fetchLayerColors: FetchProducts<LayerColor> = async () => {
    try {
      const data = await getLayerColors();
      return data;
    } catch (error) {
      console.error("Произошла ошибка при загрузке типов свечей:", error);
      return [];
    }
  };

  const fetchNumberOfLayer = async (): Promise<NumberOfLayer[]> => {
    try {
      const data = await getNumberOfLayers();
      return data;
    } catch (error) {
      console.error("Произошла ошибка при загрузке типов свечей:", error);
      return [];
    }
  };

  const fetchSmells: FetchProducts<Smell> = async () => {
    try {
      const data = await getSmells();
      return data;
    } catch (error) {
      console.error("Произошла ошибка при загрузке типов свечей:", error);
      return [];
    }
  };

  const fetchWicks: FetchProducts<Wick> = async () => {
    try {
      const data = await getWicks();
      return data;
    } catch (error) {
      console.error("Произошла ошибка при загрузке типов свечей:", error);
      return [];
    }
  };

  const handleChangesCandle = (updatedcandle: Candle) => {
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData,
      candle: {
        ...candleDetailData?.candle,
        ...updatedcandle,
      },
    };

    setCandleDetailData(newCandleDetailData);
  };

  const handleChangesDecors = (updatedItems: BaseProduct[]) => {
    const updatedDecors = updatedItems as Decor[];
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      decors: updatedDecors,
    };

    setCandleDetailData(newCandleDetailData);
  };

  const handleChangesLayerColors = (updatedItems: BaseProduct[]) => {
    const updatedLayerColors = updatedItems as LayerColor[];
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      layerColors: updatedLayerColors,
    };

    setCandleDetailData(newCandleDetailData);
  };

  const handleChangesNumberOfLayers = (
    updatedNumberOfLayers: TagData[]
  ): void => {
    const numberOfLayers: NumberOfLayer[] = updatedNumberOfLayers.map(
      (tagData) => ({
        id: tagData.id,
        number: parseInt(tagData.text),
      })
    );

    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      numberOfLayers: numberOfLayers,
    };

    setCandleDetailData(newCandleDetailData);
  };

  const handleChangesSmells = (updatedItems: BaseProduct[]) => {
    const updatedSmells = updatedItems as Smell[];
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      smells: updatedSmells,
    };

    setCandleDetailData(newCandleDetailData);
  };

  const handleChangesWicks = (updatedItems: BaseProduct[]) => {
    const updatedWicks = updatedItems as Wick[];
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      wicks: updatedWicks,
    };

    setCandleDetailData(newCandleDetailData);
  };

  const updateCandle = (updatedItem: Candle) => {
    if (id) {
      const candleRequest = {
        title: updatedItem.title,
        description: updatedItem.description,
        price: updatedItem.price,
        weightGrams: updatedItem.weightGrams,
        imageURL: updatedItem.imageURL,
        typeCandle: updatedItem.typeCandle,
        isActive: updatedItem.isActive,
      };
      putCandle(id, candleRequest);
    }
  };

  const updateCandleDecors = (updatedItems: BaseProduct[]) => {
    if (id) {
      const updatedDecors = updatedItems as Decor[];
      const ids = updatedDecors.map((d) => d.id);
      putCandleDecors(id, ids);
    }
  };

  const updateCandleLayerColors = (updatedItems: BaseProduct[]) => {
    if (id) {
      const updatedLayerColors = updatedItems as LayerColor[];
      const ids = updatedLayerColors.map((l) => l.id);
      putCandleLayerColors(id, ids);
    }
  };

  const updateCandleNumberOfLayers = (updatedItems: TagData[]) => {
    if (id) {
      const ids = updatedItems.map((n) => n.id);
      putCandleNumberOfLayer(id, ids);
    }
  };

  const updateCandleSmells = (updatedItems: BaseProduct[]) => {
    if (id) {
      const updatedSmells = updatedItems as Smell[];
      const ids = updatedSmells.map((s) => s.id);
      putCandleSmells(id, ids);
    }
  };

  const updateCandleWicks = (updatedItems: BaseProduct[]) => {
    if (id) {
      const updatedWicks = updatedItems as Wick[];
      const ids = updatedWicks.map((w) => w.id);
      putCandleWicks(id, ids);
    }
  };

  useEffect(() => {
    async function fetchCandle() {
      if (id) {
        const data = await getCandleById(id);
        setCandleDetailData(data);
      }
    }

    async function fetchNumberOfLayers() {
      const data = await fetchNumberOfLayer();
      const tagData = convertToTagData(data);
      setNumberOfLayerTagData(tagData);
    }

    fetchCandle();
    fetchNumberOfLayers();
  }, [id]);

  return (
    <>
      <div className="candles">
        {candleDetailData && (
          <MainInfoCandles
            data={candleDetailData.candle}
            fetchTypeCandles={fetchTypeCandles}
            handleChangesCandle={handleChangesCandle}
            onSave={updateCandle}
          />
        )}
      </div>
      {candleDetailData?.numberOfLayers && (
        <TagsGrid
          title="Количество слоев"
          tags={convertToTagData(candleDetailData.numberOfLayers)}
          allTags={numberOfLayerTagData || []}
          onSave={updateCandleNumberOfLayers}
          onChanges={handleChangesNumberOfLayers}
        />
      )}
      {candleDetailData?.decors && (
        <ProductsGrid
          data={candleDetailData.decors}
          title="Декоры"
          fetchProducts={fetchDecors}
          handleChangesProduct={handleChangesDecors}
          onSave={updateCandleDecors}
        />
      )}
      {candleDetailData?.layerColors && (
        <ProductsGrid
          data={candleDetailData.layerColors}
          title="Слои"
          fetchProducts={fetchLayerColors}
          handleChangesProduct={handleChangesLayerColors}
          onSave={updateCandleLayerColors}
        />
      )}
      {candleDetailData?.smells && (
        <ProductsGrid
          data={candleDetailData.smells}
          title="Запахи"
          fetchProducts={fetchSmells}
          handleChangesProduct={handleChangesSmells}
          onSave={updateCandleSmells}
        />
      )}
      {candleDetailData?.wicks && (
        <ProductsGrid
          data={candleDetailData.wicks}
          title="Фитили"
          fetchProducts={fetchWicks}
          handleChangesProduct={handleChangesWicks}
          onSave={updateCandleWicks}
        />
      )}
    </>
  );
};

export default CandleDetailsPage;

export function convertToTagData(numberOfLayers: NumberOfLayer[]): TagData[] {
  const tagDataArray: TagData[] = [];

  for (let i = 0; i < numberOfLayers.length; i++) {
    const numberOfLayer = numberOfLayers[i];
    const tagData: TagData = {
      id: numberOfLayer.id,
      text: `${numberOfLayer.number}`,
    };

    tagDataArray.push(tagData);
  }

  return tagDataArray;
}
