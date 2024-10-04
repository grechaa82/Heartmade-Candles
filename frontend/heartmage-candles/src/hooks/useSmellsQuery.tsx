import { useMutation, useInfiniteQuery } from '@tanstack/react-query';

import { SmellRequest } from '../types/Requests/SmellRequest';
import { Smell } from '../types/Smell';

import { SmellsApi } from '../services/SmellsApi';

const useSmellsQuery = (pageSize: number = 21) => {
  const handleGetSmells = async ({ pageIndex = 0 }) => {
    return await SmellsApi.getAll({
      pageSize: pageSize,
      pageIndex: pageIndex,
    });
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

  const handleUpdateIsActiveSmell = async (smellId: string) => {
    const smell = data?.pages
      .flatMap((page) => page)
      .find((smell) => smell.id === parseInt(smellId));

    if (!smell) {
      throw new Error(`Smell with id ${smellId} not found`);
    }

    const newSmellRequest: SmellRequest = {
      title: smell.title,
      description: smell.description,
      price: smell.price,
      isActive: !smell.isActive,
    };

    return await SmellsApi.update(smell.id.toString(), newSmellRequest);
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
    queryKey: ['smells'],
    queryFn: ({ pageParam }) => handleGetSmells({ pageIndex: pageParam }),
    initialPageParam: 0,
    getNextPageParam: (lastPage, _, lastPageParam) => {
      return lastPage.length < pageSize ? undefined : lastPageParam + 1;
    },
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
    mutationFn: handleUpdateIsActiveSmell,
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
    createSmell,
    deleteSmell,
    updateIsActiveSmell,
  };
};

export default useSmellsQuery;
