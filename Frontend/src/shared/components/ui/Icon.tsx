import {
  Activity, ArrowDown, ArrowUp, ArrowUpRight,
  Bell, Calendar, CircleAlert, CircleCheck, CircleHelp,
  Crosshair, FileDown, FileText, Filter, FlaskConical,
  Layers, Map, Minus, Moon, Play, Plus,
  RadioTower, RotateCcw, Search, Settings,
  Siren, TriangleAlert,
  type LucideProps, type LucideIcon,
} from 'lucide-react'

const iconMap: Record<string, LucideIcon> = {
  'activity':       Activity,
  'arrow-down':     ArrowDown,
  'arrow-up':       ArrowUp,
  'arrow-up-right': ArrowUpRight,
  'bell':           Bell,
  'calendar':       Calendar,
  'circle-alert':   CircleAlert,
  'circle-check':   CircleCheck,
  'circle-help':    CircleHelp,
  'crosshair':      Crosshair,
  'file-down':      FileDown,
  'file-text':      FileText,
  'filter':         Filter,
  'flask-conical':  FlaskConical,
  'layers':         Layers,
  'map':            Map,
  'minus':          Minus,
  'moon':           Moon,
  'play':           Play,
  'plus':           Plus,
  'radio-tower':    RadioTower,
  'rotate-ccw':     RotateCcw,
  'search':         Search,
  'settings':       Settings,
  'siren':          Siren,
  'triangle-alert': TriangleAlert,
}

interface Props extends LucideProps {
  name: string
}

export function Icon({ name, size = 16, ...props }: Props) {
  const Component = iconMap[name]
  if (!Component) return null
  return <Component size={size} {...props} />
}
