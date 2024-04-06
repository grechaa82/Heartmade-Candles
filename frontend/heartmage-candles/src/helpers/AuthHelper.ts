import { Token } from '../typesV2/userAndAuth/Token';

export const AuthHelper = {
  getToken: (): Token | undefined => {
    const tokenDataString = localStorage.getItem('session');
    if (tokenDataString) {
      const {
        accessToken,
        refreshToken,
        expireAt,
      }: { accessToken: string; refreshToken: string; expireAt: string } =
        JSON.parse(tokenDataString);

      const token: Token = {
        AccessToken: accessToken,
        RefreshToken: refreshToken,
        ExpireAt: new Date(expireAt),
      };
      return token;
    }
    return undefined;
  },

  getAccessToken: (): string | undefined => {
    const token = AuthHelper.getToken();
    if (token) {
      return token.AccessToken;
    }
    return undefined;
  },

  getExpireAt: (): Date | undefined => {
    const token = AuthHelper.getToken();
    if (token) {
      return token.ExpireAt;
    }
    return undefined;
  },

  getAuthorizationString: (): string => {
    const accessToken = AuthHelper.getAccessToken();
    if (accessToken) {
      return `Bearer ${accessToken}`;
    }
    return '';
  },

  setToken: (newToken: Token): void => {
    localStorage.setItem('session', JSON.stringify(newToken));
  },
};
