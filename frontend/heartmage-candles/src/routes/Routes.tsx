import { FC, useContext } from 'react';
import { Routes as Router, Route } from 'react-router-dom';

import PrivateRoutes from './PrivateRoutes';
import { AuthContext } from '../context/AuthContext';
import AuthPage from '../pages/userAndAuth/AuthPage';
import AdminRoutes from './AdminRoutes';
import HelpPage from '../pages/home/HelpPage';
import ReviewPage from '../pages/home/ReviewPage';
import ContactPage from '../pages/home/ContactPage';
import AboutUs from '../pages/home/AboutUsPage';
import HomePage from '../pages/home/HomePage';
import NotFoundPage from '../pages/home/NotFoundPage';
import ConstructorPage from '../pages/constructor/ConstructorPage';
import BasketPage from '../pages/order/BasketPage';
import ThankPage from '../pages/order/ThankPage';
import AuthSuccessPage from '../pages/userAndAuth/AuthSuccessPage';
import UserCreatePage from '../pages/userAndAuth/UserCreatePage';

const Routes: FC = () => {
  const { isAuth } = useContext(AuthContext);

  return (
    <Router>
      <Route index element={<HomePage />} />
      <Route element={<AboutUs />} path="aboutUs" />
      <Route element={<ContactPage />} path="contact" />
      <Route element={<ReviewPage />} path="review" />
      <Route element={<HelpPage />} path="help" />
      <Route element={<ConstructorPage />} path="constructor" />
      <Route element={<BasketPage />} path="baskets/:id" />
      <Route element={<ThankPage />} path="orders/:id/thank" />
      <Route element={<AuthPage />} path="auth" />
      <Route element={<UserCreatePage />} path="user/create" />
      <Route element={<PrivateRoutes />}>
        <Route element={<AdminRoutes />} path="admin/*" />
        <Route element={<AuthSuccessPage />} path="auth/success" />
      </Route>
      <Route path="*" element={<NotFoundPage />} />
    </Router>
  );
};

export default Routes;
