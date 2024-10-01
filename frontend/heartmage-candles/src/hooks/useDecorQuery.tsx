import { useMutation, useQuery } from '@tanstack/react-query';

import { DecorRequest } from '../types/Requests/DecorRequest';
import { Decor } from '../types/Decor';

import { DecorsApi } from '../services/DecorsApi';

const useDecorQuery = () => {
  const handleGetDecors = async () => {
    return await DecorsApi.getAll();
  };

  const handleCreateDecor = async (decor: Decor) => {
    const decorRequest: DecorRequest = {
      title: decor.title,
      description: decor.description,
      price: decor.price,
      images: decor.images,
      isActive: decor.isActive,
    };

    return await DecorsApi.create(decorRequest);
  };

  const handleDeleteDecor = async (decorId: string) => {
    return await DecorsApi.delete(decorId);
  };

  const handleUpdateIsActiveCandle = async (decorId: string) => {
    const decor = data
      .flatMap((page) => page)
      .find((decor) => decor.id === parseInt(decorId));

    if (!decor) {
      throw new Error(`Candle with id ${decorId} not found`);
    }

    const newDecorRequest: DecorRequest = {
      title: decor.title,
      description: decor.description,
      price: decor.price,
      images: decor.images,
      isActive: !decor.isActive,
    };

    return await DecorsApi.update(decor.id.toString(), newDecorRequest);
  };

  const { data, isLoading, isSuccess, error, refetch } = useQuery({
    queryKey: ['decors'],
    queryFn: handleGetDecors,
  });

  const { mutate: createDecor } = useMutation({
    mutationKey: ['createDecor'],
    mutationFn: handleCreateDecor,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: deleteDecor } = useMutation({
    mutationKey: ['deleteDecor'],
    mutationFn: handleDeleteDecor,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: updateIsActiveDecor } = useMutation({
    mutationKey: ['updateIsActiveDecor'],
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
    createDecor,
    deleteDecor,
    updateIsActiveDecor,
  };
};

export default useDecorQuery;
