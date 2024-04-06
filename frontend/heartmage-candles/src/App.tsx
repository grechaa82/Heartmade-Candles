import { Routes, Route } from 'react-router-dom';

import { AuthProvider } from './context/AuthContext';
import Header from './components/shared/Header';
import PrivateRoutes from './routes/PrivateRoutes';
import PublicRoutes from './routes/PublicRoutes';

function App() {
  return (
    <AuthProvider>
      <Header />
      <Routes>
        <Route path="*" element={<PublicRoutes />} />
        <Route path="admin/*" element={<PrivateRoutes />} />
      </Routes>
    </AuthProvider>
  );
}

export default App;
