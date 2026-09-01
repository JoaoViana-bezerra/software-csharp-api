<<<<<<< HEAD
# Correções — TaskFlow Etapa 05

Estes arquivos corrigem os erros mostrados em:

- `TaskFlow.Api/Program.cs`
- `TaskFlow.Application/DependencyInjection.cs`

## 1. Substituir arquivos

Copie:

```text
TaskFlow.Api/Program.cs
```

para o projeto `TaskFlow.Api`.

Copie:

```text
TaskFlow.Application/DependencyInjection.cs
```

para o projeto `TaskFlow.Application`.

## 2. Pacote necessário no Application

Na raiz da solução:

```powershell
dotnet add .\TaskFlow.Application\TaskFlow.Application.csproj package Microsoft.Extensions.DependencyInjection.Abstractions
```

## 3. Conferir pacotes da API

```powershell
dotnet list .\TaskFlow.Api\TaskFlow.Api.csproj package
```

Devem existir pelo menos:

```text
Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.EntityFrameworkCore.Design
Swashbuckle.AspNetCore
```

Se faltar:

```powershell
dotnet add .\TaskFlow.Api\TaskFlow.Api.csproj package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add .\TaskFlow.Api\TaskFlow.Api.csproj package Swashbuckle.AspNetCore
```

## 4. Restaurar e compilar

```powershell
dotnet restore .\TaskFlow.slnx
dotnet clean .\TaskFlow.slnx
dotnet build .\TaskFlow.slnx
```

## Por que o Program.cs foi simplificado?

A configuração anterior usava tipos como:

```text
Microsoft.OpenApi.Models
OpenApiInfo
OpenApiSecurityScheme
OpenApiReference
```

As versões atuais de OpenAPI/Swashbuckle usadas com .NET 10 possuem diferenças de API.

Nesta correção, o Swagger continua habilitado com:

```csharp
builder.Services.AddSwaggerGen();
```

sem depender diretamente desses tipos.

Assim conseguimos primeiro:

- compilar a solution;
- validar JWT;
- validar controllers;
- testar os endpoints.

Depois podemos adicionar o botão `Authorize` do Swagger usando a API compatível com as versões exatas instaladas.
=======
# software-csharp-api
Backend em C#/.NET com API REST, JWT, PostgreSQL, Entity Framework Core e arquitetura modular para gerenciamento de tarefas.
>>>>>>> 3c03b78dbd5e98c056db15f84b97a1ff52d0d49e
