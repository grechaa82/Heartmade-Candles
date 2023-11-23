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

import Style from './App.module.css';

function App() {
  return (
    <>
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
                    <Route path="candles" element={<AllCandlePage />} />
                    <Route path="candles/:id" element={<CandleDetailsPage />} />
                    <Route path="decors" element={<AllDecorsPage />} />
                    <Route path="decors/:id" element={<DecorsPage />} />
                    <Route path="layerColors" element={<AllLayerColorPage />} />
                    <Route
                      path="layerColors/:id"
                      element={<LayerColorPage />}
                    />
                    <Route path="smells" element={<AllSmellPage />} />
                    <Route path="smells/:id" element={<SmellPage />} />
                    <Route path="wicks" element={<AllWickPage />} />
                    <Route path="wicks/:id" element={<WickPage />} />
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
                <Route path="" element={<HomePage />} />
                <Route path="aboutUs/" element={<AboutUs />} />
                <Route path="contact/" element={<ContactPage />} />
                <Route path="review/" element={<ReviewPage />} />
                <Route path="help/" element={<HelpPage />} />
                <Route path="constructor/" element={<ConstructorPage />} />
                <Route path="baskets/:id" element={<BasketPage />} />
                <Route path="orders/:id">
                  <Route path="thank" element={<ThankPage />} />
                </Route>
                <Route path="auth/" element={<AuthPage />} />
              </Routes>
            </>
          }
        />
      </Routes>
    </>
  );
}

export default App;
