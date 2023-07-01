import { Routes, Route } from "react-router-dom";

import CandleDetailsPage from "./pages/CandleDetailsPage";
import CandlePage from "./pages/AllCandlePage";
import AllDecorsPage from "./pages/AllDecorPage";
import DecorsPage from "./pages/DecorPage";
import AllLayerColorPage from "./pages/AllLayerColorPage";
import LayerColorPage from "./pages/LayerColorPage";
import AllSmellPage from "./pages/AllSmellPage";
import SmellPage from "./pages/SmellPage";
import AllWickPage from "./pages/AllWickPage";
import WickPage from "./pages/WickPage";
import Navbar from "./components/Navbar";

import Style from "./App.module.css";

function App() {
  return (
    <>
      <div className={Style.AdminContent}>
        <Navbar />
        <main className={Style.AdminContainer}>
          <Routes>
            <Route path="admin/">
              <Route path="candles" element={<CandlePage />} />
              <Route path="candles/:id" element={<CandleDetailsPage />} />
              <Route path="decors" element={<AllDecorsPage />} />
              <Route path="decors/:id" element={<DecorsPage />} />
              <Route path="layerColors" element={<AllLayerColorPage />} />
              <Route path="layerColors/:id" element={<LayerColorPage />} />
              <Route path="smells" element={<AllSmellPage />} />
              <Route path="smells/:id" element={<SmellPage />} />
              <Route path="wicks" element={<AllWickPage />} />
              <Route path="wicks/:id" element={<WickPage />} />
            </Route>
          </Routes>
        </main>
      </div>
    </>
  );
}

export default App;
