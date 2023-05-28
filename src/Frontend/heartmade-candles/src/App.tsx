import { Routes, Route } from 'react-router-dom';

import CandleDetailsPage from './pages/CandleDetailsPage';
import CandlePage from './pages/CandlePage';
import DecorPage from './pages/DecorPage';
import LayerColorPage from './pages/LayerColorPage';
import SmellPage from './pages/SmellPage';
import WickPage from './pages/WickPage';

import Style from './App.module.css';

function App() {
  return (
    <>
      <header></header>
      <div className={Style.AdminContent}>
        <div className={Style.AdminContainer}>
          <Routes>
            <Route path="admin/">
              <Route path="candles" element={<CandlePage />} />
              <Route path="candles/:id" element={<CandleDetailsPage />} />
              <Route path="decors" element={<DecorPage />} />
              <Route path="layerColors" element={<LayerColorPage />} />
              <Route path="smells" element={<SmellPage />} />
              <Route path="wicks" element={<WickPage />} />
            </Route>
          </Routes>
        </div>
      </div>
    </>
  );
}

export default App;
