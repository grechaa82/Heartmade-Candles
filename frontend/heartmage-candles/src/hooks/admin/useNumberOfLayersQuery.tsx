import { useQuery, useMutation } from '@tanstack/react-query';

import { NumberOfLayer } from '../../types/NumberOfLayer';
import { NumberOfLayerRequest } from '../../types/Requests/NumberOfLayerRequest';

import { NumberOfLayersApi } from '../../services/NumberOfLayersApi';

const useNumberOfLayersQuery = (isEnabled: boolean = false) => {
  const handleGetNumberOfLayers = async () => {
    return await NumberOfLayersApi.getAll();
  };

  const handleCreateNumberOfLayer = async (newNumberOfLayer: NumberOfLayer) => {
    const numberOfLayerRequest: NumberOfLayerRequest = {
      number: newNumberOfLayer.number,
    };
    return await NumberOfLayersApi.create(numberOfLayerRequest);
  };

  const handleDeleteNumberOfLayer = async (numberOfLayerId: string) => {
    return await NumberOfLayersApi.delete(numberOfLayerId);
  };

  const { data, isLoading, isSuccess, error, refetch } = useQuery({
    queryKey: ['numberOfLayers'],
    queryFn: handleGetNumberOfLayers,
    enabled: isEnabled,
  });

  const { mutate: createNumberOfLayer } = useMutation({
    mutationKey: ['createNumberOfLayer'],
    mutationFn: handleCreateNumberOfLayer,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: deleteNumberOfLayer } = useMutation({
    mutationKey: ['deleteNumberOfLayer'],
    mutationFn: handleDeleteNumberOfLayer,
    onSuccess: () => {
      refetch();
    },
  });

  return {
    data,
    isLoading,
    isSuccess,
    error,
    createNumberOfLayer,
    deleteNumberOfLayer,
  };
};

export default useNumberOfLayersQuery;
