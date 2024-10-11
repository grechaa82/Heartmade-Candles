import { useContext, useEffect } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';

import { Token } from '../../typesV2/userAndAuth/Token';
import { TokenRequest } from '../../typesV2/userAndAuth/TokenRequest';
import { AuthContext } from '../../contexts/AuthContext';
import { AuthHelper } from '../../helpers/AuthHelper';

import { AuthApi } from '../../services/AuthApi';

export const useLoginQuery = () => {
  const queryClient = useQueryClient();
  const { setIsAuth } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleLogin = async ({
    email,
    password,
  }: {
    email: string;
    password: string;
  }) => {
    queryClient.setQueryData(['token'], null);
    setIsAuth(false);
    AuthHelper.removeToken();
    return await AuthApi.login(email, password);
  };

  const {
    mutate: login,
    isSuccess,
    error,
  } = useMutation<Token, Error, { email: string; password: string }>({
    mutationKey: ['login'],
    mutationFn: handleLogin,
    onSuccess: (token) => {
      if (token) {
        queryClient.setQueryData(['token'], token);
        AuthHelper.setToken(token);
        setIsAuth(true);
        navigate('/auth/success');
      } else {
        setIsAuth(false);
        AuthHelper.removeToken();
      }
    },
    onError: (err) => {
      console.error(err);
    },
  });

  return { login, isSuccess, error };
};

export const useLogoutQuery = () => {
  const queryClient = useQueryClient();

  const handleLogout = async () => {
    return await AuthApi.logout();
  };

  const {
    mutate: logout,
    isSuccess,
    error,
  } = useMutation({
    mutationKey: ['logout'],
    mutationFn: handleLogout,
    onSuccess: async () => {
      queryClient.setQueryData(['token'], null);
      AuthHelper.removeToken();
    },
    onError: (err) => {
      console.error(err);
    },
  });

  return { logout, isSuccess, error };
};

export const useRefreshTokenQuery = () => {
  const { setIsAuth } = useContext(AuthContext);
  const queryClient = useQueryClient();

  const handleRefreshToken = async ({ accessToken, refreshToken }) => {
    const tokenRequest: TokenRequest = {
      accessToken: accessToken,
      refreshToken: refreshToken,
    };

    return await AuthApi.refreshToken(tokenRequest);
  };

  const {
    mutate: refreshToken,
    isSuccess,
    error,
  } = useMutation<Token, Error, { accessToken: string; refreshToken: string }>({
    mutationKey: ['refreshToken'],
    mutationFn: handleRefreshToken,
    onSuccess: (data) => {
      queryClient.setQueryData(['token'], data);
      AuthHelper.setToken(data);
    },
    onError: () => {
      setIsAuth(false);
      AuthHelper.removeToken();
      window.location.href = '/auth';
    },
  });

  const updateToken = () => {
    let token =
      queryClient.getQueryData<Token>(['token']) || AuthHelper.getToken();
    if (!token) return;

    const currentTime = new Date().getTime();
    const expiryTime = new Date(token.ExpireAt).getTime();
    const timeToExpire = expiryTime - currentTime;

    if (timeToExpire < 15 * 60 * 1000) {
      refreshToken({
        accessToken: token.AccessToken,
        refreshToken: token.RefreshToken,
      });
    }
  };

  useEffect(() => {
    const interval = setInterval(updateToken, 10 * 60 * 1000);
    return () => clearInterval(interval);
  }, []);

  return { refreshToken, isSuccess, error };
};
