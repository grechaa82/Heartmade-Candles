import { ApiResponse } from './ApiResponse';
import { CreateUserRequest } from '../typesV2/userAndAuth/CreateUserRequest';

import { apiUrl } from '../config';

export const UserApi = {
  create: async (
    createUserRequest: CreateUserRequest,
  ): Promise<ApiResponse<void>> => {
    try {
      const response = await fetch(`${apiUrl}/user`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(createUserRequest),
      });

      if (response.ok) {
        return { data: null, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
};
