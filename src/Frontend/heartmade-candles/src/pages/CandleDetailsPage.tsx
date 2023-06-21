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
  getSmells,
  getTypeCandles,
  getWicks,
  putCandleDecors,
  putCandleLayerColors,
  putCandleSmells,
  putCandleWicks,
} from "../Api";
import { UpdateCandleDetailsRequest } from "../types/Requests/UpdateCandleDetailsRequest";
import { CandleRequest } from "../types/Requests/CandleRequest";
import { Candle } from "../types/Candle";
import { Decor } from "../types/Decor";
import { NumberOfLayer } from "../types/NumberOfLayer";
import { LayerColor } from "../types/LayerColor";
import { Smell } from "../types/Smell";
import { Wick } from "../types/Wick";
import { BaseProduct } from "../types/BaseProduct";

import Style from "./CandleDetailsPage.module.css";

type CandleDetailsParams = {
  id: string;
};

const CandleDetailsPage: FC = () => {
  const { id } = useParams<CandleDetailsParams>();
  const [candleDetailData, setCandleDetailData] = useState<CandleDetail>();

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
    updatedNumberOfLayers: NumberOfLayer[]
  ) => {
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      numberOfLayers: updatedNumberOfLayers,
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

  const updateCandleNumberOfLayers = (updatedItems: BaseProduct[]) => {
    // Update NumberOfLayers
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
    fetchCandle();
  }, [id]);

  return (
    <>
      <div className="candles">
        {candleDetailData && (
          <MainInfoCandles
            data={candleDetailData.candle}
            fetchTypeCandles={fetchTypeCandles}
            handleChangesCandle={handleChangesCandle}
          />
        )}
      </div>
      {candleDetailData?.numberOfLayers && (
        <TagsGrid
          data={candleDetailData.numberOfLayers}
          title="Количество слоев"
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
