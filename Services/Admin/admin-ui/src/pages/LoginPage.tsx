import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authApi } from '../api/client';
import { Hotel } from 'lucide-react';
import { Button, Input } from '../components/ui';
import { t } from '../lib/i18n';

export function LoginPage() {
  const [username, setUsername] = useState('admin');
  const [password, setPassword] = useState('change-me');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const res = await authApi.login(username, password);
      localStorage.setItem('token', res.accessToken);
      navigate('/');
    } catch {
      setError(t.login.invalidCredentials);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-bg">
      <div className="w-full max-w-sm">
        {/* Gold gradient line */}
        <div className="h-1 bg-gradient-to-r from-gold/0 via-gold to-gold/0 rounded-t-2xl" />

        <div className="bg-bg-card border border-border border-t-0 rounded-b-2xl p-8">
          {/* Brand */}
          <div className="text-center mb-8">
            <div className="inline-flex bg-gold/10 p-3 rounded-xl mb-4">
              <Hotel size={32} className="text-gold" />
            </div>
            <h1 className="text-2xl font-bold text-fg">{t.login.title}</h1>
            <p className="text-sm text-fg-muted mt-1">{t.login.subtitle}</p>
          </div>

          {/* Form */}
          <form onSubmit={handleSubmit} className="space-y-4">
            <Input
              label={t.login.username}
              type="text"
              value={username}
              onChange={e => setUsername(e.target.value)}
            />
            <Input
              label={t.login.password}
              type="password"
              value={password}
              onChange={e => setPassword(e.target.value)}
            />
            {error && <p className="text-sm text-red">{error}</p>}
            <Button
              type="submit"
              loading={loading}
              className="w-full"
            >
              {loading ? t.login.signingIn : t.login.signIn}
            </Button>
          </form>

          {/* Demo hint */}
          <p className="text-xs text-fg-muted text-center mt-6">
            {t.login.demo}
          </p>
        </div>
      </div>
    </div>
  );
}
