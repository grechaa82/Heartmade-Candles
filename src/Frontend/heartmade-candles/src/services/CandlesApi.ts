import { Candle } from "../types/Candle";
import { CandleDetail } from "../types/CandleDetail";
import { CandleRequest } from "../types/Requests/CandleRequest";

const apiUrl = "http://localhost:5000/api/admin/candles";

export const CandlesApi = {
  async getAll(): Promise<Candle[]> {
    const response = await fetch(`${apiUrl}`, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
    });
    return (await response.json()) as Candle[];
  },
  async getById(id: string): Promise<CandleDetail> {
    const response = await fetch(`${apiUrl}/${id}`, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
    });
    return (await response.json()) as CandleDetail;
  },
  async create(candle: CandleRequest): Promise<void> {
    await fetch(`${apiUrl}`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(candle),
    });
  },
  async update(id: string, candle: CandleRequest): Promise<void> {
    await fetch(`${apiUrl}/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(candle),
    });
  },
  async delete(id: string): Promise<void> {
    await fetch(`${apiUrl}/${id}`, {
      method: "DELETE",
      headers: { "Content-Type": "application/json" },
    });
  },
  async updateDecor(id: string, decorIds: number[]): Promise<void> {
    await fetch(`${apiUrl}/${id}/decors`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(decorIds),
    });
  },
  async updateLayerColor(id: string, layerColorIds: number[]): Promise<void> {
    await fetch(`${apiUrl}/${id}/layerColors`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(layerColorIds),
    });
  },
  async updateNumberOfLayer(
    id: string,
    numberOfLayerIds: number[]
  ): Promise<void> {
    await fetch(`${apiUrl}/${id}/numberOfLayers`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(numberOfLayerIds),
    });
  },
  async updateSmell(id: string, smellIds: number[]): Promise<void> {
    await fetch(`${apiUrl}/${id}/smells`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(smellIds),
    });
  },
  async updateWick(id: string, wickIds: number[]): Promise<void> {
    await fetch(`${apiUrl}/${id}/wicks`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(wickIds),
    });
  },
};
