import { LayerColor } from "../types/LayerColor";
import { LayerColorRequest } from "../types/Requests/LayerColorRequest";

const apiUrl = "http://localhost:5000/api/admin/layerColors";

export const LayerColorsApi = {
  async getAll(): Promise<LayerColor[]> {
    const response = await fetch(`${apiUrl}`, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
    });
    return (await response.json()) as LayerColor[];
  },
  async getById(id: string): Promise<LayerColor> {
    const response = await fetch(`${apiUrl}/${id}`, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
    });
    return (await response.json()) as LayerColor;
  },
  async create(layerColor: LayerColorRequest): Promise<void> {
    await fetch(`${apiUrl}`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(layerColor),
    });
  },
  async update(id: string, layerColor: LayerColorRequest): Promise<void> {
    await fetch(`${apiUrl}/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(layerColor),
    });
  },
  async delete(id: string): Promise<void> {
    await fetch(`${apiUrl}/${id}`, {
      method: "DELETE",
      headers: { "Content-Type": "application/json" },
    });
  },
};
