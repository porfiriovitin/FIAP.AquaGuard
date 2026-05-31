# AquaGuard Backend - Documentação do Projeto e API

## Visão geral
- Solução backend em `.NET` para gestão de cidades, usuários, sensores e análise de risco de enchente.
- Arquitetura em camadas:
- `FIAP.AquaGuard.API`: controllers e configuração HTTP.
- `FIAP.Aquaguard.Application`: casos de uso e contratos de entrada/saída.
- `FIAP.AquaGuard.Domain`: entidades, enums e modelos de domínio.
- `FIAP.AquaGuard.Infrastructure`: persistência (EF Core + PostgreSQL), repositórios e integrações.

## Stack e configuração principal
- ASP.NET Core Web API com Swagger no ambiente de desenvolvimento.
- Autenticação JWT Bearer, com leitura do token no cookie `access_token`.
- Banco de dados PostgreSQL via `Npgsql` e `Entity Framework Core`.
- Rate limit global: `60` requisições por minuto por IP.
- Localização habilitada para `en`, `pt-BR`, `es`.

## Autorização (policies)
- `AdminOnly`: requer role `Admin`.
- `ManagerOrAdmin`: requer role `Admin` ou `Manager`.
- `EmployeeManagerOrAdmin`: requer role `Admin`, `Manager` ou `Employee`.
- `AuthenticatedUsers`: requer usuário autenticado.

## Contrato de resposta
- As rotas retornam `PayloadResponse<T>`.
- Campo `Status` usa `nameof(ResponseStatus.Success)` ou `nameof(ResponseStatus.Error)` (string).

## Endpoints da API

### Auth
- `POST /api/login`
- Acesso: público.
- Ação: autentica usuário, grava cookie de acesso e retorna dados do usuário logado.

### Users (`/api/users`)
- `POST /api/users/register`
- Acesso: `ManagerOrAdmin`.
- Ação: registra usuário.
- `GET /api/users/{id}`
- Acesso: `ManagerOrAdmin`.
- Ação: busca usuário por id.
- `GET /api/users?cityId={guid?}&page={n}&pageSize={n}`
- Acesso: `ManagerOrAdmin`.
- Ação: lista usuários com paginação.
- `PUT /api/users/{id}`
- Acesso: `ManagerOrAdmin`.
- Ação: atualiza nome e email do usuário.
- `DELETE /api/users/{id}`
- Acesso: `ManagerOrAdmin`.
- Ação: remove usuário.

### Cities (`/api/cities`)
- `POST /api/cities`
- Acesso: `AdminOnly`.
- Ação: cria cidade.
- `GET /api/cities/{id}`
- Acesso: `AdminOnly`.
- Ação: busca cidade por id.
- `GET /api/cities?page={n}&pageSize={n}`
- Acesso: `AdminOnly`.
- Ação: lista cidades com paginação.
- `PUT /api/cities/{id}`
- Acesso: `AdminOnly`.
- Ação: atualiza coordenadas da cidade.
- `PATCH /api/cities/plan?id={guid}&isPaidPlan={0|1}`
- Acesso: `AdminOnly`.
- Ação: atualiza plano pago da cidade (`short`, não enum).
- `DELETE /api/cities/{id}`
- Acesso: `AdminOnly`.
- Ação: remove cidade.

### Sensors (`/api/sensors`)
- `POST /api/sensors`
- Acesso: `ManagerOrAdmin`.
- Ação: cria sensor.
- `GET /api/sensors/{id}`
- Acesso: `ManagerOrAdmin`.
- Ação: busca sensor por id.
- `GET /api/sensors?cityId={guid}&page={n}&pageSize={n}`
- Acesso: autenticado (`[Authorize]`).
- Ação: lista sensores com paginação.
- `PUT /api/sensors/{id}`
- Acesso: `ManagerOrAdmin`.
- Ação: atualiza dados de sensor.
- `PATCH /api/sensors/status?id={guid}&status={int}`
- Acesso: `AdminOnly`.
- Ação: atualiza status do sensor por inteiro convertido para enum `SensorStatus`.
- `DELETE /api/sensors/{id}`
- Acesso: `ManagerOrAdmin`.
- Ação: remove sensor.

### Risk (`/api/risk`)
- `GET /api/risk/simple?latitude={double}&longitude={double}`
- Acesso: público.
- Ação: consulta de risco simplificada.
- `GET /api/risk?latitude={double}&longitude={double}`
- Acesso: público.
- Ação: consulta de risco detalhada por coordenadas.

## Enums e valores numéricos (importante para integração)

### `UserRole` (`FIAP.AquaGuard.Domain.Enums`)
- `Admin = 0`
- `Manager = 1`
- `Employee = 2`
- `Resident = 3`

### `SensorType` (`FIAP.AquaGuard.Domain.Enums`)
- `WaterLevel = 0`
- `FlowRate = 1`
- `RainGauge = 2`

### `SensorStatus` (`FIAP.AquaGuard.Domain.Enums`)
- `Active = 0`
- `Inactive = 1`
- `Maintenance = 2`

### `RiskSourceType` (`FIAP.AquaGuard.Domain.Enums`)
- `Weather = 0`
- `Flow = 1`
- `Elevation = 2`
- `Satellite = 3`
- `Sensor = 4`

### `RiskLevel` (`FIAP.AquaGuard.Domain.Enums`)
- `Low = 0`
- `Moderate = 1`
- `High = 2`
- `Critical = 3`

### `RiskLevel` (`FIAP.AquaGuard.Domain.Models.FloodRiskResult`)
- `Low = 0`
- `Moderate = 1`
- `High = 2`
- `Critical = 3`

### `ResponseStatus` (`FIAP.AquaGuard.Application.Shared.Responses`)
- `Success = 0`
- `Error = 1`

Observação importante:
- Em C#, enums começam em `0` por padrão quando não há valor explícito definido.
- Exemplo prático: para enviar `SensorStatus.Active`, o valor correto é `0`.
