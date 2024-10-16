import { useMutation } from '@tanstack/react-query';

import { CreateUserRequest } from '../../typesV2/userAndAuth/CreateUserRequest';

import { UserApi } from '../../services/UserApi';

const useUserQuery = () => {
  const handleCreateUser = async ({
    email,
    password,
    confirmPassword,
  }: {
    email: string;
    password: string;
    confirmPassword: string;
  }) => {
    const createUserRequest: CreateUserRequest = {
      email: email,
      password: password,
      confirmPassword: confirmPassword,
    };

    return await UserApi.create(createUserRequest);
  };

  const {
    mutate: createUser,
    isSuccess,
    isPending,
    isError,
    error,
  } = useMutation<
    void,
    Error,
    { email: string; password: string; confirmPassword: string }
  >({
    mutationKey: ['createUser'],
    mutationFn: handleCreateUser,
  });

  return {
    createUser,
    isSuccess,
    isPending,
    isError,
    error,
  };
};

export default useUserQuery;
