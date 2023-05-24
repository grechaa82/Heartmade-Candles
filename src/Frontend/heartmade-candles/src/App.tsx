import React, { useState, useEffect } from 'react';
import { Routes, Route } from 'react-router-dom';
import './App.css';
import CandleDetailsPage from './pages/CandleDetailsPage';
import CandlePage from './pages/CandlePage';

function App() {
  return (
    <Routes>
      <Route path="admin/candles" element={<CandlePage />} />
      <Route path="admin/candles/:id" element={<CandleDetailsPage />} />
    </Routes>
  );
}

export default App;
