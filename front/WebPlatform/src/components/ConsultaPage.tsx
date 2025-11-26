import { useState } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './ui/card';
import { Input } from './ui/input';
import { Button } from './ui/button';
import { Badge } from './ui/badge';
import { Search, FileText, Calendar, User } from 'lucide-react';

interface ConsultaPageProps {
  onNavigate: (page: 'dashboard' | 'consulta' | 'upload' | 'cid') => void;
}

type ExameItem = {
  id: string;
  cpf: string;
  tipo: string;
  data: string;
};

export default function ConsultaPage({ onNavigate }: ConsultaPageProps) {
  const [searchTerm, setSearchTerm] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [results, setResults] = useState<ExameItem[]>([]);
  const [activeTab, setActiveTab] = useState<"exames" | "diagnosticos">("exames");

  const handleSearch = async () => {
    setIsLoading(true);

    try {
      const response = await fetch(`/api/search/exam?cpf=${encodeURI(searchTerm)}`, {
        method: 'GET',
        credentials: 'include',
        headers: {
          "Accept": "application/json",
        },
      });

      if (response.status === 400) {
        setResults([]);
        return;
      }

      if (!response.ok) {
        console.error("Erro na resposta da API", response.status);
        setResults([]);
        return;
      }

      const data = await response.json();
      setResults(Array.isArray(data.exames) ? data.exames : []);

    } catch (err) {
      console.error(err);
    }
    setIsLoading(false);
  };

  const handleDownload = async (id: string) => {
    setIsLoading(true);

    try {
      const response = await fetch(`/api/download/exam?id=${encodeURI(id)}`, {
        method: 'GET',
        credentials: 'include',
        headers: {
          "Accept":"application/json",
        },
      });

      if (!response.ok) {
        console.error("Erro na resposta da API", response.status);
        return;
      }

      const data = await response.json();

      if (!data.url) {
        console.error("A api não retornou uma url");
        return;
      }

      window.location.href = data.url;

    } catch (err) {
      console.error(err);
    }
    setIsLoading(false);
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      handleSearch();
    }
  };

  return (
    <div className="space-y-6">
      <div>
        <h2>Consulta de Dados</h2>
        <p className="text-muted-foreground">
          Pesquise informações de saúde no banco de dados unificado
        </p>
      </div>

      {/* Barra de Pesquisa */}
      <Card>
        <CardContent className="pt-6">
          <div className="flex space-x-2">
            <div className="relative flex-1">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground h-4 w-4" />
              <Input
                placeholder="Pesquisar por CPF (sem pontos ou traços)"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                onKeyPress={handleKeyPress}
                className="pl-10"
              />
            </div>
            <Button onClick={handleSearch} disabled={isLoading}>
              {isLoading ? "Buscando..." : "Pesquisar"}
            </Button>
          </div>
        </CardContent>
      </Card>

      {/* Só mostra abas SE houver resultados */}
      {results.length > 0 && (
        <div className="flex items-center -mb-1">
          <button
            onClick={() => setActiveTab("exames")}
            className={`px-4 py-2 font-medium border
              ${activeTab === "exames" ? "bg-blue-500 text-black" : "bg-gray-200 text-black"}
            `}
            style={{
              borderTopLeftRadius: 20,
              borderBottomLeftRadius: 20,
            }}
          >
            Exames
          </button>

          <button
            onClick={() => setActiveTab("diagnosticos")}
            className={`px-4 py-2 font-medium border rounded-r-lg
              ${activeTab === "diagnosticos" ? "bg-blue-500 text-black" : "bg-gray-200 text-black"}
            `}
            style={{
              borderTopRightRadius: 20,
              borderBottomRightRadius: 20,
            }}
          >
            Diagnóstico
          </button>
        </div>
      )}

      {/* Retângulo dos resultados */}
      <div
        className={`
          w-full border border-black rounded-xl p-6
          ${results.length > 0 ? "bg-[#7B0015] text-white" : ""}
        `}
      >
        {/* Título sempre aparece */}
        <div className="flex items-center justify-between mb-4">
          <h3 className={results.length > 0 ? "text-muted-foreground" : ""}>Resultados da Pesquisa</h3>
          <Badge variant="secondary">
            {results.length} registro(s) encontrado(s)
          </Badge>
        </div>

        {/* Se não houver nada */}
        {results.length === 0 && (
          <Card>
            <CardContent className="pt-6 text-center">
              <FileText className="h-12 w-12 text-muted-foreground mx-auto mb-4" />
              <p className="text-muted-foreground">Nenhum resultado encontrado</p>
            </CardContent>
          </Card>
        )}

        {/* Conteúdo da aba EXAMES */}
        {results.length > 0 && activeTab === "exames" && (
          <div className="grid gap-4">
            {(results || []).map((item) => (
              <Card key={item.id} className="hover:shadow-md transition-shadow">
                <CardHeader>
                  <div className="flex items-start justify-between">
                    <div>
                      <CardTitle className="flex items-center space-x-2">
                        <User className="h-4 w-4" />
                        <span>{item.tipo}</span>
                      </CardTitle>
                      <CardDescription>CPF: {item.cpf}</CardDescription>
                    </div>
                    <div className="flex space-x-2">
                      <Button onClick={() => handleDownload(item.id)}>
                        Download
                      </Button>
                      <Badge variant="outline" className="flex items-center space-x-1">
                        <Calendar className="h-3 w-3" />
                        <span>{new Date(item.data).toLocaleDateString("pt-BR")}</span>
                      </Badge>
                    </div>
                  </div>
                </CardHeader>
              </Card>
            ))}
          </div>
        )}

        {/* Conteúdo da aba DIAGNÓSTICO */}
        {results.length > 0 && activeTab === "diagnosticos" && (
          <div>
            {/* Conteúdo */} 
          </div>
        )}
      </div>

      {/* Botão de Voltar */}
      <div className="flex justify-end">
        <Button variant="outline" onClick={() => onNavigate("dashboard")}>
          Voltar ao Dashboard
        </Button>
      </div>
    </div>
  );
}
