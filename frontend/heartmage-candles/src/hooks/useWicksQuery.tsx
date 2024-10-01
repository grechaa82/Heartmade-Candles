import { useMutation, useQuery } from '@tanstack/react-query';

import { WickRequest } from '../types/Requests/WickRequest';
import { Wick } from '../types/Wick';

import { WicksApi } from '../services/WicksApi';

const useWicksQuery = () => {
  const handleGetWicks = async () => {
    return await WicksApi.getAll();
  };

  const handleCreateWick = async (wick: Wick) => {
    const wickRequest: WickRequest = {
      title: wick.title,
      description: wick.description,
      price: wick.price,
      images: wick.images,
      isActive: wick.isActive,
    };

    return await WicksApi.create(wickRequest);
  };

  const handleDeleteWick = async (wickId: string) => {
    return await WicksApi.delete(wickId);
  };

  const handleUpdateIsActiveCandle = async (wickId: string) => {
    const wick = data
      .flatMap((page) => page)
      .find((wick) => wick.id === parseInt(wickId));

    if (!wick) {
      throw new Error(`Candle with id ${wickId} not found`);
    }

    const newWickRequest: WickRequest = {
      title: wick.title,
      description: wick.description,
      price: wick.price,
      images: wick.images,
      isActive: !wick.isActive,
    };

    return await WicksApi.update(wick.id.toString(), newWickRequest);
  };

  const { data, isLoading, isSuccess, error, refetch } = useQuery({
    queryKey: ['wicks'],
    queryFn: handleGetWicks,
  });

  const { mutate: createWick } = useMutation({
    mutationKey: ['createWick'],
    mutationFn: handleCreateWick,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: deleteWick } = useMutation({
    mutationKey: ['deleteWick'],
    mutationFn: handleDeleteWick,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: updateIsActiveWick } = useMutation({
    mutationKey: ['updateIsActiveWick'],
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
    createWick,
    deleteWick,
    updateIsActiveWick,
  };
};

export default useWicksQuery;
