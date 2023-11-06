import { fileURLToPath, URL } from 'node:url'
const path = require('path')

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

const postcssPresetEnv = require('postcss-preset-env')

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
      '~mdi': path.resolve(__dirname, 'node_modules/@mdi/font/css/materialdesignicons.min.css'),
      '@vueuse/integrations/useJwt': path.resolve(__dirname, 'node_modules/jwt-decode/build/jwt-decode.esm.js'),      
    }
  },
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5160', // 后端服务器的 URL
        changeOrigin: true, // 改变请求源，解决跨域问题
        rewrite: (path) => path.replace(/^\/api/, ''), // 重写请求路径，如果后端服务器不需要 '/api' 前缀
      },
    },
  },
  css: {
    postcss: {
      plugins: [
        postcssPresetEnv(),
        require('autoprefixer'),
        require('postcss-import'),
        require('postcss-include'),
        require('postcss-mixins'),
        require('postcss-nested')
      ]
    },
    preprocessorOptions: {
      scss: {
        additionalData: `@import "@/assets/base.scss";` // Import your global SCSS file
      }
    }
  }
})
