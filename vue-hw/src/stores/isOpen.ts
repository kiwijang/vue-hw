import { ref } from 'vue'
import { defineStore } from 'pinia'

export const useOpenStore = defineStore('isOpen', () => {
  const isOpen = ref(false)

  const toggleIsOpen = () => {    
    isOpen.value = !isOpen.value;
  }

  return { toggleIsOpen, isOpen }
})
