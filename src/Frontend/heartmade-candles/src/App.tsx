import React, { useState, useEffect } from 'react';
import { Routes, Route } from 'react-router-dom';
import './App.css';
import CandleDetailsPage from './pages/CandleDetailsPage';
import CandlePage from './pages/CandlePage';
import DecorPage from './pages/DecorPage';
import LayerColorPage from './pages/LayerColorPage';
import SmellPage from './pages/SmellPage';
import WickPage from './pages/WickPage';

function App() {
  return (
    <Routes>
      <Route path="admin/candles" element={<CandlePage />} />
      <Route path="admin/candles/:id" element={<CandleDetailsPage />} />
      <Route path="admin/decors" element={<DecorPage />} />
      <Route path="admin/layerColors" element={<LayerColorPage />} />
      <Route path="admin/smells" element={<SmellPage />} />
      <Route path="admin/wicks" element={<WickPage />} />
    </Routes>
  );
}

export default App;
