import { useQuery, useMutation } from '@tanstack/react-query';

import { Decor } from '../types/Decor';
import { DecorRequest } from '../types/Requests/DecorRequest';

import { DecorsApi } from '../services/DecorsApi';

const useDecorByIdQuery = (id: string) => {
  const handleGetDecorById = async (decorId: string) => {
    return await DecorsApi.getById(decorId.toString());
  };

  const handleUpdateDecor = async (decor: Decor) => {
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
    queryKey: ['decor', id],
    queryFn: () => handleGetDecorById(id),
  });

  const { mutate: updateDecor } = useMutation({
    mutationKey: ['updateDecor'],
    mutationFn: handleUpdateDecor,
    onSuccess: () => {
      refetch();
    },
  });

  return {
    data,
    isLoading,
    isSuccess,
    error,
    updateDecor,
  };
};

export default useDecorByIdQuery;
