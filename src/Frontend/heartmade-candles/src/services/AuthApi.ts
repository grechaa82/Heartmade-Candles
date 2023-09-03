const apiUrl = 'http://localhost:80/api/auth';

export const AuthApi = {
  async login(login: string, password: string): Promise<string> {
    const response = await fetch(`${apiUrl}/login`, {
      method: 'POST',
      mode: 'cors',
      credentials: 'include',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        login: login,
        password: password,
      }),
    });
    const data = await response.json();
    return data.token;
  },
};
