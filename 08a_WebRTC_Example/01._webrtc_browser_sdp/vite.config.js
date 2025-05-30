import { defineConfig } from 'vite'

export default defineConfig({
  server: {
    allowedHosts: [
      'localhost',
      '127.0.0.1',
      'frede.loca.lt',
      // Add your specific domains here
      // 'example.com',
      // 'subdomain.example.com'
    ]
  }
})
