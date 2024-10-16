import { useState } from 'react';
import { useInfiniteQuery, useMutation } from '@tanstack/react-query';

import { Candle } from '../../types/Candle';
import { CandleRequest } from '../../types/Requests/CandleRequest';

import { CandlesApi } from '../../services/CandlesApi';

const useCandlesQuery = (
  typeFilter: string,
  pageSize: number = 6,
  isEnabled: boolean = false,
) => {
  const [totalCount, setTotalCount] = useState(0);

  const handleGetCandles = async (type: string, { pageIndex = 0 }) => {
    const [candlesResponse, totalCountResponse] = await CandlesApi.getAll(
      type,
      {
        pageSize: pageSize,
        pageIndex: pageIndex,
      },
    );

    if (totalCountResponse) {
      setTotalCount(totalCountResponse);
    }

    return candlesResponse;
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

    return await CandlesApi.create(candleRequest);
  };

  const handleDeleteCandle = async (candleId: string) => {
    return await CandlesApi.delete(candleId);
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

    return await CandlesApi.update(candle.id.toString(), newCandleRequest);
  };

  const {
    data,
    fetchNextPage,
    isLoading,
    hasNextPage,
    isFetchingNextPage,
    refetch,
  } = useInfiniteQuery({
    queryKey: ['candles', typeFilter],
    queryFn: ({ pageParam }) =>
      handleGetCandles(typeFilter, { pageIndex: pageParam }),
    initialPageParam: 0,
    enabled: isEnabled,
    getNextPageParam: (lastPage, allPages, lastPageParam) => {
      const currentPageSize = lastPage.length;

      if (totalCount) {
        if (currentPageSize < pageSize || allPages.length >= totalCount) {
          return undefined;
        }
        return lastPageParam + 1;
      }

      return currentPageSize < pageSize ? undefined : lastPageParam + 1;
    },
  });

  const { mutate: createCandle } = useMutation({
    mutationKey: ['createCandle'],
    mutationFn: handleCreateCandle,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: deleteCandle } = useMutation({
    mutationKey: ['deleteCandle'],
    mutationFn: handleDeleteCandle,
    onSuccess: () => {
      refetch();
    },
  });

  const { mutate: updateIsActiveCandle } = useMutation({
    mutationKey: ['updateIsActiveCandle'],
    mutationFn: handleUpdateIsActiveCandle,
    onSuccess: () => {
      refetch();
    },
  });

  return {
    data,
    fetchNextPage,
    isLoading,
    hasNextPage,
    isFetchingNextPage,
    createCandle,
    deleteCandle,
    updateIsActiveCandle,
  };
};

export default useCandlesQuery;
