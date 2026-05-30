# AquaGuard — Design System

> Plataforma inteligente de monitoramento e simulação de risco de enchentes.

---

## Sumário

1. [Tipografia](#tipografia)
2. [Paleta de Cores](#paleta-de-cores)
3. [Cores Semânticas de Risco](#cores-semânticas-de-risco)
4. [Tokens de UI](#tokens-de-ui)
5. [Espaçamento](#espaçamento)
6. [Bordas (Radius)](#bordas-radius)
7. [Sombras e Anéis](#sombras-e-anéis)

---

## Tipografia

### Famílias

| Token | Família | Uso |
|---|---|---|
| `--font-display` | Sora | Títulos e display — caráter geométrico e engineered |
| `--font-sans` | DM Sans | Corpo de texto e UI — legível e neutro |
| `--font-mono` | JetBrains Mono | Telemetria, coordenadas, IDs de sensores |

**Importação (Google Fonts):**
```css
@import url('https://fonts.googleapis.com/css2?family=Sora:wght@300;400;500;600;700;800&family=DM+Sans:opsz,wght@9..40,400;9..40,500;9..40,600;9..40,700&family=JetBrains+Mono:wght@400;500;600&display=swap');
```

---

### Tamanhos

| Token | Valor | Classe / Uso |
|---|---|---|
| `--t-eyebrow` | `12px` | `.t-eyebrow` — labels em uppercase |
| `--t-caption` | `13px` | `.t-caption` — legendas e notas |
| `--t-body-sm` | `14px` | Texto secundário compacto |
| `--t-body` | `15px` | `.t-body`, `p` — corpo padrão |
| `--t-body-lg` | `17px` | `.t-body-lg` — lide, introduções |
| `--t-h6` | `18px` | `h6`, `.t-h6` |
| `--t-h5` | `20px` | `h5`, `.t-h5` |
| `--t-h4` | `24px` | `h4`, `.t-h4` |
| `--t-h3` | `30px` | `h3`, `.t-h3` |
| `--t-h2` | `40px` | `h2`, `.t-h2` |
| `--t-h1` | `56px` | `h1`, `.t-h1` |
| `--t-hero` | `76px` | `.t-hero`, `h1.hero` — hero sections |

---

### Pesos

| Token | Valor |
|---|---|
| `--w-regular` | `400` |
| `--w-medium` | `500` |
| `--w-semibold` | `600` |
| `--w-bold` | `700` |
| `--w-extra` | `800` |

---

### Line-heights

| Token | Valor | Uso |
|---|---|---|
| `--lh-tight` | `1.08` | Headings grandes (h1, hero) |
| `--lh-snug` | `1.22` | Headings médios (h2–h5) |
| `--lh-base` | `1.5` | Corpo padrão |
| `--lh-relaxed` | `1.65` | Parágrafos e lides |

---

### Letter-spacing (Tracking)

| Token | Valor | Uso |
|---|---|---|
| `--tr-tighter` | `-0.035em` | Display headings |
| `--tr-tight` | `-0.02em` | h2, h3 |
| `--tr-normal` | `0` | Corpo |
| `--tr-wide` | `0.04em` | — |
| `--tr-eyebrow` | `0.12em` | Labels uppercase |

---

### Classes semânticas de tipo

| Classe | Fonte | Peso | Tamanho | Line-height | Uso |
|---|---|---|---|---|---|
| `.t-hero` | Display | 800 | 76px | tight | Hero sections |
| `h1`, `.t-h1` | Display | 700 | 56px | tight | Títulos principais |
| `h2`, `.t-h2` | Display | 700 | 40px | snug | Seções |
| `h3`, `.t-h3` | Display | 600 | 30px | snug | Subseções |
| `h4`, `.t-h4` | Display | 600 | 24px | snug | Cards e painéis |
| `h5`, `.t-h5` | Sans | 600 | 20px | snug | Títulos de componente |
| `h6`, `.t-h6` | Sans | 600 | 18px | snug | Rótulos de seção |
| `.t-body-lg` | Sans | 400 | 17px | relaxed | Introduções |
| `p`, `.t-body` | Sans | 400 | 15px | relaxed | Corpo padrão |
| `.t-caption` | Sans | 400 | 13px | base | Legendas |
| `.t-eyebrow` | Sans | 600 | 12px | — | Labels uppercase em `--cyan-600` |
| `.t-mono` | Mono | — | — | — | Dados de sensores, coordenadas |
| `.t-num` | — | — | — | — | `tabular-nums` para números alinhados |
| `code`, `.code` | Mono | — | 0.92em | — | Código inline, leituras de sensor |

---

## Paleta de Cores

### Navy — Identidade primária

| Token | Hex | Uso |
|---|---|---|
| `--navy-950` | `#061a33` | Superfícies mais profundas |
| `--navy-900` | `#0b2445` | Wordmark, texto de títulos |
| `--navy-800` | `#14365e` | Contorno do logo, sidebars |
| `--navy-700` | `#1a4a7e` | — |
| `--navy-600` | `#21609d` | — |
| `--navy-500` | `#2c79bf` | Nível de risco normal |

---

### Blue — Ação / Links / Plots

| Token | Hex | Uso |
|---|---|---|
| `--blue-700` | `#1455c0` | Hover do accent |
| `--blue-600` | `#1f6fe5` | CTA primário (`--accent`) |
| `--blue-500` | `#3b82f6` | — |
| `--blue-400` | `#6fa8f5` | — |
| `--blue-300` | `#a8c8f8` | — |
| `--blue-100` | `#e4eefd` | Tint de risco normal |
| `--blue-50` | `#f1f6fe` | — |

---

### Cyan / Teal — Dados ao vivo, pulsação de sensores

| Token | Hex | Uso |
|---|---|---|
| `--cyan-700` | `#0f8a93` | — |
| `--cyan-600` | `#149aa4` | Cor do `.t-eyebrow` |
| `--cyan-500` | `#20bfc8` | `--signal` — telemetria |
| `--cyan-400` | `#2dd4d4` | — |
| `--cyan-300` | `#6ee5e0` | — |
| `--cyan-100` | `#d0f5f3` | `--signal-soft` |
| `--cyan-50` | `#ecfbfa` | — |

---

### Mint — Deltas positivos, recuperação

| Token | Hex | Uso |
|---|---|---|
| `--mint-500` | `#1bb892` | `--success`, risco baixo |
| `--mint-400` | `#46dcc3` | — |
| `--mint-300` | `#8aeed7` | — |
| `--mint-100` | `#d6f6ec` | Tint de risco baixo |

---

### Neutrals (Ink) — Slates com matiz frio

| Token | Hex | Uso |
|---|---|---|
| `--ink-950` | `#050b15` | — |
| `--ink-900` | `#0d1726` | Texto primário (`--fg`) |
| `--ink-700` | `#2a3a52` | — |
| `--ink-600` | `#475672` | Texto secundário (`--fg-muted`) |
| `--ink-500` | `#677694` | Texto terciário (`--fg-subtle`) |
| `--ink-400` | `#94a1ba` | — |
| `--ink-300` | `#c2cad8` | Borda forte |
| `--ink-200` | `#dde3ec` | Borda padrão |
| `--ink-150` | `#e7ecf3` | — |
| `--ink-100` | `#f1f4f9` | Fundo recuado (`--bg-sunken`) |
| `--ink-50` | `#f7f9fc` | Fundo padrão (`--bg`) |
| `--white` | `#ffffff` | Superfície elevada (`--bg-elevated`) |

---

## Cores Semânticas de Risco

> Mapeadas às categorias de monitoramento de enchentes da **Defesa Civil Brasileira**.

| Nível | Token (cor) | Hex | Token (tint) | Hex |
|---|---|---|---|---|
| Crítico | `--risk-critical` | `#d63a3a` | `--risk-critical-tint` | `#fde6e6` |
| Alto | `--risk-high` | `#ef7a1a` | `--risk-high-tint` | `#fdecd7` |
| Moderado | `--risk-moderate` | `#f1b50a` | `--risk-moderate-tint` | `#fdf4d2` |
| Baixo | `--risk-low` | `#1bb892` | `--risk-low-tint` | `#d6f6ec` |
| Normal | `--risk-normal` | `#2c79bf` | `--risk-normal-tint` | `#e4eefd` |

### Status de UI

| Token | Alias |
|---|---|
| `--success` | `--mint-500` |
| `--warning` | `--risk-moderate` |
| `--danger` | `--risk-critical` |
| `--info` | `--blue-600` |

---

## Tokens de UI

### Superfícies

| Token | Valor |
|---|---|
| `--bg` | `--ink-50` — fundo padrão da página |
| `--bg-elevated` | `--white` — cards, modais |
| `--bg-sunken` | `--ink-100` — wells, áreas recuadas |
| `--bg-inverse` | `--navy-900` — sidebars escuras, headers |

### Texto

| Token | Valor |
|---|---|
| `--fg` | `--ink-900` — texto primário |
| `--fg-muted` | `--ink-600` — texto secundário |
| `--fg-subtle` | `--ink-500` — texto terciário |
| `--fg-onDark` | `#eaf2ff` — texto sobre fundo escuro |
| `--fg-onDarkMuted` | `#9fb4d4` — texto secundário sobre fundo escuro |

### Bordas

| Token | Valor |
|---|---|
| `--border` | `--ink-200` — borda padrão |
| `--border-strong` | `--ink-300` — borda enfatizada |
| `--border-onDark` | `#1d3a63` — borda sobre fundo escuro |

### Accent

| Token | Valor |
|---|---|
| `--accent` | `--blue-600` — CTA, links |
| `--accent-hover` | `--blue-700` — estado hover |
| `--accent-press` | `#103e8f` — estado pressionado |

### Signal (Telemetria ao vivo)

| Token | Valor |
|---|---|
| `--signal` | `--cyan-500` — pulsos, dados em streaming |
| `--signal-soft` | `--cyan-100` — fundo suave de live badge |

---

## Espaçamento

> Base de 4px. Uso: padding, margin, gap.

| Token | Valor |
|---|---|
| `--s-0` | `0` |
| `--s-1` | `4px` |
| `--s-2` | `8px` |
| `--s-3` | `12px` |
| `--s-4` | `16px` |
| `--s-5` | `20px` |
| `--s-6` | `24px` |
| `--s-8` | `32px` |
| `--s-10` | `40px` |
| `--s-12` | `48px` |
| `--s-16` | `64px` |
| `--s-20` | `80px` |
| `--s-24` | `96px` |

---

## Bordas (Radius)

> Soft mas disciplinado — identidade engineered, não lúdica.

| Token | Valor | Uso típico |
|---|---|---|
| `--radius-xs` | `4px` | Tags inline, `code` |
| `--radius-sm` | `6px` | Botões pequenos, inputs |
| `--radius-md` | `10px` | Botões padrão, cards compactos |
| `--radius-lg` | `14px` | Cards |
| `--radius-xl` | `20px` | Painéis, modais |
| `--radius-2xl` | `28px` | Drawers, sheets grandes |
| `--radius-pill` | `999px` | Badges, chips, avatares |

---

## Sombras e Anéis

### Elevação (`elev`)

| Token | Valor |
|---|---|
| `--shadow-xs` | `0 1px 2px rgba(11,37,69,.06)` |
| `--shadow-sm` | `0 1px 2px rgba(11,37,69,.06), 0 1px 3px rgba(11,37,69,.05)` |
| `--shadow-md` | `0 4px 12px -2px rgba(11,37,69,.10), 0 2px 4px rgba(11,37,69,.04)` |
| `--shadow-lg` | `0 16px 40px -12px rgba(11,37,69,.18), 0 4px 10px rgba(11,37,69,.06)` |
| `--shadow-xl` | `0 28px 60px -20px rgba(11,37,69,.30)` |

### Anéis de foco/sinal (`ring`)

| Token | Valor | Uso |
|---|---|---|
| `--ring-focus` | `0 0 0 3px rgba(31,111,229,.30)` | Foco de teclado (azul) |
| `--ring-signal` | `0 0 0 4px rgba(32,191,200,.22)` | Pulsação de sensor (cyan) |
| `--ring-danger` | `0 0 0 3px rgba(214,58,58,.25)` | Estado de erro/crítico |

### Inset

| Token | Valor | Uso |
|---|---|---|
| `--inset-soft` | `inset 0 1px 0 rgba(11,37,69,.04)` | Wells de gráficos e mapas |
