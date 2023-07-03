import { Candle } from "./types/Candle";
import { CandleDetail } from "./types/CandleDetail";
import { Decor } from "./types/Decor";
import { LayerColor } from "./types/LayerColor";
import { NumberOfLayer } from "./types/NumberOfLayer";
import { Smell } from "./types/Smell";
import { TypeCandle } from "./types/TypeCandle";
import { Wick } from "./types/Wick";
import { CandleRequest } from "./types/Requests/CandleRequest";
import { DecorRequest } from "./types/Requests/DecorRequest";
import { LayerColorRequest } from "./types/Requests/LayerColorRequest";
import { SmellRequest } from "./types/Requests/SmellRequest";
import { WickRequest } from "./types/Requests/WickRequest";

type FetchOptions = {
  path: string;
  method?: "GET" | "POST" | "PUT" | "DELETE";
  body?: any;
};

const apiUrl = "http://localhost:5000/api";

const fetchApi = async <T>({
  path,
  method = "GET",
  body,
}: FetchOptions): Promise<T> => {
  const response = await fetch(`${apiUrl}${path}`, {
    method,
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });
  if (!response.ok) {
    throw new Error("Ошибка получения данных");
  }
  const data = (await response.json()) as T;
  return data;
};

export const getCandle = (): Promise<Candle[]> =>
  fetchApi<Candle[]>({
    path: `/admin/candles/`,
  });

export const getCandleById = (id: string): Promise<CandleDetail> =>
  fetchApi<CandleDetail>({
    path: `/admin/candles/${id}`,
  });

export const getTypeCandles = (): Promise<TypeCandle[]> =>
  fetchApi<TypeCandle[]>({
    path: "/admin/typeCandles/",
  });

export const getNumberOfLayers = (): Promise<NumberOfLayer[]> =>
  fetchApi<NumberOfLayer[]>({
    path: "/admin/numberOfLayers/",
  });

export const getDecors = (): Promise<Decor[]> =>
  fetchApi<Decor[]>({
    path: "/admin/decors/",
  });

export const getDecorById = (id: string): Promise<Decor> =>
  fetchApi<Decor>({
    path: `/admin/decors/${id}`,
  });

export const getLayerColors = (): Promise<LayerColor[]> =>
  fetchApi<LayerColor[]>({
    path: "/admin/layerColors/",
  });

export const getLayerColorById = (id: string): Promise<LayerColor> =>
  fetchApi<LayerColor>({
    path: `/admin/layerColors/${id}`,
  });

export const getSmells = (): Promise<Smell[]> =>
  fetchApi<Smell[]>({
    path: "/admin/smells/",
  });

export const getSmellById = (id: string): Promise<Smell> =>
  fetchApi<Smell>({
    path: `/admin/smells/${id}`,
  });

export const getWicks = (): Promise<Wick[]> =>
  fetchApi<Wick[]>({
    path: "/admin/wicks/",
  });

export const getWickById = (id: string): Promise<Wick> =>
  fetchApi<Wick>({
    path: `/admin/wicks/${id}`,
  });

export const putCandle = async (
  id: string,
  candle: CandleRequest
): Promise<void> => {
  const url = `/api/admin/candles/${id}`;
  const response = await fetch(url, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(candle),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to update candle: ${errorText}`);
  }
};

export const putCandleDecors = async (
  id: string,
  decorIds: number[]
): Promise<void> => {
  const url = `/api/admin/candles/${id}/decors`;
  const response = await fetch(url, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(decorIds),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to update candleDecor: ${errorText}`);
  }
};

export const putCandleLayerColors = async (
  id: string,
  decorIds: number[]
): Promise<void> => {
  const url = `/api/admin/candles/${id}/layerColors`;
  const response = await fetch(url, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(decorIds),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to update candleDecor: ${errorText}`);
  }
};

export const putCandleNumberOfLayer = async (
  id: string,
  numberOfLayerIds: number[]
): Promise<void> => {
  const url = `/api/admin/candles/${id}/numberOfLayers`;
  const response = await fetch(url, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(numberOfLayerIds),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to update candleDecor: ${errorText}`);
  }
};

export const putCandleSmells = async (
  id: string,
  decorIds: number[]
): Promise<void> => {
  const url = `/api/admin/candles/${id}/smells`;
  const response = await fetch(url, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(decorIds),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to update candleDecor: ${errorText}`);
  }
};

export const putCandleWicks = async (
  id: string,
  decorIds: number[]
): Promise<void> => {
  const url = `/api/admin/candles/${id}/wicks`;
  const response = await fetch(url, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(decorIds),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to update candleDecor: ${errorText}`);
  }
};

export const putDecor = async (
  id: string,
  decor: DecorRequest
): Promise<void> => {
  const url = `/api/admin/decors/${id}`;
  const response = await fetch(url, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(decor),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to update decor: ${errorText}`);
  }
};

export const putLayerColor = async (
  id: string,
  layerColor: LayerColorRequest
): Promise<void> => {
  const url = `/api/admin/layerColors/${id}`;
  const response = await fetch(url, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(layerColor),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to update decor: ${errorText}`);
  }
};

export const putSmell = async (
  id: string,
  smell: SmellRequest
): Promise<void> => {
  const url = `/api/admin/smells/${id}`;
  const response = await fetch(url, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(smell),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to update decor: ${errorText}`);
  }
};

export const putWick = async (id: string, wick: WickRequest): Promise<void> => {
  const url = `/api/admin/wicks/${id}`;
  const response = await fetch(url, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(wick),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to update decor: ${errorText}`);
  }
};

export const createCandle = async (candle: CandleRequest): Promise<void> => {
  const url = "/api/admin/candles";
  const response = await fetch(url, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(candle),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to create candle: ${errorText}`);
  }
};

export const createDecor = async (decor: DecorRequest): Promise<void> => {
  const url = "/api/admin/decors";
  const response = await fetch(url, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(decor),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to create candle: ${errorText}`);
  }
};

export const createLayerColor = async (
  layerColor: LayerColorRequest
): Promise<void> => {
  const url = "/api/admin/layerColors";
  const response = await fetch(url, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(layerColor),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to create candle: ${errorText}`);
  }
};

export const createSmell = async (smell: SmellRequest): Promise<void> => {
  const url = "/api/admin/smells";
  const response = await fetch(url, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(smell),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to create candle: ${errorText}`);
  }
};

export const createWick = async (wick: WickRequest): Promise<void> => {
  const url = "/api/admin/wicks";
  const response = await fetch(url, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(wick),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to create candle: ${errorText}`);
  }
};

export const deleteCandle = async (id: string): Promise<void> => {
  const url = `/api/admin/candles/${id}`;
  const response = await fetch(url, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to delete candle: ${errorText}`);
  }
};

export const deleteDecor = async (id: string): Promise<void> => {
  const url = "/api/admin/decors";
  const response = await fetch(url, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to delete decor: ${errorText}`);
  }
};

export const deleteLayerColor = async (id: string): Promise<void> => {
  const url = "/api/admin/layerColors";
  const response = await fetch(url, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to delete layerColor: ${errorText}`);
  }
};

export const deleteSmell = async (id: string): Promise<void> => {
  const url = "/api/admin/smells";
  const response = await fetch(url, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to delete smell: ${errorText}`);
  }
};

export const deleteWick = async (id: string): Promise<void> => {
  const url = "/api/admin/wicks";
  const response = await fetch(url, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to delete wick: ${errorText}`);
  }
};
