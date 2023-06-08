import { Candle } from "./types/Candle";
import { CandleDetail } from "./types/CandleDetail";
import { Decor } from "./types/Decor";
import { LayerColor } from "./types/LayerColor";
import { NumberOfLayer } from "./types/NumberOfLayer";
import { Smell } from "./types/Smell";
import { TypeCandle } from "./types/TypeCandle";
import { Wick } from "./types/Wick";
import { UpdateCandleDetailsRequest } from "./types/Requests/UpdateCandleDetailsRequest";

type FetchOptions = {
    path: string;
    method?: 'GET' | 'POST' | 'PUT' | 'DELETE';
    body?: any;
};
  
const apiUrl = 'http://localhost:5000/api';

const fetchApi = async <T>({ path, method = 'GET', body }: FetchOptions): Promise<T> => {
    const response = await fetch(`${apiUrl}${path}`, {
        method,
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(body),
    });
    if (!response.ok) {
        throw new Error('Ошибка получения данных');
    }
    const data = await response.json() as T;
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

export const putCandle = (id: string, updateCandleDetailsRequest: UpdateCandleDetailsRequest): Promise<void> =>
fetchApi<void>({
    path: `/admin/candles/${id}`,
    method: 'PUT',
    body: updateCandleDetailsRequest,
});

export const getTypeCandles = (): Promise<TypeCandle[]> =>
fetchApi<TypeCandle[]>({
    path: '/admin/typeCandles/',
});

export const getNumberOfLayers = (): Promise<NumberOfLayer[]> =>
fetchApi<NumberOfLayer[]>({
    path: '/admin/numberOfLayers/',
});

export const getDecors = (): Promise<Decor[]> =>
fetchApi<Decor[]>({
    path: '/admin/decors/',
});

export const getLayerColors = (): Promise<LayerColor[]> =>
fetchApi<LayerColor[]>({
    path: '/admin/layerColors/',
});

export const getSmells = (): Promise<Smell[]> =>
fetchApi<Smell[]>({
    path: '/admin/smells/',
});

export const getWicks = (): Promise<Wick[]> =>
fetchApi<Wick[]>({
    path: '/admin/wicks/',
});
  