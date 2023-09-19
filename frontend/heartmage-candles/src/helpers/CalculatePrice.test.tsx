import { CandleDetail } from '../typesV2/constructor/CandleDetail';
import { calculatePrice } from './CalculatePrice';

let candleDetail: CandleDetail = {
  candle: {
    id: 1,
    title: 'Scented Candle',
    description: 'A beautiful scented candle',
    price: 10,
    weightGrams: 200,
    typeCandle: {
      id: 1,
      title: 'Type 1',
    },
    createdAt: '2021-01-01',
    images: [
      {
        fileName: 'candle.jpg',
        alternativeName: 'Scented Candle',
      },
    ],
  },
  decors: [
    {
      id: 1,
      title: 'Decor 1',
      description: 'A beautiful decor',
      price: 5,
      images: [
        {
          fileName: 'decor.jpg',
          alternativeName: 'Decor 1',
        },
      ],
    },
  ],
  layerColors: [
    {
      id: 1,
      title: 'Layer Color 1',
      description: 'A beautiful layer color',
      price: 1.2,
      images: [
        {
          fileName: 'layer-color1.jpg',
          alternativeName: 'Layer Color 1',
        },
      ],
    },
    {
      id: 2,
      title: 'Layer Color 2',
      description: 'Another beautiful layer color',
      price: 0.3,
      images: [
        {
          fileName: 'layer-color2.jpg',
          alternativeName: 'Layer Color 2',
        },
      ],
    },
  ],
  numberOfLayers: [
    {
      id: 1,
      number: 2,
    },
  ],
  smells: [
    {
      id: 1,
      title: 'Smell 1',
      description: 'A beautiful smell',
      price: 1,
    },
  ],
  wicks: [
    {
      id: 1,
      title: 'Wick 1',
      description: 'A beautiful wick',
      price: 0.5,
      images: [
        {
          fileName: 'wick.jpg',
          alternativeName: 'Wick 1',
        },
      ],
    },
  ],
};

describe('calculatePrice', () => {
  test('should calculate the correct price when all details are provided', () => {
    const totalPrice = calculatePrice(candleDetail);

    expect(totalPrice).toBe(167);
  });

  test('should calculate the correct price when only candle is provided', () => {
    const newCandleDetail = {
      ...candleDetail,
      decors: [],
      layerColors: [],
      numberOfLayers: [],
      smells: [],
      wicks: [],
    };

    const totalPrice = calculatePrice(newCandleDetail);

    expect(totalPrice).toBe(10);
  });

  test('should calculate the correct price when decors is not provided', () => {
    const newCandleDetail = {
      ...candleDetail,
      decors: [],
    };

    const totalPrice = calculatePrice(newCandleDetail);

    expect(totalPrice).toBe(162);
  });

  test('should calculate the correct price when layerColors is not provided', () => {
    const newCandleDetail = {
      ...candleDetail,
      layerColors: [],
    };

    const totalPrice = calculatePrice(newCandleDetail);

    expect(totalPrice).toBe(17);
  });

  test('should calculate the correct price when one of two layerColors does not exist', () => {
    const newCandleDetail = {
      ...candleDetail,
      layerColors: [
        {
          id: 1,
          title: 'Layer Color 1',
          description: 'A beautiful layer color',
          price: 1.2,
          images: [
            {
              fileName: 'layer-color1.jpg',
              alternativeName: 'Layer Color 1',
            },
          ],
        },
      ],
    };

    const totalPrice = calculatePrice(newCandleDetail);

    expect(totalPrice).toBe(137);
  });

  test('should calculate the correct price when numberOfLayers is not provided', () => {
    const newCandleDetail = {
      ...candleDetail,
      numberOfLayers: [],
    };

    const totalPrice = calculatePrice(newCandleDetail);

    expect(totalPrice).toBe(17);
  });

  test('should calculate the correct price when smells is not provided', () => {
    const newCandleDetail = {
      ...candleDetail,
      smells: [],
    };

    const totalPrice = calculatePrice(newCandleDetail);

    expect(totalPrice).toBe(166);
  });

  test('should calculate the correct price when wicks is not provided', () => {
    const newCandleDetail = {
      ...candleDetail,
      wicks: [],
    };

    const totalPrice = calculatePrice(newCandleDetail);

    expect(totalPrice).toBe(166);
  });
});
