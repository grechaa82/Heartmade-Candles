import { ApiResponse } from './ApiResponse';

import { apiUrl } from '../config';

export const AuthApi = {
  login: async (
    email: string,
    password: string,
  ): Promise<ApiResponse<string>> => {
    try {
      const response = await fetch(`${apiUrl}/auth/login`, {
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          email: email,
          password,
        }),
      });
      if (response.ok) {
        const data = await response.json();
        return { data: data.token, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
};
