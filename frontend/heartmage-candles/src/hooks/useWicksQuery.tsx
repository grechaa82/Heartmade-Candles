import { useState } from 'react';
import { useMutation, useInfiniteQuery } from '@tanstack/react-query';

import { WickRequest } from '../types/Requests/WickRequest';
import { Wick } from '../types/Wick';

import { WicksApi } from '../services/WicksApi';

const useWicksQuery = (pageSize: number = 21) => {
  const [totalCount, setTotalCount] = useState(0);

  const handleGetWicks = async ({ pageIndex = 0 }) => {
    const [wicksResponse, totalCountResponse] = await WicksApi.getAll({
      pageSize: pageSize,
      pageIndex: pageIndex,
    });

    if (totalCountResponse) {
      setTotalCount(totalCountResponse);
    }

    return wicksResponse;
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

  const handleUpdateIsActiveWick = async (wickId: string) => {
    const wick = data?.pages
      .flatMap((page) => page)
      .find((wick) => wick.id === parseInt(wickId));

    if (!wick) {
      throw new Error(`Wick with id ${wickId} not found`);
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
    queryKey: ['wicks'],
    queryFn: ({ pageParam }) => handleGetWicks({ pageIndex: pageParam }),
    initialPageParam: 0,
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
    mutationFn: handleUpdateIsActiveWick,
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
    createWick,
    deleteWick,
    updateIsActiveWick,
  };
};

export default useWicksQuery;
