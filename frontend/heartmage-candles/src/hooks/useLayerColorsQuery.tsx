import { useMutation, useQuery } from '@tanstack/react-query';

import { LayerColorRequest } from '../types/Requests/LayerColorRequest';
import { LayerColor } from '../types/LayerColor';

import { LayerColorsApi } from '../services/LayerColorsApi';

const useLayerColorsQuery = () => {
  const handleGetLayerColors = async () => {
    return await LayerColorsApi.getAll();
  };

  const handleCreateLayerColor = async (layerColor: LayerColor) => {
    const layerColorRequest: LayerColorRequest = {
      title: layerColor.title,
      description: layerColor.description,
      pricePerGram: layerColor.pricePerGram,
      images: layerColor.images,
      isActive: layerColor.isActive,
    };

    return await LayerColorsApi.create(layerColorRequest);
  };

  const handleDeleteLayerColor = async (layerColorId: string) => {
    return await LayerColorsApi.delete(layerColorId);
  };

  const handleUpdateIsActiveCandle = async (layerColorId: string) => {
    if (!data) {
      throw new Error(`LayerColors data is not available.`);
    }

    const layerColor = data
      .flatMap((page) => page)
      .find((layerColor) => layerColor.id === parseInt(layerColorId));

    if (!layerColor) {
      throw new Error(`LayerColor with id ${layerColorId} not found`);
    }

    const newLayerColorRequest: LayerColorRequest = {
      title: layerColor.title,
      description: layerColor.description,
      pricePerGram: layerColor.pricePerGram,
      images: layerColor.images,
      isActive: !layerColor.isActive,
    };

    return await LayerColorsApi.update(
      layerColor.id.toString(),
      newLayerColorRequest,
    );
  };

  const { data, isLoading, isSuccess, error, refetch } = useQuery({
    queryKey: ['layerColors'],
    queryFn: handleGetLayerColors,
  });

  const { mutate: createLayerColor } = useMutation({
    mutationKey: ['createLayerColor'],
    mutationFn: handleCreateLayerColor,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: deleteLayerColor } = useMutation({
    mutationKey: ['deleteLayerColor'],
    mutationFn: handleDeleteLayerColor,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: updateIsActiveLayerColor } = useMutation({
    mutationKey: ['updateIsActiveLayerColor'],
    mutationFn: handleUpdateIsActiveCandle,
    onSuccess: () => {
      refetch();
    },
  });

  return {
    data,
    isLoading,
    isSuccess,
    error,
    createLayerColor,
    deleteLayerColor,
    updateIsActiveLayerColor,
  };
};

export default useLayerColorsQuery;
