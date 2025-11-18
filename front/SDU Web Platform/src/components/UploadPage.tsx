import { useState } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './ui/card';
import { Input } from './ui/input';
import { Button } from './ui/button';
import { Label } from './ui/label';
import { Textarea } from './ui/textarea';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from './ui/select';
import { Alert, AlertDescription } from './ui/alert';
import { Upload, FileText, CheckCircle } from 'lucide-react';

interface UploadPageProps {
  onNavigate: (page: 'dashboard' | 'consulta' | 'upload' | 'cid') => void;
}

export default function UploadPage({ onNavigate }: UploadPageProps) {
  const [formData, setFormData] = useState({
    nome: '',
    cpf: '',
    tipo: '',
    descricao: '',
    resultado: '',
    arquivo: null as File | null
  });
  const [isLoading, setIsLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState('');

  const handleInputChange = (field: string, value: string) => {
    setFormData(prev => ({ ...prev, [field]: value }));
    setError('');
    setSuccess(false);
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0] || null;
    setFormData(prev => ({ ...prev, arquivo: file }));
    setError('');
    setSuccess(false);
  };

  const formatCPF = (value: string) => {
    const numbers = value.replace(/\D/g, '');
    if (numbers.length <= 11) {
      return numbers.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
    }
    return value;
  };

  const handleCPFChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const formatted = formatCPF(e.target.value);
    handleInputChange('cpf', formatted);
  };

  const validateForm = () => {
    if (!formData.nome.trim()) return 'Nome é obrigatório';
    if (!formData.cpf.trim()) return 'CPF é obrigatório';
    if (formData.cpf.replace(/\D/g, '').length !== 11) return 'CPF deve ter 11 dígitos';
    if (!formData.tipo) return 'Tipo é obrigatório';
    if (!formData.descricao.trim()) return 'Descrição é obrigatória';
    if (formData.tipo === 'diagnostico' && !formData.resultado.trim()) return 'Resultado é obrigatório para diagnósticos';
    if (!formData.arquivo) return 'Arquivo é obrigatório';
    return null;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    const validationError = validateForm();
    if (validationError) {
      setError(validationError);
      return;
    }

    setIsLoading(true);
    setError('');

    // Simular upload
    setTimeout(() => {
      setSuccess(true);
      setIsLoading(false);
      
      // Reset form after success
      setTimeout(() => {
        setFormData({
          nome: '',
          cpf: '',
          tipo: '',
          descricao: '',
          resultado: '',
          arquivo: null
        });
        setSuccess(false);
        // Reset file input
        const fileInput = document.getElementById('arquivo') as HTMLInputElement;
        if (fileInput) fileInput.value = '';
      }, 2000);
    }, 1500);
  };

  const formatFileSize = (bytes: number) => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  };

  return (
    <div className="space-y-6">
      <div>
        <h2>Upload de Arquivos</h2>
        <p className="text-muted-foreground">
          Envie novos exames e diagnósticos para o sistema
        </p>
      </div>

      <div className="max-w-2xl">
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center space-x-2">
              <Upload className="h-5 w-5" />
              <span>Novo Upload</span>
            </CardTitle>
            <CardDescription>
              Preencha os dados abaixo para enviar um novo arquivo ao sistema
            </CardDescription>
          </CardHeader>
          <CardContent>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="nome">Nome do Paciente</Label>
                  <Input
                    id="nome"
                    type="text"
                    value={formData.nome}
                    onChange={(e) => handleInputChange('nome', e.target.value)}
                    placeholder="Digite o nome completo"
                    disabled={isLoading}
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="cpf">CPF</Label>
                  <Input
                    id="cpf"
                    type="text"
                    value={formData.cpf}
                    onChange={handleCPFChange}
                    placeholder="000.000.000-00"
                    maxLength={14}
                    disabled={isLoading}
                  />
                </div>
              </div>

              <div className="space-y-2">
                <Label htmlFor="tipo">Tipo de Documento</Label>
                <Select
                  value={formData.tipo}
                  onValueChange={(value) => handleInputChange('tipo', value)}
                  disabled={isLoading}
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Selecione o tipo de documento" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="exame">Exame</SelectItem>
                    <SelectItem value="diagnostico">Diagnóstico</SelectItem>
                  </SelectContent>
                </Select>
              </div>

              <div className="space-y-2">
                <Label htmlFor="descricao">Descrição</Label>
                <Textarea
                  id="descricao"
                  value={formData.descricao}
                  onChange={(e) => handleInputChange('descricao', e.target.value)}
                  placeholder="Descreva o conteúdo do documento (ex: Hemograma completo, Raio-X tórax, etc.)"
                  disabled={isLoading}
                  rows={3}
                />
              </div>

              {formData.tipo === 'diagnostico' && (
                <div className="space-y-2">
                  <Label htmlFor="resultado">Resultado</Label>
                  <Textarea
                    id="resultado"
                    value={formData.resultado}
                    onChange={(e) => handleInputChange('resultado', e.target.value)}
                    placeholder="Digite o resultado detalhado do diagnóstico (ex: CID-10, conclusões médicas, etc.)"
                    disabled={isLoading}
                    rows={4}
                  />
                </div>
              )}

              <div className="space-y-2">
                <Label htmlFor="arquivo">Arquivo</Label>
                <Input
                  id="arquivo"
                  type="file"
                  onChange={handleFileChange}
                  accept=".pdf,.jpg,.jpeg,.png,.doc,.docx"
                  disabled={isLoading}
                />
                {formData.arquivo && (
                  <div className="flex items-center space-x-2 text-sm text-muted-foreground">
                    <FileText className="h-4 w-4" />
                    <span>{formData.arquivo.name}</span>
                    <span>({formatFileSize(formData.arquivo.size)})</span>
                  </div>
                )}
                <p className="text-sm text-muted-foreground">
                  Formatos aceitos: PDF, JPG, PNG, DOC, DOCX (máx. 10MB)
                </p>
              </div>

              {error && (
                <Alert variant="destructive">
                  <AlertDescription>{error}</AlertDescription>
                </Alert>
              )}

              {success && (
                <Alert className="border-green-200 bg-green-50">
                  <CheckCircle className="h-4 w-4 text-green-600" />
                  <AlertDescription className="text-green-800">
                    Arquivo enviado com sucesso! O documento foi adicionado ao sistema.
                  </AlertDescription>
                </Alert>
              )}

              <div className="flex space-x-2 pt-4">
                <Button 
                  type="submit" 
                  disabled={isLoading}
                  className="flex-1"
                >
                  {isLoading ? 'Enviando...' : 'Enviar Arquivo'}
                </Button>
                <Button 
                  type="button" 
                  variant="outline" 
                  onClick={() => onNavigate('dashboard')}
                  disabled={isLoading}
                >
                  Cancelar
                </Button>
              </div>
            </form>
          </CardContent>
        </Card>

        {/* Instruções */}
        <Card className="mt-6">
          <CardHeader>
            <CardTitle>Instruções de Upload</CardTitle>
          </CardHeader>
          <CardContent>
            <ul className="space-y-2 text-sm text-muted-foreground">
              <li>• Certifique-se de que o arquivo esteja legível e completo</li>
              <li>• O CPF deve ser válido e corresponder ao paciente</li>
              <li>• Selecione o tipo correto: Exame para resultados laboratoriais/imagem, Diagnóstico para laudos médicos</li>
              <li>• Arquivos são processados automaticamente e indexados no sistema</li>
              <li>• Mantenha a confidencialidade dos dados médicos</li>
            </ul>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}