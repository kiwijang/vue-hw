<template>
  <header class="max-w-1400">
    <router-link to="/">首頁</router-link>

    <div class="svg-wrap">
      <svg
        width="30px"
        height="30px"
        viewBox="0 0 200 200"
        xmlns="http://www.w3.org/2000/svg"
        version="1.1"
      >
        <polygon
          style="fill: none; stroke: #fff; stroke-width: 2.5px;"
          points="183.138438763306,172 16.8615612366939,172 100,28"
        />
      </svg>
      <svg
        width="30px"
        height="30px"
        viewBox="0 0 200 200"
        xmlns="http://www.w3.org/2000/svg"
        version="1.1"
      >
        <polygon
          style="fill: none; stroke: #fff; stroke-width: 2.5px;"
          points="183.138438763306,172 16.8615612366939,172 100,28"
        />
      </svg>
      <svg
        width="30px"
        height="30px"
        viewBox="0 0 200 200"
        xmlns="http://www.w3.org/2000/svg"
        version="1.1"
      >
        <polygon
          style="fill: none; stroke: #fff; stroke-width: 2.5px;"
          points="183.138438763306,172 16.8615612366939,172 100,28"
        />
      </svg>
    </div>

    <!-- md 以上選單 -->
    <nav class="func-wrap">
      <router-link to="/register" v-if="!isUserLoggedIn">註冊</router-link>
      <router-link to="/login" v-if="!isUserLoggedIn">登入</router-link>
      <div
        v-else
        class="btn btn-secondary btn-rounded btn-fw"
        @click.prevent="logout()"
      >
        登出
      </div>
      <span>
        <router-link to="/backstage" v-if="isUserLoggedIn">後台</router-link>
      </span>
    </nav>

    <!-- xs 選單 -->
    <button class="burger" type="button" @click="burgerClick()">
      <span class="mdi mdi-menu"></span>
    </button>
    <nav
      class="sidebar sidebar-offcanvas"
      tabindex="-1"
      :class="{ active: isActive }"
    >
      <div>
        <router-link to="/register" v-if="!isUserLoggedIn">註冊</router-link>
      </div>
      <div>
        <router-link to="/login" v-if="!isUserLoggedIn">登入</router-link>
        <div v-else class="btn btn-secondary btn-rounded btn-fw">
          登出
        </div>
      </div>
      <div>
        <router-link to="/backstage" v-if="isUserLoggedIn">後台</router-link>
      </div>
      <div @click.prevent="logout()">
        登出
      </div>
    </nav>

    <div
      class="backdrop"
      :class="{ hide: !isActive }"
      @click="burgerClick()"
    ></div>
  </header>
</template>

<script setup lang="ts">
import { AuthApi } from '@/api';
import { useLoginStore } from '@/stores/isLogin';
import { auth_api, configuration, invailidJwtRelaodPage, removeBrowserStorage } from '@/utils';
import type { Subscription } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { computed, onMounted, onUnmounted, ref } from 'vue';
let isActive = ref(false);
const loginStore = useLoginStore();

// 訂閱 isLogin 的值
let isUserLoggedIn = computed(() => loginStore.isLogin);

let sub: Subscription | null;

onMounted(() => {
  loginStore.checkIsLogin();
  console.log(isUserLoggedIn.value);
});

const burgerClick = () => {
  isActive.value = !isActive.value;
};

const logout = () => {
  sub = auth_api
    .apiAuthLogoutPost()
    .subscribe((x) => console.log(x));

  removeBrowserStorage('jwt');
  loginStore.checkIsLogin();
  // 重整網頁
  location.reload();
};

onUnmounted(() => {
  if (sub) {
    sub.unsubscribe();
  }
});
</script>

<style lang="scss" scoped>
.btn {
  cursor: pointer;
}

header {
  height: $navbar-height;
  width: 100%;
  display: flex !important;
  justify-content: space-between;
  align-items: center;
  padding: 0 40px;
  margin: auto;
  border-bottom: 1px solid gray;
  background: linear-gradient(to bottom, #3f3f3f 40%, #383838 150%),
    linear-gradient(
      to top,
      rgba(255, 255, 255, 0.4) 0%,
      rgba(0, 0, 0, 0.25) 200%
    );
  background-blend-mode: multiply;
}
.func-wrap {
  display: flex;
  justify-content: center;
  align-items: center;
  @include breakpoint(xs) {
    display: none;
  }
}
.burger {
  color: white;
  border: 0;
  background: transparent;
  border-radius: 50px;
  transition: 0.5s;
  @include breakpoint(xs) {
    display: inline-block;
  }
  @include breakpoint(sm) {
    display: none;
  }

  &:active {
    background: #aaa;
    transition: 0.5s;
  }
}
.sidebar {
  @include breakpoint(sm) {
    display: none;
  }
}
.hide {
  display: none;
}
.backdrop {
  background-color: black;
  opacity: 0.5;
  position: fixed;
  width: 100%;
  top: $navbar-height;
  left: 0;
  height: $content-height;

  @include breakpoint(sm) {
    display: none;
  }
}

a {
  color: rgb(165, 165, 165);
  font-size: 14px;
  display: flex;
  justify-content: center;
  align-items: center;
  height: 40px;
  border: 1px solid transparent;
  padding: 12px 24px;
  &:hover {
    color: burlywood;
    text-underline-offset: 2px;
    transition: 0.2s;
  }
  &:active {
    border: 1px solid #aaa;
  }
}
svg {
  pointer-events: none;
}
// .svg-wrap {
//   transform: translateY(15px);
// }
</style>
