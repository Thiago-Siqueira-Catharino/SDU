import { useState } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './ui/card';
import { Input } from './ui/input';
import { Button } from './ui/button';
import { Badge } from './ui/badge';
import { Search, BarChart3, TrendingUp, Users } from 'lucide-react';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts';

interface CidPageProps {
  onNavigate: (page: 'dashboard' | 'consulta' | 'upload' | 'cid') => void;
}

// Dados mockados de CIDs
const cidDatabase = {
  'I10': {
    codigo: 'I10',
    descricao: 'Hipertensão essencial (primária)',
    categoria: 'Doenças do aparelho circulatório',
    casos: 1245,
    prevalencia: '15.2%'
  },
  'E11': {
    codigo: 'E11',
    descricao: 'Diabetes mellitus não-insulino-dependente',
    categoria: 'Doenças endócrinas, nutricionais e metabólicas',
    casos: 987,
    prevalencia: '12.1%'
  },
  'J44': {
    codigo: 'J44',
    descricao: 'Outras doenças pulmonares obstrutivas crônicas',
    categoria: 'Doenças do aparelho respiratório',
    casos: 543,
    prevalencia: '6.7%'
  },
  'M79': {
    codigo: 'M79',
    descricao: 'Outros transtornos dos tecidos moles',
    categoria: 'Doenças do sistema osteomuscular',
    casos: 432,
    prevalencia: '5.3%'
  },
  'Z51': {
    codigo: 'Z51',
    descricao: 'Outros cuidados médicos',
    categoria: 'Fatores que influenciam o estado de saúde',
    casos: 321,
    prevalencia: '3.9%'
  }
};

// Dados para gráficos
const chartData = [
  { mes: 'Jan', casos: 145 },
  { mes: 'Fev', casos: 167 },
  { mes: 'Mar', casos: 198 },
  { mes: 'Abr', casos: 234 },
  { mes: 'Mai', casos: 189 },
  { mes: 'Jun', casos: 243 }
];

const pieData = [
  { name: 'Circulatório', value: 35, color: '#8884d8' },
  { name: 'Endócrino', value: 25, color: '#82ca9d' },
  { name: 'Respiratório', value: 20, color: '#ffc658' },
  { name: 'Osteomuscular', value: 12, color: '#ff7c7c' },
  { name: 'Outros', value: 8, color: '#8dd1e1' }
];

export default function CidPage({ onNavigate }: CidPageProps) {
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCid, setSelectedCid] = useState<any>(null);
  const [isLoading, setIsLoading] = useState(false);

  const handleSearch = async () => {
    if (!searchTerm.trim()) return;
    
    setIsLoading(true);
    
    // Simular busca no banco de dados
    setTimeout(() => {
      const cid = cidDatabase[searchTerm.toUpperCase() as keyof typeof cidDatabase];
      setSelectedCid(cid || null);
      setIsLoading(false);
    }, 800);
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      handleSearch();
    }
  };

  return (
    <div className="space-y-6">
      <div>
        <h2>Consulta CID</h2>
        <p className="text-muted-foreground">
          Pesquise e analise Códigos Internacionais de Doenças
        </p>
      </div>

      {/* Busca de CID */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center space-x-2">
            <Search className="h-5 w-5" />
            <span>Pesquisar CID</span>
          </CardTitle>
          <CardDescription>
            Digite o código CID para obter informações detalhadas
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="flex space-x-2">
            <Input
              placeholder="Digite o código CID (ex: I10, E11, J44...)"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value.toUpperCase())}
              onKeyPress={handleKeyPress}
              className="flex-1"
            />
            <Button onClick={handleSearch} disabled={isLoading}>
              {isLoading ? 'Buscando...' : 'Pesquisar'}
            </Button>
          </div>
          
          {selectedCid && (
            <div className="mt-4 p-4 border rounded-lg bg-muted/50">
              <div className="space-y-2">
                <div className="flex items-center space-x-2">
                  <Badge variant="default">{selectedCid.codigo}</Badge>
                  <Badge variant="outline">{selectedCid.categoria}</Badge>
                </div>
                <h4 className="font-medium">{selectedCid.descricao}</h4>
                <div className="grid grid-cols-2 gap-4 text-sm">
                  <div>
                    <span className="font-medium">Casos registrados: </span>
                    <span>{selectedCid.casos.toLocaleString()}</span>
                  </div>
                  <div>
                    <span className="font-medium">Prevalência: </span>
                    <span>{selectedCid.prevalencia}</span>
                  </div>
                </div>
              </div>
            </div>
          )}
          
          {searchTerm && !selectedCid && !isLoading && (
            <div className="mt-4 p-4 border rounded-lg text-center text-muted-foreground">
              CID não encontrado. Tente códigos como: I10, E11, J44, M79, Z51
            </div>
          )}
        </CardContent>
      </Card>

      {/* Gráficos */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Gráfico de Barras */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center space-x-2">
              <BarChart3 className="h-5 w-5" />
              <span>Casos por Mês</span>
            </CardTitle>
            <CardDescription>
              Evolução mensal dos casos registrados
            </CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={chartData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="mes" />
                <YAxis />
                <Tooltip />
                <Bar dataKey="casos" fill="#8884d8" />
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        {/* Gráfico de Pizza */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center space-x-2">
              <TrendingUp className="h-5 w-5" />
              <span>Distribuição por Categoria</span>
            </CardTitle>
            <CardDescription>
              Percentual de casos por categoria de doença
            </CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <PieChart>
                <Pie
                  data={pieData}
                  cx="50%"
                  cy="50%"
                  labelLine={false}
                  label={({ name, value }) => `${name}: ${value}%`}
                  outerRadius={80}
                  fill="#8884d8"
                  dataKey="value"
                >
                  {pieData.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={entry.color} />
                  ))}
                </Pie>
                <Tooltip />
              </PieChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      </div>

      {/* Top CIDs */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center space-x-2">
            <Users className="h-5 w-5" />
            <span>CIDs Mais Prevalentes</span>
          </CardTitle>
          <CardDescription>
            Códigos mais frequentes no sistema
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            {Object.values(cidDatabase).map((cid, index) => (
              <div key={cid.codigo} className="flex items-center justify-between p-3 border rounded-lg hover:bg-muted/50 transition-colors">
                <div className="flex items-center space-x-3">
                  <Badge variant="outline">{index + 1}º</Badge>
                  <div>
                    <div className="flex items-center space-x-2">
                      <Badge>{cid.codigo}</Badge>
                      <span className="font-medium">{cid.descricao}</span>
                    </div>
                    <p className="text-sm text-muted-foreground">{cid.categoria}</p>
                  </div>
                </div>
                <div className="text-right">
                  <div className="font-medium">{cid.casos.toLocaleString()} casos</div>
                  <div className="text-sm text-muted-foreground">{cid.prevalencia}</div>
                </div>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>

      {/* Botão de Voltar */}
      <div className="flex justify-end">
        <Button variant="outline" onClick={() => onNavigate('dashboard')}>
          Voltar ao Dashboard
        </Button>
      </div>
    </div>
  );
}