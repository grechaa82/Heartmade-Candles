import { Token } from '../typesV2/userAndAuth/Token';

import { apiUrl } from '../config';
import { TokenRequest } from '../typesV2/userAndAuth/TokenRequest';
import { AuthHelper } from '../helpers/AuthHelper';

export const AuthApi = {
  login: async (email: string, password: string): Promise<Token> => {
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

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return response.json();
  },

  logout: async (): Promise<void> => {
    const authorizationString = AuthHelper.getAuthorizationString();
    const response = await fetch(`${apiUrl}/auth/logout`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: authorizationString,
      },
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },

  refreshToken: async (tokenRequest: TokenRequest): Promise<Token> => {
    const response = await fetch(`${apiUrl}/auth/refresh`, {
      method: 'POST',
      mode: 'cors',
      credentials: 'include',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(tokenRequest),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return response.json();
  },
};
