import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
// Order matters: tailwind base → design tokens → semantic overrides
import './styles/tailwind.css'
import './styles/tokens.css'
import './styles/base.css'
import App from './App'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
