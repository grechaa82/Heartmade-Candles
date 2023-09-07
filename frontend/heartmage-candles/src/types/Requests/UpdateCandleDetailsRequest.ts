import { CandleRequest } from "./CandleRequest";

export interface UpdateCandleDetailsRequest {
    candleRequest: CandleRequest;
    decorsIds: number[]; 
    layerColorsIds: number[];
    numberOfLayersIds: number[];
    smellsIds: number[];
    wicksIds: number[];
}