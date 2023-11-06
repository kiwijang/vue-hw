<template>
  <div class="login-wrap mt-10">
    <div>
      <h1 class="text-center">登入</h1>
    </div>
    <div class="card">
      <div class="card-body">
        <form class="forms-sample" autocomplete="true" ref="form">
          <div class="form-group">
            <label for="exampleInputEmail3">Email</label>
            <input
              type="email"
              class="form-control"
              v-model="viewModel.email"
              placeholder="Email"
              name="email"
            />
          </div>
          <div class="form-group">
            <label for="exampleInputPassword4">密碼</label>
            <input
              type="password"
              class="form-control"
              v-model="viewModel.pwd"
              placeholder="密碼"
              name="pwd"
            />
          </div>
          <button
            type="submit"
            class="btn btn-primary mr-2 font-white"
            @click.prevent="submit()"
          >
            登入
          </button>

          <div class="form-check form-check-primary">
            <label class="form-check-label">
              <input
                type="checkbox"
                class="form-check-input"
                v-model="viewModel.rememberMe"
              />
              記住我<i class="input-helper"></i>
            </label>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { AuthApi, type LoginVM } from '@/api';
import router from '@/router';
import { useLoginStore } from '@/stores/isLogin';
import { auth_api, configuration, getBrowserStorage, invailidJwtRelaodPage, recaptchaExec, removeBrowserStorage, setBrowserStorage } from '@/utils';
import type { Subscription } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { onUnmounted, onMounted } from 'vue';
import { useReCaptcha } from 'vue-recaptcha-v3';

let form: HTMLFormElement;
const recaptchaInstance = useReCaptcha();
let sub: Subscription | null;
const viewModel = {
  email: getBrowserStorage("rememberMe")[0] ?? "", pwd: "", rememberMe: !!(getBrowserStorage("rememberMe")[0]) , recaptchaToken:"",
};

const loginStore = useLoginStore();
const toggleLogin = () => {
  // 更新isLogin的值
  loginStore.checkIsLogin();
}

onMounted(() => {
  recaptchaInstance?.instance.value?.showBadge();
});

const getToken = async () => {
  viewModel.recaptchaToken = await recaptchaExec('login', recaptchaInstance) ?? '';
};

const submit = async () => {
  await getToken();

  const model: LoginVM | any = viewModel;
  sub = auth_api
    .apiAuthLoginPost({ loginVM: model })
    .subscribe(async (token: any) => {
      const jwt = (await token)?.jwt;
      if (jwt) {
        setBrowserStorage("jwt", jwt);
      }
      form.reset();
      toggleLogin();
      redirectToHome();
    });

  if (viewModel.rememberMe) {
    setBrowserStorage("rememberMe", viewModel.email);
  } else {
    removeBrowserStorage("rememberMe");
  }

  const redirectToHome = () => {
    router.push('/');
  }
};

onUnmounted(() => {
  recaptchaInstance?.instance.value?.hideBadge();

  if (sub) {
    sub.unsubscribe();
  }
});
</script>

<style scoped lang="scss">
.login-wrap {
  margin-left: auto;
  margin-right: auto;
}
:deep .grecaptcha-badge {
  display: block !important;
  visibility: visible !important;
}
.card {
  @include breakpoint(sm) {
    min-width: 460px;
  }
}
</style>
