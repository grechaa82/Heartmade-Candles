import { FC } from 'react';
import { Routes, Route } from 'react-router-dom';

import Navbar from '../components/admin/Navbar';
import AllCandlePage from '../pages/admin/AllCandlePage';
import CandleDetailsPage from '../pages/admin/CandleDetailsPage';
import AllDecorPage from '../pages/admin/AllDecorPage';
import DecorPage from '../pages/admin/DecorPage';
import AllLayerColorPage from '../pages/admin/AllLayerColorPage';
import LayerColorPage from '../pages/admin/LayerColorPage';
import AllSmellPage from '../pages/admin/AllSmellPage';
import SmellPage from '../pages/admin/SmellPage';
import AllWickPage from '../pages/admin/AllWickPage';
import WickPage from '../pages/admin/WickPage';
import BotPage from '../pages/admin/BotPage';
import NotFoundPage from '../pages/home/NotFoundPage';

import Style from './AdminRoutes.module.css';

const AdminRoutes: FC = () => {
  return (
    <div className={Style.AdminContent}>
      <Navbar />
      <main className={Style.AdminContainer}>
        <Routes>
          <Route index element={<AllCandlePage />} path="candles" />
          <Route element={<CandleDetailsPage />} path="candles/:id" />
          <Route index element={<AllDecorPage />} path="decors" />
          <Route element={<DecorPage />} path="decors/:id" />
          <Route index element={<AllLayerColorPage />} path="layerColors" />
          <Route element={<LayerColorPage />} path="layerColors/:id" />
          <Route index element={<AllSmellPage />} path="smells" />
          <Route element={<SmellPage />} path="smells/:id" />
          <Route index element={<AllWickPage />} path="wicks" />
          <Route element={<WickPage />} path="wicks/:id" />
          <Route element={<BotPage />} path="bot" />
          <Route path="*" element={<NotFoundPage />} />
        </Routes>
      </main>
    </div>
  );
};

export default AdminRoutes;
