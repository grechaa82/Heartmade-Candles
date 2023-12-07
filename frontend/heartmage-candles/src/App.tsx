import { Routes, Route } from 'react-router-dom';

import CandleDetailsPage from './pages/admin/CandleDetailsPage';
import AllCandlePage from './pages/admin/AllCandlePage';
import AllDecorsPage from './pages/admin/AllDecorPage';
import DecorsPage from './pages/admin/DecorPage';
import AllLayerColorPage from './pages/admin/AllLayerColorPage';
import LayerColorPage from './pages/admin/LayerColorPage';
import AllSmellPage from './pages/admin/AllSmellPage';
import SmellPage from './pages/admin/SmellPage';
import AllWickPage from './pages/admin/AllWickPage';
import WickPage from './pages/admin/WickPage';
import Navbar from './components/admin/Navbar';
import ConstructorPage from './pages/constructor/ConstructorPage';
import BasketPage from './pages/order/BasketPage';
import AuthPage from './pages/auth/AuthPage';
import ThankPage from './pages/order/ThankPage';
import Header from './components/shared/Header';
import AboutUs from './pages/home/AboutUsPage';
import ContactPage from './pages/home/ContactPage';
import ReviewPage from './pages/home/ReviewPage';
import HelpPage from './pages/home/HelpPage';
import HomePage from './pages/home/HomePage';
import NotFoundPage from './pages/home/NotFoundPage';

import Style from './App.module.css';

function App() {
  return (
    <Routes>
      <Route
        path="admin/*"
        element={
          <>
            <Header />
            <div className={Style.AdminContent}>
              <Navbar />
              <main className={Style.AdminContainer}>
                <Routes>
                  <Route index element={<AllCandlePage />} path="candles" />
                  <Route element={<CandleDetailsPage />} path="candles/:id" />
                  <Route index element={<AllDecorsPage />} path="decors" />
                  <Route element={<DecorsPage />} path="decors/:id" />
                  <Route
                    index
                    element={<AllLayerColorPage />}
                    path="layerColors"
                  />
                  <Route element={<LayerColorPage />} path="layerColors/:id" />
                  <Route index element={<AllSmellPage />} path="smells" />
                  <Route element={<SmellPage />} path="smells/:id" />
                  <Route index element={<AllWickPage />} path="wicks" />
                  <Route element={<WickPage />} path="wicks/:id" />
                  <Route path="*" element={<NotFoundPage />} />
                </Routes>
              </main>
            </div>
          </>
        }
      />
      <Route
        path="*"
        element={
          <>
            <Header />
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
              <Route path="*" element={<NotFoundPage />} />
            </Routes>
          </>
        }
      />
      <Route path="*" element={<NotFoundPage />} />
    </Routes>
  );
}

export default App;
