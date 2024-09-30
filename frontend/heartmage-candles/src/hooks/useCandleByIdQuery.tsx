import { useQuery, useMutation } from '@tanstack/react-query';

import { Candle } from '../types/Candle';
import { Decor } from '../types/Decor';
import { NumberOfLayer } from '../types/NumberOfLayer';

import { CandlesApi } from '../services/CandlesApi';

const useCandleByIdQuery = (id: string) => {
  const handleGetCandleById = async (candleId: string) => {
    return await CandlesApi.getById(candleId.toString());
  };

  const handleUpdateCandle = async (newCandle: Candle) => {
    const newCandleRequest = {
      title: newCandle.title,
      description: newCandle.description,
      price: newCandle.price,
      weightGrams: newCandle.weightGrams,
      images: newCandle.images,
      typeCandle: newCandle.typeCandle,
      isActive: newCandle.isActive,
    };

    return await CandlesApi.update(id.toString(), newCandleRequest);
  };

  const handleUpdateCandleNumberOfLayers = async (
    numberOfLayers: NumberOfLayer[],
  ) => {
    return await CandlesApi.updateNumberOfLayer(
      id,
      numberOfLayers.map((numberOfLayer) => numberOfLayer.id),
    );
  };

  const handleUpdateCandleDecors = async (decors: Decor[]) => {
    return await CandlesApi.updateDecor(
      id,
      decors.map((decor) => decor.id),
    );
  };

  const { data, isLoading, isSuccess, error, refetch } = useQuery({
    queryKey: ['candle', id],
    queryFn: () => handleGetCandleById(id),
  });

  const { mutate: updateCandle } = useMutation({
    mutationKey: ['updateCandle'],
    mutationFn: handleUpdateCandle,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: updateCandleNumberOfLayers } = useMutation({
    mutationKey: ['updateCandleNumberOfLayers'],
    mutationFn: handleUpdateCandleNumberOfLayers,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: updateCandleDecors } = useMutation({
    mutationKey: ['updateCandleDecors'],
    mutationFn: handleUpdateCandleDecors,
    onSuccess: () => {
      refetch();
    },
  });

  return {
    data,
    isLoading,
    isSuccess,
    error,
    updateCandle,
    updateCandleNumberOfLayers,
    updateCandleDecors,
  };
};

export default useCandleByIdQuery;
