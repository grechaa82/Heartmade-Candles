import { CandleDetailFilterRequest } from "./CandleDetailFilterRequest";

export interface CandleDetailFilterBasketRequest {
  candleDetailFilterRequests: CandleDetailFilterRequest[];
  configuredCandleFiltersString: string;
}
