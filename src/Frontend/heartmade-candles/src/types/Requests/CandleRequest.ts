import { TypeCandle } from "../TypeCandle";

export interface CandleRequest {
    title: string;
    description: string;
    price: number;
    weightGrams: number;
    imageURL: string;
    typeCandle: TypeCandle;
    isActive: boolean;
}