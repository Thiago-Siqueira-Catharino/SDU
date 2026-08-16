# SDU — Sistema de Dados Unificado

Projeto acadêmico para **centralizar exames e diagnósticos médicos**, permitindo upload, consulta e download seguro de arquivos clínicos vinculados ao paciente (CPF) e à classificação da doença (CID). Inclui um contador de incidência por CID, útil para análises epidemiológicas simples.

## Como funciona

- **Exames** e **Diagnósticos** são registrados no banco com metadados (CPF, tipo/CID, data), enquanto o arquivo em si (PDF, PNG ou JPEG) fica armazenado no **Amazon S3**.
- O download não expõe o arquivo diretamente: gera uma **URL pré-assinada do S3** com expiração curta (30s), evitando links permanentes ou acesso não autorizado ao bucket.
- Todo upload passa por validação de tipo real do arquivo via `python-magic` (leitura dos bytes/magic number), não apenas a extensão — bloqueia arquivos disfarçados.
- Um `Tracker` mantém a contagem de diagnósticos por CID, incrementado atomicamente a cada novo registro.

## Stack

| Camada | Tecnologia |
|---|---|
| Backend | Python 3.13, Django |
| Armazenamento de arquivos | Amazon S3 (via boto3) |
| Validação de arquivo | python-magic (detecção de MIME real) |
| Frontend | React + TypeScript |

## Estrutura

```
SDU/
├── api/            # App Django: models, views e regras de negócio
│   ├── models.py    # Exame, Diagnostico, Tracker
│   ├── views.py      # Endpoints de upload/download/busca
│   └── utils.py       # Integração com S3 e validações
├── front/           # App Django que serve o frontend React
│   └── WebPlatform/  # Build do React
└── SDU/             # Configuração do projeto Django
```

## Endpoints da API (`/api/`)

| Método | Rota | Descrição |
|---|---|---|
| GET | `/` | Health check |
| POST | `/upload/exam` | Envia um exame (arquivo + CPF + tipo) |
| GET | `/search/exam?cpf=` | Lista exames de um paciente |
| GET | `/download/exam?id=` | Gera link de download do exame |
| POST | `/upload/diagnosis` | Registra diagnóstico (CPF + CID + exames vinculados + arquivo) |
| GET | `/search/diagnosis?cpf=` | Lista diagnósticos de um paciente |
| GET | `/download/diagnosis?id=` | Gera link de download do diagnóstico |

## Como rodar localmente

### Pré-requisitos
- Python 3.13 + [Pipenv](https://pipenv.pypa.io/)
- Node.js (para build do frontend)
- Um bucket S3 (ou serviço compatível) e credenciais AWS

### Passos

1. Instale as dependências:
   ```bash
   pipenv install
   ```

2. Configure as variáveis de ambiente (via `django-environ`, em um arquivo `.env`):
   ```
   AWS_ACCESS_KEY_ID=...
   AWS_SECRET_ACCESS_KEY=...
   REGION_NAME=...
   BUCKET_NAME=...
   ```

3. Rode as migrations:
   ```bash
   pipenv run python manage.py migrate
   ```

4. Instale e builde o frontend (dentro de `front/WebPlatform`):
   ```bash
   npm install
   npm run build
   ```

5. Suba o servidor:
   ```bash
   pipenv run python manage.py runserver
   ```

## Notas

> ⚠️⚠️⚠️ **CPF e dados de saúde**: este projeto lida com dados sensíveis (CPF, exames e diagnósticos). Os endpoints de busca (`get_exams`/`get_diagnoses`) têm a verificação de autenticação **comentada no código atual** — antes de qualquer uso além de fins acadêmicos/demonstração, essa validação precisa ser reativada e reforçada (ex: garantir que o CPF consultado pertence ao usuário autenticado, ou que o solicitante tem permissão explícita).
