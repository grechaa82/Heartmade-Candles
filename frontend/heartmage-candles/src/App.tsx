import { AuthProvider } from './contexts/AuthContext';
import Header from './components/shared/Header';
import Routes from './routes/Routes';

function App() {
  return (
    <AuthProvider>
      <Header />
      <Routes />
    </AuthProvider>
  );
}

export default App;
