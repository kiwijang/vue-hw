import './assets/main.scss'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { VueReCaptcha } from 'vue-recaptcha-v3'

import App from './App.vue'
import router from './router'

const app = createApp(App)

app.use(createPinia())
app.use(router)
app.use(VueReCaptcha, {
  siteKey: '6LcdrxooAAAAABpR6EJ4pQnp8r4J6E-mm0weKKLU',
  loaderOptions: {
    useRecaptchaNet: true
  }
})

app.mount('#app')
