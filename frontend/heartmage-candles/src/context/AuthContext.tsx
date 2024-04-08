import { FC, createContext, ReactNode, useState } from 'react';

export interface AuthProviderProps {
  children?: ReactNode;
}

type IAuthContext = {
  isAuth: boolean;
  setIsAuth: (newState: boolean) => void;
};

const initialValue = {
  isAuth: false,
  setIsAuth: () => {},
};

const AuthContext = createContext<IAuthContext>(initialValue);

const AuthProvider: FC<AuthProviderProps> = ({ children }) => {
  const [isAuth, setIsAuth] = useState(initialValue.isAuth);

  return (
    <AuthContext.Provider value={{ isAuth, setIsAuth }}>
      {children}
    </AuthContext.Provider>
  );
};

export { AuthContext, AuthProvider };
