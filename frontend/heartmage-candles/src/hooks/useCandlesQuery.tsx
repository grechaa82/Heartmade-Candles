import { useInfiniteQuery, useMutation } from '@tanstack/react-query';

import { Candle } from '../types/Candle';
import { CandleRequest } from '../types/Requests/CandleRequest';

import { CandlesApi } from '../services/CandlesApi';

const useCandlesQuery = (typeFilter: string, pageSize: number = 6) => {
  const handleGetCandles = async (type: string, { pageIndex = 0 }) => {
    const response = await CandlesApi.getAll(type, {
      pageSize: pageSize,
      pageIndex: pageIndex,
    });

    return response;
  };

  const handleCreateCandle = async (newCandle: Candle) => {
    const candleRequest: CandleRequest = {
      title: newCandle.title,
      description: newCandle.description,
      price: newCandle.price,
      weightGrams: newCandle.weightGrams,
      images: newCandle.images,
      typeCandle: newCandle.typeCandle,
      isActive: newCandle.isActive,
    };

    const response = await CandlesApi.create(candleRequest);

    return response;
  };

  const handleDeleteCandle = async (candleId: string) => {
    const response = await CandlesApi.delete(candleId);

    return response;
  };

  const handleUpdateIsActiveCandle = async (candleId: string) => {
    const candle = data?.pages
      .flatMap((page) => page)
      .find((candle) => candle.id === parseInt(candleId));

    if (!candle) {
      throw new Error(`Candle with id ${candleId} not found`);
    }

    const newCandleRequest = {
      title: candle.title,
      description: candle.description,
      price: candle.price,
      weightGrams: candle.weightGrams,
      images: candle.images,
      typeCandle: candle.typeCandle,
      isActive: !candle.isActive,
    };

    const response = await CandlesApi.update(
      candle.id.toString(),
      newCandleRequest,
    );

    return response;
  };

  const { data, fetchNextPage, hasNextPage, isFetchingNextPage, refetch } =
    useInfiniteQuery({
      queryKey: ['candles', typeFilter],
      queryFn: ({ pageParam }) =>
        handleGetCandles(typeFilter, { pageIndex: pageParam }),
      initialPageParam: 0,
      getNextPageParam: (lastPage, _, lastPageParam) => {
        return lastPage.length < pageSize ? undefined : lastPageParam + 1;
      },
    });

  const { mutate: createCandle } = useMutation({
    mutationFn: handleCreateCandle,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: deleteCandle } = useMutation({
    mutationFn: handleDeleteCandle,
    mutationKey: ['deleteCandle'],
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: updateIsActiveCandle } = useMutation({
    mutationFn: handleUpdateIsActiveCandle,
    mutationKey: ['updateIsActiveCandle'],
    onSuccess: () => {
      refetch();
    },
  });

  return {
    data,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    createCandle,
    deleteCandle,
    updateIsActiveCandle,
  };
};

export default useCandlesQuery;
