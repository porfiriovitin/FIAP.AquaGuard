# AquaGuard Frontend - Documentação do Projeto

## Visão geral
- SPA React + TypeScript para monitoramento de risco de enchente em tempo real.
- Arquitetura feature-based: `src/features/` (domínios), `src/pages/` (rotas), `src/shared/` (utilitários transversais).
- Suporte nativo a dados mock via `VITE_MOCK_DATA=true` — todos os serviços alternam entre API real e dados locais.
- Consome a API REST do AquaGuard Backend; autenticação via JWT em cookie `access_token` (`HttpOnly`).

## Stack e configuração principal
- React `18.3.1` + Vite `6.0.0` + TypeScript `5.6.2` (strict mode).
- Tailwind CSS `4.3.0` via `@tailwindcss/vite`.
- React Router DOM `7.16.0` com rotas protegidas (`ProtectedRoute`).
- Mapbox GL JS `3.24.0` — mapa escuro com marcadores customizados e camada SAR de inundação.
- Framer Motion `12.40.0` — transições de página por `AnimatePresence` e modais com slide.
- Lucide React `0.453.0` — biblioteca de ícones.
- Estado de autenticação: Context API + `useReducer` (máquina de estados `idle → loading → authenticated/unauthenticated`).
- HTTP: Fetch nativo com wrapper `apiFetch` — `credentials: 'include'` para cookie JWT; evento global `auth:unauthorized` no 401.
- Sessão persistida em `localStorage` (chave `aquaguard:user`).

## Variáveis de ambiente
- `VITE_MAPBOX_TOKEN` — token público Mapbox (escopos mínimos: `styles:read`, `tiles:read`).
- `VITE_API_BASE_URL` — URL base do backend sem barra final (ex: `http://localhost:5000`).
- `VITE_MOCK_DATA` — `true` ativa dados mock em todos os serviços; qualquer outro valor desativa.

## Estrutura de pastas

```
Frontend/
├── index.html
├── vite.config.ts
├── tsconfig.json
├── .env.example
├── DESIGN.md                       # design system completo (tokens, cores, tipografia)
├── project-info.md                 # este arquivo
├── public/
│   ├── favicon.svg
│   └── landingpage/
└── src/
    ├── main.tsx                    # entrypoint React
    ├── App.tsx                     # router raiz + layout
    ├── vite-env.d.ts
    ├── assets/
    │   └── logos/
    ├── features/
    │   ├── auth/
    │   │   ├── components/         # LoginForm
    │   │   ├── context/            # AuthContext, useAuth
    │   │   ├── services/           # authService (mock-aware)
    │   │   └── types/              # AuthUser, LoginRequest
    │   ├── map/
    │   │   ├── components/         # MapView, MapMarker
    │   │   ├── data/               # floodLayers (config camada SAR)
    │   │   ├── hooks/              # useDeviceLocation
    │   │   └── services/           # mapService
    │   ├── sensor/
    │   │   ├── components/         # SensorCard, SensorDetailModal, SensorRow
    │   │   ├── data/               # mock: SENSORS, SENSOR_DETAILS
    │   │   ├── hooks/              # useSensors, useSensorDetail
    │   │   ├── services/           # sensorService
    │   │   └── utils/              # sensorStatus, trend
    │   ├── risk/
    │   │   ├── components/         # RiskCard, RiskCardCompact, RiskDetailModal
    │   │   ├── data/               # mock: RISKS, RISK_DETAILS
    │   │   ├── hooks/              # useRisks, useRiskDetail
    │   │   ├── services/           # riskService
    │   │   └── utils/              # riskStyle (cores semânticas), isUrgent
    │   ├── satellite/
    │   │   └── components/         # SatelliteCard (Sentinel-1)
    │   ├── profile/
    │   │   ├── components/         # ProfileHeader, SettingsRow, SettingsSection, StatsRow
    │   │   └── data/               # mock profile
    │   └── simulation/             # (reservado — não implementado)
    ├── pages/
    │   ├── HomePage.tsx
    │   ├── LoginPage.tsx
    │   ├── MapPage.tsx
    │   ├── RisksPage.tsx
    │   ├── SensorsPage.tsx
    │   └── ProfilePage.tsx
    ├── shared/
    │   ├── components/
    │   │   ├── auth/               # ProtectedRoute
    │   │   ├── layout/             # TopAppBar (65px), BottomNavBar (75px)
    │   │   └── ui/                 # RiskBadge, StatTile, Button, LoadingSpinner, SectionEyebrow
    │   ├── config/                 # navTabs (fonte única das abas de navegação)
    │   ├── hooks/                  # useAsync
    │   └── services/               # api.ts → apiFetch
    └── styles/
        ├── tailwind.css            # imports Tailwind + animações customizadas
        └── tokens.css              # CSS custom properties (cores, tipografia, espaçamento)
```

## Roteamento

- `GET /login` — público; redireciona para `/app` se já autenticado.
- `/app/*` — protegido por `ProtectedRoute`; redireciona para `/login` se sessão inválida.

| Tab | Path | Ícone |
|-----|------|-------|
| Home | `/app` | `Home` |
| Map | `/app/map` | `Map` |
| Risks | `/app/risks` | `Bell` (canAlert) |
| Sensors | `/app/sensors` | `Radio` |
| Profile | `/app/profile` | `User` |

- Transições: slide horizontal via `framer-motion` + `AnimatePresence`, direção baseada na ordem das tabs.

## Páginas

| Página | Path | Propósito |
|--------|------|-----------|
| `HomePage` | `/app` | Dashboard: riscos ativos, cobertura de satélite, preview do mapa, últimas leituras de sensores |
| `MapPage` | `/app/map` | Mapa Mapbox full-screen com camada SAR de inundação, localização do dispositivo e marcadores interativos |
| `RisksPage` | `/app/risks` | Lista de riscos com filtros, janelas de ação e modais de detalhe |
| `SensorsPage` | `/app/sensors` | Dashboard de telemetria: lista de sensores com status, tendência, modal de detalhe |
| `ProfilePage` | `/app/profile` | Informações do usuário, configurações do app, stats, versão |
| `LoginPage` | `/login` | Formulário de email + senha, suporte a mock, persistência em `localStorage` |

## Módulos de feature

### Auth
- `AuthContext` — provedor global; expõe `user`, `login(email, password)`, `logout()`, `status`.
- `useAuth()` — hook de consumo do contexto.
- `authService.login()` — chama `POST /api/login`; em mock retorna usuário fixo (`role: Manager = 1`).
- Restauração de sessão automática na montagem do `AuthProvider` via `localStorage`.

### Map
- `MapView` — inicializa Mapbox GL com estilo dark; aceita `center`, `zoom`, `pitch`, `bearing`, `points` (marcadores), `interactions` (enable/disable).
- `useDeviceLocation()` — `Geolocation API` com fallback para coordenadas de São Paulo.
- `mapService.getFloodLayerCallback()` — retorna callback para adicionar camada SAR ao mapa.
- Marcadores: tamanhos `sm | md | lg`, cor customizável, animação pulse opcional.

### Sensor
- Tipos: `SensorEntry` (id, stationId, location, status, currentLevel, trend).
- `useSensors()` — lista sensores; `useSensorDetail(id)` — detalhe com telemetria.
- `sensorService` — alterna entre `GET /api/sensors` e mock com base em `VITE_MOCK_DATA`.
- `sensorStatus(status)` — rótulo de display; `trend(value)` — direção da tendência.

### Risk
- Tipos: `RiskEntry` (level, riverName, stationName, currentLevel, limitLevel, trend).
- `useRisks()` — lista riscos; `useRiskDetail(id)` — detalhe com previsão de evolução e telemetria.
- `riskService` — alterna entre `GET /api/risk` e mock com base em `VITE_MOCK_DATA`.
- `isUrgent(level)` — retorna `true` para `critical | high | moderate`.
- `RiskDetailModal` e `RiskCard` incluem mapa embutido (`MapView`) com heatmap de inundação.

### Satellite
- `SatelliteCard` — exibe cobertura do satélite Sentinel-1 com parâmetros orbitais.
- Props: `name`, `scheme` (`mint | orange`), `stats` (orbit, altitude, band).

### Profile
- Componentes puramente presentacionais;

## Componentes compartilhados

### Layout
- `TopAppBar` — barra de topo (65px); brand + status de conexão.
- `BottomNavBar` — navegação inferior (75px); 5 tabs com indicador de alerta em Risks.

### UI
- `RiskBadge` — badge inline com nível de risco e ponto colorido.
- `StatTile` — KPI: número + rótulo.
- `Button` — botão de ação primária.
- `LoadingSpinner` — spinner animado.
- `SectionEyebrow` — cabeçalho de seção (12px, uppercase, `cyan-600`).

### Hooks
- `useAsync<T>(fn, deps)` — abstração genérica de estado assíncrono; retorna `{ data, isLoading, error }`.

### Serviços
- `apiFetch<T>(path, options)` — wrapper sobre Fetch.
  - Prefixo automático com `VITE_API_BASE_URL`.
  - `credentials: 'include'` em todas as requisições.
  - Retorna `PayloadResponse<T>`.
  - Em status 401: dispara evento `auth:unauthorized` (desacoplado do `AuthContext`).

## Sistema de design

- Documentação completa em `DESIGN.md` e tokens em `src/styles/tokens.css`.
- Famílias tipográficas:
  - `Sora` — títulos e display.
  - `DM Sans` — corpo de texto.
  - `JetBrains Mono` — telemetria e valores numéricos.
- Cores semânticas de risco (alinhadas com Defesa Civil brasileira):
  - `critical` → `#d63a3a` (vermelho)
  - `high` → `#ef7a1a` (laranja)
  - `moderate` → `#f1b50a` (amarelo)
  - `low` → `#1bb892` (mint)
  - `normal` → `#2c79bf` (azul)
- Paletas de identidade: `navy` (identidade, wordmark), `blue` (ações), `cyan` (sinal ao vivo), `ink` (neutros).
- Espaçamento base de 4px (`--s-1` a `--s-24`).
- Raio de borda: `xs` (4px) até `pill` (999px).
- Animações: `pulse`, `pulse-crit`, `ping`, `orbit`, `live-blink`.

## Contratos de tipos relevantes

### `RiskLevel` (`src/features/risk/`)
- `'critical'`
- `'high'`
- `'moderate'`
- `'low'`
- `'normal'`

Observação: espelha o enum `RiskLevel` do backend, porém representado como string (não inteiro) no frontend.

### `SensorStatus` (`src/features/sensor/`)
- `'online'`
- `'unstable'`
- `'offline'`

Observação: espelha `SensorStatus` do backend (`Active = 0`, `Inactive = 1`, `Maintenance = 2`) com mapeamento semântico.

### `AuthUser` e `UserRole` (`src/features/auth/types/`)
- `Admin = 0`
- `Manager = 1`
- `Employee = 2`
- `Resident = 3`

Observação: valores inteiros idênticos ao enum `UserRole` do backend.

### `PayloadResponse<T>` (`src/shared/services/api.ts`)
- `status: string` — `'Success'` ou `'Error'` (espelha `ResponseStatus` do backend).
- `data?: T` — payload tipado.
- `message?: string` — mensagem de erro ou informação.

## Comandos de desenvolvimento

- `npm run dev` — inicia servidor Vite com HMR.
- `npm run build` — checagem TypeScript + build de produção em `dist/`.
- `npm run preview` — serve o build de produção localmente.
