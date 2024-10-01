import { useMutation, useQuery } from '@tanstack/react-query';

import { SmellRequest } from '../types/Requests/SmellRequest';
import { Smell } from '../types/Smell';

import { SmellsApi } from '../services/SmellsApi';

const useSmellsQuery = () => {
  const handleGetSmells = async () => {
    return await SmellsApi.getAll();
  };

  const handleCreateSmell = async (smell: Smell) => {
    const smellRequest: SmellRequest = {
      title: smell.title,
      description: smell.description,
      price: smell.price,
      isActive: smell.isActive,
    };

    return await SmellsApi.create(smellRequest);
  };

  const handleDeleteSmell = async (smellId: string) => {
    return await SmellsApi.delete(smellId);
  };

  const handleUpdateIsActiveCandle = async (smellId: string) => {
    const smell = data
      .flatMap((page) => page)
      .find((smell) => smell.id === parseInt(smellId));

    if (!smell) {
      throw new Error(`Candle with id ${smellId} not found`);
    }

    const newSmellRequest: SmellRequest = {
      title: smell.title,
      description: smell.description,
      price: smell.price,
      isActive: !smell.isActive,
    };

    return await SmellsApi.update(smell.id.toString(), newSmellRequest);
  };

  const { data, isLoading, isSuccess, error, refetch } = useQuery({
    queryKey: ['smells'],
    queryFn: handleGetSmells,
  });

  const { mutate: createSmell } = useMutation({
    mutationKey: ['createSmell'],
    mutationFn: handleCreateSmell,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: deleteSmell } = useMutation({
    mutationKey: ['deleteSmell'],
    mutationFn: handleDeleteSmell,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: updateIsActiveSmell } = useMutation({
    mutationKey: ['updateIsActiveSmell'],
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
    createSmell,
    deleteSmell,
    updateIsActiveSmell,
  };
};

export default useSmellsQuery;
