import { Routes, Route } from "react-router-dom";

import CandleDetailsPage from "./pages/CandleDetailsPage";
import CandlePage from "./pages/AllCandlePage";
import AllDecorsPage from "./pages/AllDecorPage";
import DecorsPage from "./pages/DecorPage";
import AllLayerColorPage from "./pages/AllLayerColorPage";
import AllSmellPage from "./pages/AllSmellPage";
import AllWickPage from "./pages/AllWickPage";

import Style from "./App.module.css";

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
              <Route path="decors" element={<AllDecorsPage />} />
              <Route path="decors/:id" element={<DecorsPage />} />
              <Route path="layerColors" element={<AllLayerColorPage />} />
              <Route path="smells" element={<AllSmellPage />} />
              <Route path="wicks" element={<AllWickPage />} />
            </Route>
          </Routes>
        </div>
      </div>
    </>
  );
}

export default App;
