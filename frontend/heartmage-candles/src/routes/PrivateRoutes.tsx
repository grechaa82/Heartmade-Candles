import { FC, useContext, useEffect } from 'react';
import { Outlet, useNavigate } from 'react-router-dom';

import { AuthContext } from '../contexts/AuthContext';
import { AuthHelper } from '../helpers/AuthHelper';
import { AuthApi } from '../services/AuthApi';

const PrivateRoutes: FC = () => {
  const { setIsAuth } = useContext(AuthContext);

  const navigate = useNavigate();

  useEffect(() => {
    const intervalInMinutes = 2.83;
    const intervalInMilliseconds = intervalInMinutes * 60 * 1000;

    const isTokenExpiringSoon = (expireAt: Date) => {
      const timeLeft = new Date(expireAt).getTime() - Date.now();
      return timeLeft < 5 * 60 * 1000;
    };

    const refreshAuthToken = async () => {
      const token = AuthHelper.getToken();

      if (token && isTokenExpiringSoon(token?.ExpireAt)) {
        try {
          const tokenRequest = {
            accessToken: token.AccessToken,
            refreshToken: token.RefreshToken,
          };

          const newToken = await AuthApi.refreshToken(tokenRequest);

          if (newToken.data) {
            AuthHelper.setToken(newToken.data);
            console.log('Token refreshed successfully.');
          } else {
            throw new Error('Failed to refresh token.');
          }
        } catch (error) {
          console.error('Error refreshing token:', error);
          navigate('/auth');
        }
      }
    };

    refreshAuthToken();

    const interval = setInterval(() => {
      refreshAuthToken();
    }, intervalInMilliseconds);

    return () => clearInterval(interval);
  }, []);

  useEffect(() => {
    const token = AuthHelper.getToken();
    if (token) {
      setIsAuth(true);
    } else {
      navigate('/auth');
    }
  }, []);

  return <Outlet />;
};

export default PrivateRoutes;
