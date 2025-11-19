import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './ui/card';
import { Button } from './ui/button';
import { FileSearch, Upload, Activity, BarChart3 } from 'lucide-react';

interface DashboardProps {
  onNavigate: (page: 'dashboard' | 'consulta' | 'upload' | 'cid') => void;
}

export default function Dashboard({ onNavigate }: DashboardProps) {
  const stats = [
    { title: 'Total de Registros', value: '12.547', description: 'Exames e diagnósticos armazenados' },
    { title: 'Pacientes Únicos', value: '3.892', description: 'Pacientes cadastrados no sistema' },
    { title: 'Uploads Hoje', value: '47', description: 'Novos arquivos enviados' },
    { title: 'CIDs Consultados', value: '156', description: 'Códigos pesquisados esta semana' }
  ];

  const quickActions = [
    {
      title: 'Consultar Dados',
      description: 'Pesquisar informações de saúde no banco de dados',
      icon: FileSearch,
      action: () => onNavigate('consulta'),
      color: 'bg-blue-500'
    },
    {
      title: 'Upload de Arquivos',
      description: 'Enviar novos exames e diagnósticos',
      icon: Upload,
      action: () => onNavigate('upload'),
      color: 'bg-green-500'
    },
    {
      title: 'Pesquisar CID',
      description: 'Consultar códigos internacionais de doenças',
      icon: BarChart3,
      action: () => onNavigate('cid'),
      color: 'bg-purple-500'
    }
  ];

  return (
    <div className="space-y-6">
      <div>
        <h2>Dashboard</h2>
        <p className="text-muted-foreground">
          Visão geral do Sistema de Dados Unificado de Saúde
        </p>
      </div>

      {/* Estatísticas */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        {stats.map((stat, index) => (
          <Card key={index}>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">{stat.title}</CardTitle>
              <Activity className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{stat.value}</div>
              <p className="text-xs text-muted-foreground">{stat.description}</p>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* Ações Rápidas */}
      <div>
        <h3 className="mb-4">Ações Rápidas</h3>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {quickActions.map((action, index) => (
            <Card key={index} className="cursor-pointer transition-all hover:shadow-md">
              <CardHeader>
                <div className="flex items-center space-x-2">
                  <div className={`p-2 rounded-lg ${action.color} text-white`}>
                    <action.icon className="h-6 w-6" />
                  </div>
                  <CardTitle className="text-lg">{action.title}</CardTitle>
                </div>
                <CardDescription>{action.description}</CardDescription>
              </CardHeader>
              <CardContent>
                <Button onClick={action.action} className="w-full">
                  Acessar
                </Button>
              </CardContent>
            </Card>
          ))}
        </div>
      </div>

      {/* Informações do Sistema */}
      <Card>
        <CardHeader>
          <CardTitle>Sobre o SDU</CardTitle>
        </CardHeader>
        <CardContent>
          <p className="text-muted-foreground">
            O Sistema de Dados Unificado (SDU) centraliza informações de saúde, permitindo 
            armazenar, consultar e analisar exames e diagnósticos de forma integrada. 
            Utilize as opções acima para navegar pelas funcionalidades do sistema.
          </p>
        </CardContent>
      </Card>
    </div>
  );
}