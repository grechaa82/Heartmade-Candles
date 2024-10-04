import { useMutation, useInfiniteQuery } from '@tanstack/react-query';

import { LayerColorRequest } from '../types/Requests/LayerColorRequest';
import { LayerColor } from '../types/LayerColor';

import { LayerColorsApi } from '../services/LayerColorsApi';

const useLayerColorsQuery = (pageSize: number = 21) => {
  const handleGetLayerColors = async ({ pageIndex = 0 }) => {
    return await LayerColorsApi.getAll({
      pageSize: pageSize,
      pageIndex: pageIndex,
    });
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

  const handleUpdateIsActiveLayerColor = async (layerColorId: string) => {
    if (!data) {
      throw new Error(`LayerColors data is not available.`);
    }

    const layerColor = data?.pages
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

  const {
    data,
    error,
    isLoading,
    isSuccess,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    refetch,
  } = useInfiniteQuery({
    queryKey: ['layerColors'],
    queryFn: ({ pageParam }) => handleGetLayerColors({ pageIndex: pageParam }),
    initialPageParam: 0,
    getNextPageParam: (lastPage, _, lastPageParam) => {
      return lastPage.length < pageSize ? undefined : lastPageParam + 1;
    },
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
    mutationFn: handleUpdateIsActiveLayerColor,
    onSuccess: () => {
      refetch();
    },
  });

  return {
    data,
    isLoading,
    isSuccess,
    error,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    createLayerColor,
    deleteLayerColor,
    updateIsActiveLayerColor,
  };
};

export default useLayerColorsQuery;
