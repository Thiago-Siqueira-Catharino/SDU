import { useState, useEffect } from "react";
import Login from "./components/Login";
import Dashboard from "./components/Dashboard";
import ConsultaPage from "./components/ConsultaPage";
import UploadPage from "./components/UploadPage";
import CidPage from "./components/CidPage";
import { Switch } from "./components/ui/switch";
import { Moon, Sun } from "lucide-react";

export default function App() {
  const [currentPage, setCurrentPage] = useState<
    "login" | "dashboard" | "consulta" | "upload" | "cid"
  >("login");
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isDarkMode, setIsDarkMode] = useState(false);

  const handleLogin = () => {
    setIsAuthenticated(true);
    setCurrentPage("dashboard");
  };

  const handleLogout = () => {
    setIsAuthenticated(false);
    setCurrentPage("login");
  };

  const toggleDarkMode = () => {
    setIsDarkMode(!isDarkMode);
  };

  // Aplicar/remover a classe dark no documento
  useEffect(() => {
    if (isDarkMode) {
      document.documentElement.classList.add("dark");
    } else {
      document.documentElement.classList.remove("dark");
    }
  }, [isDarkMode]);

  // Verificar preferência do sistema ao carregar
  useEffect(() => {
    const darkModePreference = window.matchMedia(
      "(prefers-color-scheme: dark)",
    ).matches;
    setIsDarkMode(darkModePreference);
  }, []);

  if (!isAuthenticated) {
    return <Login onLogin={handleLogin} />;
  }

  const renderPage = () => {
    switch (currentPage) {
      case "dashboard":
        return <Dashboard onNavigate={setCurrentPage} />;
      case "consulta":
        return <ConsultaPage onNavigate={setCurrentPage} />;
      case "upload":
        return <UploadPage onNavigate={setCurrentPage} />;
      case "cid":
        return <CidPage onNavigate={setCurrentPage} />;
      default:
        return <Dashboard onNavigate={setCurrentPage} />;
    }
  };

  return (
    <div className="min-h-screen bg-background">
      <nav className="border-b bg-card px-6 py-4">
        <div className="flex items-center justify-between">
          <div className="flex items-center space-x-6">
            <h1 className="text-xl font-medium">
              SDU - Sistema de Dados Unificado
            </h1>
            <div className="flex space-x-4">
              <button
                onClick={() => setCurrentPage("dashboard")}
                className={`px-3 py-1 rounded ${currentPage === "dashboard" ? "bg-primary text-primary-foreground" : "hover:bg-accent"}`}
              >
                Dashboard
              </button>
              <button
                onClick={() => setCurrentPage("consulta")}
                className={`px-3 py-1 rounded ${currentPage === "consulta" ? "bg-primary text-primary-foreground" : "hover:bg-accent"}`}
              >
                Consulta
              </button>
              <button
                onClick={() => setCurrentPage("upload")}
                className={`px-3 py-1 rounded ${currentPage === "upload" ? "bg-primary text-primary-foreground" : "hover:bg-accent"}`}
              >
                Upload
              </button>
              <button
                onClick={() => setCurrentPage("cid")}
                className={`px-3 py-1 rounded ${currentPage === "cid" ? "bg-primary text-primary-foreground" : "hover:bg-accent"}`}
              >
                CID
              </button>
            </div>
          </div>
          <div className="flex items-center space-x-4">
            <div className="flex items-center space-x-2">
              <Sun className="h-4 w-4" />
              <Switch
                checked={isDarkMode}
                onCheckedChange={toggleDarkMode}
                aria-label="Alternar modo escuro"
              />
              <Moon className="h-4 w-4" />
            </div>
            <button
              onClick={handleLogout}
              className="px-4 py-2 bg-destructive text-destructive-foreground rounded hover:bg-destructive/90"
            >
              Sair
            </button>
          </div>
        </div>
      </nav>
      <main className="p-6">{renderPage()}</main>
    </div>
  );
}