import { useState } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './ui/card';
import { Input } from './ui/input';
import { Button } from './ui/button';
import { Badge } from './ui/badge';
import { Search, FileText, Calendar, User } from 'lucide-react';

interface ConsultaPageProps {
  onNavigate: (page: 'dashboard' | 'consulta' | 'upload' | 'cid') => void;
}

// Dados mockados para demonstração
const mockData = [
  {
    id: '1',
    paciente: 'João Silva',
    cpf: '123.456.789-00',
    tipo: 'Exame',
    descricao: 'Hemograma Completo',
    data: '2024-01-15',
    resultado: 'Normal - Todos os parâmetros dentro da normalidade'
  },
  {
    id: '2',
    paciente: 'Maria Santos',
    cpf: '987.654.321-00',
    tipo: 'Diagnóstico',
    descricao: 'Hipertensão Arterial',
    data: '2024-01-10',
    resultado: 'CID-10: I10 - Hipertensão essencial'
  },
  {
    id: '3',
    paciente: 'Carlos Oliveira',
    cpf: '456.789.123-00',
    tipo: 'Exame',
    descricao: 'Raio-X Tórax',
    data: '2024-01-12',
    resultado: 'Sem alterações significativas'
  },
  {
    id: '4',
    paciente: 'Ana Costa',
    cpf: '789.123.456-00',
    tipo: 'Diagnóstico',
    descricao: 'Diabetes Mellitus Tipo 2',
    data: '2024-01-08',
    resultado: 'CID-10: E11 - Diabetes mellitus não-insulino-dependente'
  },
  {
    id: '5',
    paciente: 'Pedro Souza',
    cpf: '321.654.987-00',
    tipo: 'Exame',
    descricao: 'Eletrocardiograma',
    data: '2024-01-14',
    resultado: 'Ritmo sinusal normal'
  }
];

type ExameItem = {
  id: number;
  cpf: string;
  tipo: string;
  data: string;
};


export default function ConsultaPage({ onNavigate }: ConsultaPageProps) {
  const [searchTerm, setSearchTerm] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [results, setResults] = useState<ExameItem[]>([]);

  const handleSearch = async () => {
    setIsLoading(true);

    try {
      const response = await fetch(`/api/search/exam?cpf=${encodeURI(searchTerm)}`, {
        method: 'GET',
        credentials: 'include',
        headers: {
          "Accept":"application/json",
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
      console.error(err)
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
              {isLoading ? 'Buscando...' : 'Pesquisar'}
            </Button>
          </div>
        </CardContent>
      </Card>

      {/* Resultados */}
      <div>
        <div className="flex items-center justify-between mb-4">
          <h3>Resultados da Pesquisa</h3>
          <Badge variant="secondary">{results.length} registro(s) encontrado(s)</Badge>
        </div>

        {results.length === 0 ? (
          <Card>
            <CardContent className="pt-6 text-center">
              <FileText className="h-12 w-12 text-muted-foreground mx-auto mb-4" />
              <p className="text-muted-foreground">Nenhum resultado encontrado</p>
            </CardContent>
          </Card>
        ) : (
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
                      <Badge variant="outline" className="flex items-center space-x-1">
                        <Calendar className="h-3 w-3" />
                        <span>{new Date(item.data).toLocaleDateString('pt-BR')}</span>
                      </Badge>
                    </div>
                  </div>
                </CardHeader>
              </Card>
            ))}
          </div>
        )}
      </div>

      {/* Botão de Voltar */}
      <div className="flex justify-end">
        <Button variant="outline" onClick={() => onNavigate('dashboard')}>
          Voltar ao Dashboard
        </Button>
      </div>
    </div>
  );
}