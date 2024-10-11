import { FC } from 'react';
import { Outlet } from 'react-router-dom';

import { useRefreshTokenQuery } from '../hooks/userAndAuth/useAuthQueries';

const PrivateRoutes: FC = () => {
  useRefreshTokenQuery();

  return <Outlet />;
};

export default PrivateRoutes;
