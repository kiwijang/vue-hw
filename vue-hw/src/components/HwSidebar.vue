<template>
  <nav class="my-sidebar">
    <ul class="nav">
      <li class="nav-item">
        <router-link to="/user">
          <i class="mdi mdi-account menu-icon"></i>
          <span class="menu-title">個人資料</span>
        </router-link>
      </li>
      <!-- 限管理員 -->
      <li class="nav-item" v-if="isAdmin">
        <router-link to="/memberList">
          <i class="mdi mdi-account-details menu-icon"></i>
          <span class="menu-title">會員清單</span>
        </router-link>
      </li>
      <li class="nav-item" v-if="isAdmin">
        <router-link to="/eventList">
          <i class="mdi mdi-clock menu-icon"></i>
          <span class="menu-title">活動清單</span>
        </router-link>
      </li>
    </ul>
  </nav>
  <hr class="hr" />
</template>

<script setup lang="ts">
import { checkRoleEqual } from '@/utils';

const isAdmin = checkRoleEqual('Admin');
</script>

<style lang="scss" scoped>
.my-sidebar {
  background-color: #fff;
  border-radius: 20px;
  padding: 10px;
  @include breakpoint(xs) {
    display: flex;
    justify-content: center;
  }
  .nav {
    display: flex;
  }
  li {
    display: block;
    padding-right: 10px;
    @include breakpoint(xs) {
      padding-right: 0px;
    }
  }
  ul a {
    font-size: 18px;
    color: #7048ff;
    text-decoration: none;
    padding: 12px 24px;
    display: flex;
    justify-content: center;
    align-items: center;
    transition: 0.2s;
    border-radius: 10px;
    border: 1px solid #7048ff;
    @include breakpoint(xs) {
      padding: 4px 8px;
      margin: 2px;
    }
    &:hover {
      background: #aaa;
    }
  }
}
.hr {
  border: 0px;
  width: 100%;
  margin: 0;
  margin-bottom: 8px;
}
</style>
