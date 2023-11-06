<script setup lang="ts">
import { RouterView } from 'vue-router';
import HwLayout from './components/HwLayout.vue';
</script>

<template>
  <HwLayout>
    <template v-slot:content>
      <RouterView />
    </template>
  </HwLayout>
  <div class="footer">
    <footer>
      2023
    </footer>
  </div>
</template>

<style lang="scss" scoped>
.footer {
  footer {
    display: flex;
    justify-content: center;
  }
}
</style>
