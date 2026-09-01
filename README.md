# TaskFlow API

API REST para gerenciamento de tarefas desenvolvida com **C# e .NET**, utilizando **ASP.NET Core, PostgreSQL, Entity Framework Core, autenticação JWT e arquitetura em camadas**.

O projeto foi criado para demonstrar boas práticas de desenvolvimento backend, separação de responsabilidades, persistência de dados, segurança e organização de código em uma aplicação real.

---

## Visão geral

O TaskFlow permite que usuários se cadastrem, façam login e gerenciem suas próprias tarefas por meio de endpoints protegidos com autenticação JWT.

Principais recursos:

- cadastro de usuários
- login com geração de JWT
- senhas protegidas com BCrypt
- criação de tarefas
- consulta de tarefas
- edição de tarefas
- exclusão de tarefas
- alteração de status
- prioridades
- datas de vencimento
- persistência com PostgreSQL
- migrations com Entity Framework Core
- documentação via Swagger
- tratamento global de exceções
- arquitetura em camadas
- testes preparados em projeto separado

---

## Stack

### Backend

- C#
- .NET
- ASP.NET Core Web API

### Banco de dados

- PostgreSQL
- Entity Framework Core
- Npgsql

### Segurança

- JWT Bearer Authentication
- BCrypt

### Documentação e ferramentas

- Swagger / OpenAPI
- Git
- GitHub
- Visual Studio Code
- pgAdmin

---

## Arquitetura

```text
software-csharp-api/
│
├── TaskFlow.Api/
├── TaskFlow.Application/
├── TaskFlow.Domain/
├── TaskFlow.Infrastructure/
├── TaskFlow.Tests/
│
└── TaskFlow.slnx
```

### TaskFlow.Api

Responsável pela camada HTTP: controllers, autenticação, autorização, middleware, configuração da aplicação e Swagger.

### TaskFlow.Application

Responsável pelos casos de uso, DTOs, serviços, interfaces e contratos.

### TaskFlow.Domain

Núcleo da aplicação com entidades, enums e regras de negócio.

### TaskFlow.Infrastructure

Implementa PostgreSQL, Entity Framework Core, repositories, migrations, BCrypt e JWT.

### TaskFlow.Tests

Projeto destinado aos testes automatizados da solução.

---

## Fluxo da aplicação

```text
HTTP Request
      |
      v
TaskFlow.Api
      |
      v
Application Services
      |
      v
Domain
      |
      v
Repository Interfaces
      |
      v
Infrastructure
      |
      v
Entity Framework Core
      |
      v
PostgreSQL
```

---

## Modelo de domínio

### User

```text
User
├── Id
├── Name
├── Email
├── PasswordHash
├── CreatedAt
├── UpdatedAt
└── Tasks
```

### TaskItem

```text
TaskItem
├── Id
├── UserId
├── Title
├── Description
├── Status
├── Priority
├── DueDate
├── CreatedAt
├── UpdatedAt
└── CompletedAt
```

---

## Status das tarefas

```text
Pending = 1
InProgress = 2
Completed = 3
Cancelled = 4
```

## Prioridades

```text
Low = 1
Medium = 2
High = 3
Critical = 4
```

---

## Endpoints

### Autenticação

```http
POST /api/auth/register
POST /api/auth/login
```

Exemplo de cadastro:

```json
{
  "name": "João Viana",
  "email": "joao@taskflow.dev",
  "password": "TaskFlow123"
}
```

### Tarefas

```http
GET    /api/tasks
GET    /api/tasks/{id}
POST   /api/tasks
PUT    /api/tasks/{id}
PATCH  /api/tasks/{id}/start
PATCH  /api/tasks/{id}/complete
PATCH  /api/tasks/{id}/reopen
PATCH  /api/tasks/{id}/cancel
DELETE /api/tasks/{id}
```

Os endpoints protegidos exigem:

```http
Authorization: Bearer SEU_TOKEN
```

---

## Segurança

A autenticação usa JWT e as senhas são armazenadas como hash BCrypt.

```text
E-mail + senha
      |
      v
AuthService
      |
      +---- UserRepository
      |
      +---- BCrypt.Verify()
      |
      +---- JwtTokenService
                    |
                    v
                JWT Token
```

---

## Banco de dados

```text
taskflow_db
├── users
├── tasks
└── __EFMigrationsHistory
```

Relacionamento:

```text
users 1 ---- N tasks
```

---

## Entity Framework Core

O projeto utiliza:

- DbContext
- DbSet
- IEntityTypeConfiguration
- Fluent API
- índices
- constraints
- relacionamento 1:N
- migrations
- conversão de enums
- cascade delete

Criar migration:

```powershell
dotnet ef migrations add NomeDaMigration `
  --project .\TaskFlow.Infrastructure\TaskFlow.Infrastructure.csproj `
  --startup-project .\TaskFlow.Api\TaskFlow.Api.csproj `
  --output-dir Data\Migrations
```

Aplicar migration:

```powershell
dotnet ef database update `
  --project .\TaskFlow.Infrastructure\TaskFlow.Infrastructure.csproj `
  --startup-project .\TaskFlow.Api\TaskFlow.Api.csproj
```

---

## Configuração local

Pré-requisitos:

- .NET SDK
- PostgreSQL
- Git

Clone:

```bash
git clone https://github.com/JoaoViana-bezerra/software-csharp-api.git
cd software-csharp-api
```

Restaure e compile:

```powershell
dotnet restore .\TaskFlow.slnx
dotnet build .\TaskFlow.slnx
```

---

## Segredos locais

Não versionar credenciais reais.

```powershell
dotnet user-secrets init --project .\TaskFlow.Api\TaskFlow.Api.csproj
```

Connection string:

```powershell
dotnet user-secrets set "ConnectionStrings:PostgreSQL" "Host=localhost;Port=5432;Database=taskflow_db;Username=postgres;Password=SUA_SENHA" --project .\TaskFlow.Api\TaskFlow.Api.csproj
```

JWT:

```powershell
dotnet user-secrets set "Jwt:Key" "SUA_CHAVE_FORTE_COM_PELO_MENOS_32_CARACTERES" --project .\TaskFlow.Api\TaskFlow.Api.csproj
```

---

## Executar

```powershell
dotnet run --project .\TaskFlow.Api\TaskFlow.Api.csproj
```

Swagger em desenvolvimento:

```text
http://localhost:5057/swagger
```

Health endpoint:

```http
GET /
```

Exemplo:

```json
{
  "name": "TaskFlow API",
  "version": "1.0.0",
  "status": "running"
}
```

---

## Tratamento de erros

| Situação | HTTP |
|---|---:|
| Dados inválidos | 400 |
| Credenciais inválidas | 401 |
| Recurso não encontrado | 404 |
| Erro inesperado | 500 |

Exemplo:

```json
{
  "status": 400,
  "error": "BadRequest",
  "message": "A user with this email already exists."
}
```

---

## Boas práticas aplicadas

- arquitetura em camadas
- separação de responsabilidades
- dependency injection
- repository pattern
- DTOs
- encapsulamento de domínio
- async/await
- CancellationToken
- Entity Framework Fluent API
- migrations
- JWT
- hashing de senha
- middleware global
- configuração externa
- User Secrets
- rotas REST
- Swagger

---

## Roadmap

```text
[x] Domain
[x] Application
[x] Infrastructure
[x] PostgreSQL
[x] Entity Framework Core
[x] Migrations
[x] BCrypt
[x] JWT
[x] Controllers
[x] Swagger
[x] CRUD de tarefas
[ ] Testes automatizados
[ ] Filtros e paginação
[ ] Docker
[ ] CI/CD
[ ] Deploy
```

---

## Objetivo do projeto

Este projeto faz parte do meu portfólio de desenvolvimento e demonstra conhecimentos de backend no ecossistema .NET.

```text
C#
+
.NET
+
ASP.NET Core
+
REST API
+
PostgreSQL
+
Entity Framework Core
+
JWT
+
Arquitetura de Software
```

---

## Autor

**João Viana**

Desenvolvimento de Software | Automação | Dados

GitHub: [JoaoViana-bezerra](https://github.com/JoaoViana-bezerra)
