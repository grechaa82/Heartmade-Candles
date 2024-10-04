import { useState } from 'react';
import { useMutation, useInfiniteQuery } from '@tanstack/react-query';

import { DecorRequest } from '../types/Requests/DecorRequest';
import { Decor } from '../types/Decor';

import { DecorsApi } from '../services/DecorsApi';

const useDecorsQuery = (pageSize: number = 21) => {
  const [totalCount, setTotalCount] = useState(0);

  const handleGetDecors = async ({ pageIndex = 0 }) => {
    const [decorsResponse, totalCountResponse] = await DecorsApi.getAll({
      pageSize: pageSize,
      pageIndex: pageIndex,
    });

    if (totalCountResponse) {
      setTotalCount(totalCountResponse);
    }

    return decorsResponse;
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

  const handleUpdateIsActiveDecor = async (decorId: string) => {
    const decor = data?.pages
      .flatMap((page) => page)
      .find((decor) => decor.id === parseInt(decorId));

    if (!decor) {
      throw new Error(`Decor with id ${decorId} not found`);
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
    queryKey: ['decors'],
    queryFn: ({ pageParam }) => handleGetDecors({ pageIndex: pageParam }),
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
    mutationFn: handleUpdateIsActiveDecor,
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
    createDecor,
    deleteDecor,
    updateIsActiveDecor,
  };
};

export default useDecorsQuery;
