import { useQuery, useMutation } from '@tanstack/react-query';

import { TypeCandle } from '../types/TypeCandle';
import { TypeCandleRequest } from '../types/Requests/TypeCandleRequest';

import { TypeCandlesApi } from '../services/TypeCandlesApi';

const useTypeCandlesQuery = () => {
  const handleGetTypeCandles = async () => {
    return await TypeCandlesApi.getAll();
  };

  const handleCreateTypeCandle = async (newTypeCandle: TypeCandle) => {
    const typeCandleRequest: TypeCandleRequest = {
      title: newTypeCandle.title,
    };
    return await TypeCandlesApi.create(typeCandleRequest);
  };

  const handleDeleteTypeCandle = async (typeCandleId: string) => {
    return await TypeCandlesApi.delete(typeCandleId);
  };

  const { data, isLoading, isSuccess, error, refetch } = useQuery({
    queryKey: ['typeCandles'],
    queryFn: handleGetTypeCandles,
  });

  const { mutate: createTypeCandle } = useMutation({
    mutationKey: ['createTypeCandle'],
    mutationFn: handleCreateTypeCandle,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: deleteTypeCandle } = useMutation({
    mutationKey: ['deleteTypeCandle'],
    mutationFn: handleDeleteTypeCandle,
    onSuccess: () => {
      refetch();
    },
  });

  return {
    data,
    isLoading,
    isSuccess,
    error,
    createTypeCandle,
    deleteTypeCandle,
  };
};

export default useTypeCandlesQuery;
