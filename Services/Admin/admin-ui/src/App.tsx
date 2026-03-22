import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { Layout } from './components/Layout';
import { LoginPage } from './pages/LoginPage';
import { DashboardPage } from './pages/DashboardPage';
import { HotelsPage } from './pages/HotelsPage';
import { HotelDetailPage } from './pages/HotelDetailPage';
import { RoomsPage } from './pages/RoomsPage';
import { HousekeepingPage } from './pages/HousekeepingPage';
import { RatePlansPage } from './pages/RatePlansPage';
import { ActivityPage } from './pages/ActivityPage';
import { ToastProvider } from './components/ui';

const queryClient = new QueryClient({
  defaultOptions: { queries: { refetchOnWindowFocus: false, retry: 1 } },
});

function ProtectedRoute({ children }: { children: React.ReactNode }) {
  const token = localStorage.getItem('token');
  if (!token) return <Navigate to="/login" replace />;
  return <>{children}</>;
}

export default function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <ToastProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/" element={<ProtectedRoute><Layout /></ProtectedRoute>}>
              <Route index element={<DashboardPage />} />
              <Route path="hotels" element={<HotelsPage />} />
              <Route path="hotels/:id" element={<HotelDetailPage />} />
              <Route path="hotels/:hotelId/rooms" element={<RoomsPage />} />
              <Route path="hotels/:hotelId/rate-plans" element={<RatePlansPage />} />
              <Route path="housekeeping" element={<HousekeepingPage />} />
              <Route path="activity" element={<ActivityPage />} />
            </Route>
          </Routes>
        </BrowserRouter>
      </ToastProvider>
    </QueryClientProvider>
  );
}
