import { FC, useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import MainInfoCandle, { FetchTypeCandle } from "../modules/MainInfoCandle";
import { CandleDetail } from "../types/CandleDetail";
import ProductsGrid, { FetchProducts } from "../modules/ProductsGrid";
import TagsGrid from "../modules/TagsGrid";
import { Candle } from "../types/Candle";
import { Decor } from "../types/Decor";
import { NumberOfLayer } from "../types/NumberOfLayer";
import { LayerColor } from "../types/LayerColor";
import { Smell } from "../types/Smell";
import { Wick } from "../types/Wick";
import { BaseProduct } from "../types/BaseProduct";
import { TagData } from "../components/Tag";
import AddProductPopUp from "../components/PopUp/AddProductPopUp";

import { CandlesApi } from "../services/CandlesApi";
import { DecorsApi } from "../services/DecorsApi";
import { LayerColorsApi } from "../services/LayerColorsApi";
import { NumberOfLayersApi } from "../services/NumberOfLayersApi";
import { SmellsApi } from "../services/SmellsApi";
import { TypeCandlesApi } from "../services/TypeCandlesApi";
import { WicksApi } from "../services/WicksApi";

import Style from "./CandleDetailsPage.module.css";

type CandleDetailsParams = {
  id: string;
};

const CandleDetailsPage: FC = () => {
  const { id } = useParams<CandleDetailsParams>();
  const [candleDetailData, setCandleDetailData] = useState<CandleDetail>();
  const [numberOfLayerTagData, setNumberOfLayerTagData] = useState<TagData[]>();

  const fetchTypeCandles: FetchTypeCandle = async () => {
    try {
      const data = await TypeCandlesApi.getAll();
      return data;
    } catch (error) {
      console.error("Произошла ошибка при загрузке типов свечей:", error);
      return [];
    }
  };

  const fetchDecors: FetchProducts<Decor> = async () => {
    try {
      const data = await DecorsApi.getAll();
      return data;
    } catch (error) {
      console.error("Произошла ошибка при загрузке типов свечей:", error);
      return [];
    }
  };

  const fetchLayerColors: FetchProducts<LayerColor> = async () => {
    try {
      const data = await LayerColorsApi.getAll();
      return data;
    } catch (error) {
      console.error("Произошла ошибка при загрузке типов свечей:", error);
      return [];
    }
  };

  const fetchNumberOfLayer = async (): Promise<NumberOfLayer[]> => {
    try {
      const data = await NumberOfLayersApi.getAll();
      return data;
    } catch (error) {
      console.error("Произошла ошибка при загрузке типов свечей:", error);
      return [];
    }
  };

  const fetchSmells: FetchProducts<Smell> = async () => {
    try {
      const data = await SmellsApi.getAll();
      return data;
    } catch (error) {
      console.error("Произошла ошибка при загрузке типов свечей:", error);
      return [];
    }
  };

  const fetchWicks: FetchProducts<Wick> = async () => {
    try {
      const data = await WicksApi.getAll();
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

  const updateCandle = async (updatedItem: Candle) => {
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
      await CandlesApi.update(id, candleRequest);
    }
  };

  const updateCandleDecors = async (updatedItems: BaseProduct[]) => {
    if (id) {
      const updatedDecors = updatedItems as Decor[];
      const ids = updatedDecors.map((d) => d.id);
      await CandlesApi.updateDecor(id, ids);
    }
  };

  const updateCandleLayerColors = async (updatedItems: BaseProduct[]) => {
    if (id) {
      const updatedLayerColors = updatedItems as LayerColor[];
      const ids = updatedLayerColors.map((l) => l.id);
      await CandlesApi.updateLayerColor(id, ids);
    }
  };

  const updateCandleNumberOfLayers = async (updatedItems: TagData[]) => {
    if (id) {
      const ids = updatedItems.map((n) => n.id);
      await CandlesApi.updateNumberOfLayer(id, ids);
    }
  };

  const updateCandleSmells = async (updatedItems: BaseProduct[]) => {
    if (id) {
      const updatedSmells = updatedItems as Smell[];
      const ids = updatedSmells.map((s) => s.id);
      await CandlesApi.updateSmell(id, ids);
    }
  };

  const updateCandleWicks = async (updatedItems: BaseProduct[]) => {
    if (id) {
      const updatedWicks = updatedItems as Wick[];
      const ids = updatedWicks.map((w) => w.id);
      await CandlesApi.updateWick(id, ids);
    }
  };

  useEffect(() => {
    async function fetchCandle() {
      if (id) {
        const data = await CandlesApi.getById(id);
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
          <MainInfoCandle
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
          title="Декоры"
          data={candleDetailData.decors}
          popUpComponent={
            <AddProductPopUp
              onClose={() => console.log("Popup closed")}
              title="Свеча и декоры"
              selectedData={candleDetailData.decors}
              setSelectedData={handleChangesDecors}
              fetchAllData={fetchDecors}
              onSave={updateCandleDecors}
            />
          }
        />
      )}
      {candleDetailData?.layerColors && (
        <ProductsGrid
          title="Слои"
          data={candleDetailData.layerColors}
          popUpComponent={
            <AddProductPopUp
              onClose={() => console.log("Popup closed")}
              title="Свеча и слои"
              selectedData={candleDetailData.layerColors}
              setSelectedData={handleChangesLayerColors}
              fetchAllData={fetchLayerColors}
              onSave={updateCandleLayerColors}
            />
          }
        />
      )}
      {candleDetailData?.smells && (
        <ProductsGrid
          title="Запахи"
          data={candleDetailData.smells}
          popUpComponent={
            <AddProductPopUp
              onClose={() => console.log("Popup closed")}
              title="Свеча и запахи"
              selectedData={candleDetailData.smells}
              setSelectedData={handleChangesSmells}
              fetchAllData={fetchSmells}
              onSave={updateCandleSmells}
            />
          }
        />
      )}
      {candleDetailData?.wicks && (
        <ProductsGrid
          title="Фитили"
          data={candleDetailData.wicks}
          popUpComponent={
            <AddProductPopUp
              onClose={() => console.log("Popup closed")}
              title="Свеча и фитили"
              selectedData={candleDetailData.wicks}
              setSelectedData={handleChangesWicks}
              fetchAllData={fetchWicks}
              onSave={updateCandleWicks}
            />
          }
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
