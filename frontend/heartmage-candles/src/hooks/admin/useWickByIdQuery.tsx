import { useQuery, useMutation } from '@tanstack/react-query';

import { Wick } from '../../types/Wick';
import { WickRequest } from '../../types/Requests/WickRequest';

import { WicksApi } from '../../services/WicksApi';

const useWickByIdQuery = (id: string, isEnabled: boolean = false) => {
  const handleGetWickById = async (wickId: string) => {
    return await WicksApi.getById(wickId.toString());
  };

  const handleUpdateWick = async (wick: Wick) => {
    const wickRequest: WickRequest = {
      title: wick.title,
      description: wick.description,
      price: wick.price,
      images: wick.images,
      isActive: wick.isActive,
    };

    return await WicksApi.update(wick.id.toString(), wickRequest);
  };

  const { data, isLoading, isSuccess, error, refetch } = useQuery({
    queryKey: ['wick', id],
    queryFn: () => handleGetWickById(id),
    enabled: isEnabled,
  });

  const { mutate: updateWick } = useMutation({
    mutationKey: ['updateWick'],
    mutationFn: handleUpdateWick,
    onSuccess: () => {
      refetch();
    },
  });

  return {
    data,
    isLoading,
    isSuccess,
    error,
    updateWick,
  };
};

export default useWickByIdQuery;
