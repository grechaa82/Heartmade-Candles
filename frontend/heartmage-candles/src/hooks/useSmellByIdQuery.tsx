import { useQuery, useMutation } from '@tanstack/react-query';

import { Smell } from '../types/Smell';
import { SmellRequest } from '../types/Requests/SmellRequest';

import { SmellsApi } from '../services/SmellsApi';

const useSmellByIdQuery = (id: string) => {
  const handleGetSmellById = async (smellId: string) => {
    return await SmellsApi.getById(smellId.toString());
  };

  const handleUpdateSmell = async (smell: Smell) => {
    const newSmellRequest: SmellRequest = {
      title: smell.title,
      description: smell.description,
      price: smell.price,
      isActive: !smell.isActive,
    };

    return await SmellsApi.update(smell.id.toString(), newSmellRequest);
  };

  const { data, isLoading, isSuccess, error, refetch } = useQuery({
    queryKey: ['smell', id],
    queryFn: () => handleGetSmellById(id),
  });

  const { mutate: updateSmell } = useMutation({
    mutationKey: ['updateSmell'],
    mutationFn: handleUpdateSmell,
    onSuccess: () => {
      refetch();
    },
  });

  return {
    data,
    isLoading,
    isSuccess,
    error,
    updateSmell,
  };
};

export default useSmellByIdQuery;
