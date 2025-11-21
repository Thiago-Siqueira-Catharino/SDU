import { useState } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './ui/card';
import { Input } from './ui/input';
import { Button } from './ui/button';
import { Label } from './ui/label';
import { Alert, AlertDescription } from './ui/alert';

interface LoginProps {
  onLogin: () => void;
}

export default function Login({ onLogin }: LoginProps) {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');

    // Simulação de autenticação
    if (username.trim() === '' || password.trim() === '') {
      setError('Por favor, preencha todos os campos');
      setIsLoading(false);
      return;
    }

    // Simular delay de autenticação
    setTimeout(() => {
      if (username === 'admin' && password === 'admin') {
        onLogin();
      } else {
        setError('Usuário ou senha incorretos');
      }
      setIsLoading(false);
    }, 1000);
  };

 //     try {
 //   const response = await fetch("http://localhost:8000/api/login/", {
 //     method: "POST",
 //     headers: { "Content-Type": "application/json" },
 //     body: JSON.stringify({ "usuarios":username, "senha":password }),
 //   });
//
//    const data = await response.json();

//    if (!response.ok) {
//      setError(data.error || "Erro no login");
//    } else {
//      onLogin();
      // opcional: guardar token no localStorage
//       localStorage.setItem("token", data.token);
//    }
//  } catch (err) {
//    setError("Erro de conexão com o servidor");
//  }

//  setIsLoading(false);
//};

  return (
    <div className="min-h-screen flex items-center justify-center bg-muted/50">
      <Card className="w-full max-w-md">
        <CardHeader className="space-y-1">
          <CardTitle className="text-2xl text-center">SDU</CardTitle>
          <CardDescription className="text-center">
            Sistema de Dados Unificado
          </CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="username">Nome de usuário</Label>
              <Input
                id="username"
                type="text"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                placeholder="Digite seu usuário"
                disabled={isLoading}
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="password">Senha</Label>
              <Input
                id="password"
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Digite sua senha"
                disabled={isLoading}
              />
            </div>
            {error && (
              <Alert variant="destructive">
                <AlertDescription>{error}</AlertDescription>
              </Alert>
            )}
            <Button 
              type="submit" 
              className="w-full" 
              disabled={isLoading}
            >
              {isLoading ? 'Entrando...' : 'Entrar'}
            </Button>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}