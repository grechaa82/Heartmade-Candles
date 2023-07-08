import { Smell } from "../types/Smell";
import { SmellRequest } from "../types/Requests/SmellRequest";

const apiUrl = "http://localhost:5000/api/admin/smells";

export const SmellsApi = {
  async getAll(): Promise<Smell[]> {
    const response = await fetch(`${apiUrl}`, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
    });
    return (await response.json()) as Smell[];
  },
  async getById(id: string): Promise<Smell> {
    const response = await fetch(`${apiUrl}/${id}`, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
    });
    return (await response.json()) as Smell;
  },
  async create(smell: SmellRequest): Promise<void> {
    await fetch(`${apiUrl}`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(smell),
    });
  },
  async update(id: string, smell: SmellRequest): Promise<void> {
    await fetch(`${apiUrl}/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(smell),
    });
  },
  async delete(id: string): Promise<void> {
    await fetch(`${apiUrl}/${id}`, {
      method: "DELETE",
      headers: { "Content-Type": "application/json" },
    });
  },
};