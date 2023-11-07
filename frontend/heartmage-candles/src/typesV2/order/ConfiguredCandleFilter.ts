export interface ConfiguredCandleFilter {
  candleId: number;
  decorId: number;
  layerColorIds?: number[];
  numberOfLayerId: number;
  smellId?: number;
  wickId: number;
  quantity: number;
}

export const ParseToFilter = (orderItemFilter: ConfiguredCandleFilter): string => {
  let filter = `c-${orderItemFilter.candleId}`;
  filter += `~n-${orderItemFilter.numberOfLayerId}`;
  if (orderItemFilter.layerColorIds) {
    const layerColorsString = orderItemFilter.layerColorIds.join('_');
    filter += `~l-${layerColorsString}`;
  }
  if (orderItemFilter.decorId) {
    filter += `~d-${orderItemFilter.decorId}`;
  }
  if (orderItemFilter.smellId) {
    filter += `~s-${orderItemFilter.smellId}`;
  }
  if (orderItemFilter.wickId) {
    filter += `~w-${orderItemFilter.wickId}`;
  }
  filter += `~q-${orderItemFilter.quantity}`;

  return filter;
};
