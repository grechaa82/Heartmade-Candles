import { CreateUserRequest } from '../typesV2/userAndAuth/CreateUserRequest';

import { apiUrl } from '../config';

export const UserApi = {
  create: async (request: CreateUserRequest) => {
    const response = await fetch(`${apiUrl}/user`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(request),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return await response.json();
  },
};
