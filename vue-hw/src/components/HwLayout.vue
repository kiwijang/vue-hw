<template>
  <div class="header">
    <HwHeader></HwHeader>
  </div>
  <div class="wrap">
    <div class="content-layout max-w-1400">
      <slot name="content"></slot>
    </div>
  </div>
</template>

<script setup lang="ts">
import HwHeader from '../components/HwHeader.vue';
</script>

<style lang="scss" scoped>
.wrap {
  position: relative;
  display: flex;
  justify-content: center;
  min-height: $content-height;
}
.header {
  margin: auto;
  position: sticky;
  top: 0;
  z-index: 1;
}
</style>
