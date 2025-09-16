
# InsurancePlatform – Sistema Modular de Propostas e Contratos de Seguro
> Plataforma moderna baseada em microsserviços com arquitetura hexagonal e comunicação assíncrona via eventos, voltada para o domínio de seguros.

---

## 🧭 Visão Geral

**InsurancePlatform** é uma aplicação backend projetada para atender o fluxo de **propostas e contratação de seguros** com rastreabilidade, segurança e desacoplamento entre domínios.

Construída com **.NET 9**, segue os princípios de **Clean Architecture**, **DDD** e **mensageria assíncrona** com RabbitMQ. Ideal para aplicações escaláveis e de alta responsabilidade.

---

## 📌 Funcionalidades Incluídas

- 🧱 Arquitetura hexagonal com separação por camadas
- 📦 Microsserviços independentes com comunicação via RabbitMQ
- 🔐 Autenticação JWT integrada
- 🧪 Testes automatizados (unitários e BDD com SpecFlow)
- 🐳 Docker e Docker Compose prontos para execução
- 📥 Coleção Postman para testes manuais

## 🎯 Problemas que resolve

- Garantia de integridade entre módulos via eventos  
- Validação automática de propostas antes da contratação  
- Rastreabilidade ponta-a-ponta com `X-Correlation-ID`  
- Separação de domínios (Proposta ≠ Contratação)

---

## 🧱 Arquitetura

```
┌────────────────────┐        ┌─────────────────────────┐
│  ProposalService   │ ─────► │ RabbitMQ (eventos)      │
│  (API REST)        │        └────────────┬────────────┘
│                    │                    ▼
│ • Cria propostas   │         ┌───────────────────────┐
│ • Aprova/Rejeita   │ ◄────── │ ContractingService     │
└────────────────────┘         │ (consome eventos)      │
                               │ • Contrata proposta    │
                               │ • Persiste contrato    │
                               └────────────────────────┘
```

---

## ⚙️ Tecnologias Utilizadas

| Tecnologia       | Função                          |
|------------------|---------------------------------|
| C# / .NET 9      | Backend e APIs                  |
| Docker           | Containerização                 |
| RabbitMQ         | Mensageria assíncrona           |
| SQL Server       | Banco de dados relacional       |
| SpecFlow / xUnit | Testes unitários e BDD          |
| Postman          | Testes manuais de endpoints     |

---

## 📂 Estrutura de Pastas

```
/src
  /ProposalService
    ├── .Api
    ├── .Application
    ├── .Domain
    └── .Infrastructure

  /ContractingService
    ├── .Api
    ├── .Application
    ├── .Domain
    └── .Infrastructure

  /Shared
    ├── Shared.Contracts
    └── Shared.CrossCutting

/tests
  ├── .UnitTests
  └── .IntegrationTests
```

---

## ♻️ Camadas Compartilhadas (`Shared`)

### 🔹 `Shared.Contracts`
- Contratos de eventos (ex: `PropostaAprovadaEvent`)
- Reaproveitamento entre serviços
- Define payloads comuns com versionamento

### 🔹 `Shared.CrossCutting`
- Middleware para logging e `X-Correlation-ID`
- Autenticação JWT
- Mensageria com abstrações
- Notificações, responses padronizadas e extensions

---

## 🚀 Como Executar o Projeto

```bash
docker-compose up --build
```

Depois de subir os containers, você pode:

- Acessar APIs: http://localhost:5001 e http://localhost:5002
- Realizar login, criar propostas e contratos
- 📦 Executar testes automatizados:

```bash
dotnet test
```

- RabbitMQ UI: http://localhost:15672 (guest/guest)

---

## 🔐 Autenticação

```http
POST /api/v1/auth/login
{
  "username": "admin",
  "password": "123456"
}
```

Header:
```http
Authorization: Bearer {{token}}
```

---

## 📄 Criar Proposta

```http
POST /proposals
Authorization: Bearer {{token}}

{
  "customer": {
    "name": "João da Silva",
    "document": "12345678900",
    "email": "joao@email.com"
  },
  "contract": {
    "type": "Standard",
    "premium": 120.5,
    "startDate": "2025-10-01",
    "endDate": "2026-10-01"
  }
}
```

---

## ✅ Aprovar / Rejeitar Proposta

```http
PUT /proposals/{id}/approve
PUT /proposals/{id}/reject
```

---

## 🤝 Criar Contrato

```http
POST /contracts
{
  "proposalId": "guid-da-proposta"
}
```

---

## 📬 Eventos Assíncronos

| Evento                 | Emitido por       | Consumido por       |
|------------------------|-------------------|----------------------|
| PropostaAprovadaEvent  | ProposalService   | ContractingService   |

- Comunicação via RabbitMQ  
- Validação de idempotência via `EventId`  
- Logs de sucesso e falha automáticos

---

## 🧪 Testes Automatizados

```bash
dotnet test
```

- Testes de domínio e casos de uso  
- Integração com controllers e repositórios  
- BDD com SpecFlow  
- Cobertura mínima: 80%

---

## 📦 Migrations (Entity Framework Core)

### 🛠️ Gerar Migration

```bash
# ContractingService
dotnet ef migrations add NomeDaMigration   --project src/ContractingService/ContractingService.Infrastructure   --startup-project src/ContractingService/ContractingService.Api

# ProposalService
dotnet ef migrations add NomeDaMigration   --project src/ProposalService/ProposalService.Infrastructure   --startup-project src/ProposalService/ProposalService.Api
```

### 🧹 Remover Última Migration

```bash
# ContractingService
dotnet ef migrations remove   --project src/ContractingService/ContractingService.Infrastructure   --startup-project src/ContractingService/ContractingService.Api

# ProposalService
dotnet ef migrations remove   --project src/ProposalService/ProposalService.Infrastructure   --startup-project src/ProposalService/ProposalService.Api
```

### 📥 Aplicar Migration

```bash
# ContractingService
dotnet ef database update   --project src/ContractingService/ContractingService.Infrastructure   --startup-project src/ContractingService/ContractingService.Api

# ProposalService
dotnet ef database update   --project src/ProposalService/ProposalService.Infrastructure   --startup-project src/ProposalService/ProposalService.Api
```

---

## ✅ Critérios de Aceite

- Retorno padrão `{ success, data, messages }`  
- Proposta só pode ser contratada se estiver aprovada  
- Logs e falhas são registrados automaticamente  
- Cobertura mínima de testes: **80%**  
- `X-Correlation-ID` propagado entre serviços

---

## 📥 Testar com Postman

1. [📥 Baixar Coleção Postman](./postman/insuranceplatform_collection.json)  
2. [🌐 Baixar Ambiente Postman](./postman/insuranceplatform_environment.json)  
3. Execute login, crie proposta, aprove e aguarde evento  
4. Consulte contrato via API

---

## 🧑‍💻 Autoria

Desenvolvido por **Tiago Nogueira**  
Desafio técnico com arquitetura hexagonal, microsserviços, testes e mensageria.

---

## 📌 Comece Agora

- 🚀 Suba o ambiente: `docker-compose up --build`  
- 📄 Cadastre uma proposta  
- ✅ Aprove a proposta  
- 📨 Aguarde o evento e verifique o contrato gerado

---

## 📝 Licença

MIT © 2025 – Livre para modificação e uso.
