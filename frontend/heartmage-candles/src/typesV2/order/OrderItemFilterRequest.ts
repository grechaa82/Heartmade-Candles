export interface OrderItemFilterRequest {
  candleId: number;
  decorId?: number;
  numberOfLayerId: number;
  layerColorIds: number[];
  smellId?: number;
  wickId: number;
  quantity: number;
}
