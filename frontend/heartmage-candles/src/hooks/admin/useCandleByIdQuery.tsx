import { useQuery, useMutation } from '@tanstack/react-query';

import { Candle } from '../../types/Candle';
import { NumberOfLayer } from '../../types/NumberOfLayer';
import { Decor } from '../../types/Decor';
import { LayerColor } from '../../types/LayerColor';
import { Smell } from '../../types/Smell';
import { Wick } from '../../types/Wick';

import { CandlesApi } from '../../services/CandlesApi';

const useCandleByIdQuery = (id: string, isEnabled: boolean = false) => {
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

  const handleUpdateCandleLayerColors = async (layerColors: LayerColor[]) => {
    return await CandlesApi.updateLayerColor(
      id,
      layerColors.map((layerColor) => layerColor.id),
    );
  };

  const handleUpdateCandleSmells = async (smells: Smell[]) => {
    return await CandlesApi.updateSmell(
      id,
      smells.map((smell) => smell.id),
    );
  };

  const handleUpdateCandleWicks = async (wicks: Wick[]) => {
    return await CandlesApi.updateWick(
      id,
      wicks.map((wick) => wick.id),
    );
  };

  const { data, isLoading, isSuccess, error, refetch } = useQuery({
    queryKey: ['candle', id],
    queryFn: () => handleGetCandleById(id),
    enabled: isEnabled,
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

  const { mutate: updateCandleLayerColors } = useMutation({
    mutationKey: ['updateCandleLayerColors'],
    mutationFn: handleUpdateCandleLayerColors,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: updateCandleSmells } = useMutation({
    mutationKey: ['updateCandleSmells'],
    mutationFn: handleUpdateCandleSmells,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: updateCandleWicks } = useMutation({
    mutationKey: ['updateCandleWicks'],
    mutationFn: handleUpdateCandleWicks,
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
    updateCandleLayerColors,
    updateCandleSmells,
    updateCandleWicks,
  };
};

export default useCandleByIdQuery;
