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
import OrderPage from './pages/order/OrderPage';
import AuthPage from './pages/auth/AuthPage';
import Weather from './Weather';

import Style from './App.module.css';

function App() {
  return (
    <>
      <Routes>
        <Route
          path="admin/*"
          element={
            <div className={Style.AdminContent}>
              <Navbar />
              <main className={Style.AdminContainer}>
                <Routes>
                  <Route path="candles" element={<AllCandlePage />} />
                  <Route path="candles/:id" element={<CandleDetailsPage />} />
                  <Route path="decors" element={<AllDecorsPage />} />
                  <Route path="decors/:id" element={<DecorsPage />} />
                  <Route path="layerColors" element={<AllLayerColorPage />} />
                  <Route path="layerColors/:id" element={<LayerColorPage />} />
                  <Route path="smells" element={<AllSmellPage />} />
                  <Route path="smells/:id" element={<SmellPage />} />
                  <Route path="wicks" element={<AllWickPage />} />
                  <Route path="wicks/:id" element={<WickPage />} />
                </Routes>
              </main>
            </div>
          }
        />
        <Route path="constructor/" element={<ConstructorPage />} />
        <Route path="order/" element={<OrderPage />} />
        <Route path="auth/" element={<AuthPage />} />
        <Route path="weather" element={<Weather />} />
      </Routes>
    </>
  );
}

export default App;