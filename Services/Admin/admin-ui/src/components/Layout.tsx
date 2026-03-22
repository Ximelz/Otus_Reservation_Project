import { Outlet, NavLink, useNavigate } from 'react-router-dom';
import { LayoutDashboard, Hotel, Paintbrush, Activity, LogOut, Menu, X } from 'lucide-react';
import { useState } from 'react';
import { t } from '../lib/i18n';

const navItems = [
  { to: '/', icon: LayoutDashboard, label: t.nav.dashboard, end: true },
  { to: '/hotels', icon: Hotel, label: t.nav.hotels },
  { to: '/housekeeping', icon: Paintbrush, label: t.nav.housekeeping },
  { to: '/activity', icon: Activity, label: t.nav.activity },
];

export function Layout() {
  const navigate = useNavigate();
  const [sidebarOpen, setSidebarOpen] = useState(true);

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/login');
  };

  return (
    <div className="flex h-screen overflow-hidden">
      {/* Sidebar */}
      <aside
        className={`${sidebarOpen ? 'w-64' : 'w-16'} transition-all duration-300 flex flex-col bg-bg-sidebar border-r border-border`}
      >
        {/* Brand / Header */}
        <div className="p-4 flex items-center gap-3 border-b border-border">
          <button
            onClick={() => setSidebarOpen(!sidebarOpen)}
            className="p-1 rounded hover:bg-white/10 text-fg-muted hover:text-fg transition-colors"
          >
            {sidebarOpen ? <X size={18} /> : <Menu size={18} />}
          </button>
          {sidebarOpen && (
            <div className="flex items-center gap-3 flex-1">
              <div className="flex flex-col">
                <div className="flex items-center gap-2">
                  <Hotel size={18} className="text-gold" />
                  <h1 className="text-sm font-bold text-gold">{t.nav.brand}</h1>
                </div>
                <p className="text-xs text-fg-muted">{t.nav.subtitle}</p>
              </div>
              {/* System status dot */}
              <div className="ml-auto group relative">
                <div className="w-2 h-2 rounded-full bg-emerald animate-pulse" />
                <span className="absolute left-1/2 -translate-x-1/2 top-full mt-1 text-xs text-fg-muted bg-bg-card border border-border rounded px-2 py-1 whitespace-nowrap opacity-0 group-hover:opacity-100 transition-opacity pointer-events-none z-50">
                  {t.dashboard.systemOnline}
                </span>
              </div>
            </div>
          )}
        </div>

        {/* Navigation */}
        <nav className="flex-1 py-4">
          {navItems.map(item => (
            <NavLink
              key={item.to}
              to={item.to}
              end={item.end}
              className={({ isActive }) =>
                `flex items-center gap-3 px-4 py-2.5 mx-2 rounded-lg text-sm transition-colors ${
                  isActive
                    ? 'bg-gold/10 text-gold border-l-2 border-gold font-medium'
                    : 'text-fg-muted hover:text-fg hover:bg-white/5'
                }`
              }
            >
              <item.icon size={18} />
              {sidebarOpen && item.label}
            </NavLink>
          ))}
        </nav>

        {/* Bottom: User + Logout */}
        <div className="p-4 border-t border-border space-y-3">
          {sidebarOpen && (
            <div className="flex items-center gap-3">
              <div className="w-8 h-8 rounded-full bg-gold/10 text-gold flex items-center justify-center text-sm font-bold">
                A
              </div>
              <div>
                <p className="text-sm font-medium text-fg">Администратор</p>
                <p className="text-xs text-fg-muted">admin</p>
              </div>
            </div>
          )}
          {!sidebarOpen && (
            <div className="flex justify-center">
              <div className="w-8 h-8 rounded-full bg-gold/10 text-gold flex items-center justify-center text-sm font-bold">
                A
              </div>
            </div>
          )}
          <button
            onClick={handleLogout}
            className="flex items-center gap-3 px-4 py-2 rounded-lg text-sm w-full text-red hover:bg-white/5 transition-colors"
          >
            <LogOut size={18} />
            {sidebarOpen && t.nav.logout}
          </button>
        </div>
      </aside>

      {/* Main Content */}
      <main className="flex-1 overflow-y-auto bg-bg">
        <div className="max-w-[1400px] mx-auto p-8">
          <Outlet />
        </div>
      </main>
    </div>
  );
}
