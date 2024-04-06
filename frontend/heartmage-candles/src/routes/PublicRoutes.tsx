import { FC } from 'react';
import { Routes, Route } from 'react-router-dom';

import UserCreatePage from '../pages/userAndAuth/UserCreatePage';
import AuthPage from '../pages/userAndAuth/AuthPage';
import ThankPage from '../pages/order/ThankPage';
import BasketPage from '../pages/order/BasketPage';
import ConstructorPage from '../pages/constructor/ConstructorPage';
import HelpPage from '../pages/home/HelpPage';
import ReviewPage from '../pages/home/ReviewPage';
import ContactPage from '../pages/home/ContactPage';
import AboutUs from '../pages/home/AboutUsPage';
import HomePage from '../pages/home/HomePage';
import NotFoundPage from '../pages/home/NotFoundPage';

const PublicRoutes: FC = () => {
  return (
    <Routes>
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
      <Route path="*" element={<NotFoundPage />} />
    </Routes>
  );
};

export default PublicRoutes;
