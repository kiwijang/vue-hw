<template>
  <div class="w-full">
    <div class="flex bs-wrap">
      <HwSidebar></HwSidebar>
      <div class="backstage-content">
        <RouterView />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import HwSidebar from '../../components/HwSidebar.vue';
import { RouterView } from 'vue-router';
</script>

<style lang="scss" scoped>
.backstage-content {
  flex: 1;
}
.bs-wrap {
  padding: 40px;
  flex-direction: column;
}
</style>
