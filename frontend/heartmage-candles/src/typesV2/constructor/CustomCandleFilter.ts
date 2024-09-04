export interface CustomCandleFilter {
  candleId: number;
  numberOfLayerId?: number;
  layerColorsIds?: number[];
  wickId?: number;
  decorId?: number;
  smellId?: number;
  quantity: number;
}

export function tryParseFilterToCustomCandleFilter(
  stringToParse: string,
): CustomCandleFilter | null {
  const MIN_LENGTH_FILTER: number = 18;

  if (stringToParse.length < MIN_LENGTH_FILTER) {
    return null;
  }

  const parts = stringToParse.split('~');

  let candleId: number | undefined;
  let numberOfLayerId: number | undefined;
  let layerColorsIds: number[] | undefined;
  let wickId: number | undefined;
  let decorId: number | undefined;
  let smellId: number | undefined;
  let quantity: number = 1;

  for (const part of parts) {
    const [key, value] = part.split('-');

    switch (key) {
      case 'c':
        candleId = parseInt(value, 10);
        break;
      case 'n':
        numberOfLayerId = value ? parseInt(value, 10) : undefined;
        break;
      case 'l':
        layerColorsIds = value.split('_').map(Number);
        break;
      case 'w':
        wickId = value ? parseInt(value, 10) : undefined;
        break;
      case 'd':
        decorId = value ? parseInt(value, 10) : undefined;
        break;
      case 's':
        smellId = value ? parseInt(value, 10) : undefined;
        break;
      case 'q':
        quantity = parseInt(value, 10);
        break;
      default:
        break;
    }
  }

  if (candleId !== undefined) {
    return {
      candleId,
      quantity,
      numberOfLayerId,
      layerColorsIds,
      wickId,
      decorId,
      smellId,
    } as CustomCandleFilter;
  }

  return null;
}
