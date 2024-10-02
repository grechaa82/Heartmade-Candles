import { useQuery, useMutation } from '@tanstack/react-query';

import { LayerColor } from '../types/LayerColor';
import { LayerColorRequest } from '../types/Requests/LayerColorRequest';

import { LayerColorsApi } from '../services/LayerColorsApi';

const useLayerColorByIdQuery = (id: string) => {
  const handleGetLayerColorById = async (layerColorId: string) => {
    return await LayerColorsApi.getById(layerColorId.toString());
  };

  const handleUpdateLayerColor = async (layerColor: LayerColor) => {
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
    queryKey: ['layerColor', id],
    queryFn: () => handleGetLayerColorById(id),
  });

  const { mutate: updateLayerColor } = useMutation({
    mutationKey: ['updateLayerColor'],
    mutationFn: handleUpdateLayerColor,
    onSuccess: () => {
      refetch();
    },
  });

  return {
    data,
    isLoading,
    isSuccess,
    error,
    updateLayerColor,
  };
};

export default useLayerColorByIdQuery;
