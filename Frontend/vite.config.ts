import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'

export default defineConfig({
  plugins: [react(), tailwindcss()],
  server: {
    host: true,
    allowedHosts: true,
    open: '/landingpage'
  },
  build: {
    rollupOptions: {
      output: {
        // Splits mapbox-gl into a separate chunk so it is cached independently
        // of app code — users only re-download it when mapbox-gl itself updates.
        manualChunks: {
          mapbox: ['mapbox-gl'],
        },
      },
    },
  },
})
