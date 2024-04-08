import { ApiResponse } from './ApiResponse';
import { Token } from '../typesV2/userAndAuth/Token';

import { apiUrl } from '../config';
import { TokenRequest } from '../typesV2/userAndAuth/TokenRequest';
import { AuthHelper } from '../helpers/AuthHelper';

export const AuthApi = {
  login: async (
    email: string,
    password: string,
  ): Promise<ApiResponse<Token>> => {
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
        const data = (await response.json()) as Token;
        return { data: data, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },

  logout: async (): Promise<ApiResponse<void>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/auth/logout`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: authorizationString,
        },
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

  refreshToken: async (
    tokenRequest: TokenRequest,
  ): Promise<ApiResponse<Token>> => {
    try {
      const response = await fetch(`${apiUrl}/auth/refresh`, {
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(tokenRequest),
      });
      if (response.ok) {
        const data = (await response.json()) as Token;
        return { data: data, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
};
