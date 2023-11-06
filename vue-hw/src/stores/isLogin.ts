import { ref } from 'vue'
import { defineStore } from 'pinia'
import { checkJwtIsLogin } from '@/utils'


export const useLoginStore = defineStore('isLogin', () => {
  const isLogin = ref(false)

  const checkIsLogin = () => {    
    isLogin.value = checkJwtIsLogin();
  }

  return { checkIsLogin, isLogin }
})
