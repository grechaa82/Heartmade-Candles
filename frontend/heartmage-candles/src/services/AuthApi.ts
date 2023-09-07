import { apiUrl } from '../config';

export const AuthApi = {
  async login(login: string, password: string): Promise<string> {
    const response = await fetch(`${apiUrl}/auth/login`, {
      method: 'POST',
      mode: 'cors',
      credentials: 'include',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        login,
        password,
      }),
    });
    const data = await response.json();
    return data.token;
  },
};
