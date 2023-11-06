<template>
  <div class="re-wrap mt-10">
    <div>
      <h1 class="text-center">註冊</h1>
    </div>
    <div class="card">
      <div class="card-body">
        <form class="forms-sample" ref="form">
          <!-- Email -->
          <div class="form-group">
            <label for="exampleInputEmail3">Email(不可重複)</label>
            <input
              type="email"
              class="form-control"
              v-model="viewModel.email"
              placeholder="Email"
              name="email"
            />
          </div>
          <!-- 姓名 -->
          <div class="form-group">
            <label for="exampleInputName1">姓名</label>
            <input
              type="text"
              class="form-control"
              v-model="viewModel.name"
              placeholder="姓名"
              name="name"
            />
          </div>
          <!-- 手機 -->
          <div class="form-group">
            <label for="exampleInputCity1">手機</label>
            <input
              type="text"
              class="form-control"
              v-model="viewModel.phone"
              placeholder="手機"
              name="phone"
            />
          </div>
          <!-- 密碼 -->
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
          <div class="err-wrap">
            <div class="err" v-for="(e, index) in errors" :key="index">
              {{ e.description }}
            </div>
          </div>
          <button
            type="submit"
            class="btn btn-primary mr-2"
            @click.prevent="submit()"
          >
            送出
          </button>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { recaptchaExec, configuration, auth_api } from '@/utils';
import { AuthApi, type RegisterVM } from '@/api';
import { onMounted, onUnmounted, ref } from 'vue';
import { useReCaptcha } from 'vue-recaptcha-v3';
import router from "@/router";

const recaptchaInstance = useReCaptcha();
const viewModel = {
  email: "", name: "", phone: "", pwd: "", recaptchaToken:""
};
let errors: any = ref([]);
let form: HTMLFormElement;

onMounted(() => {
  recaptchaInstance?.instance.value?.showBadge();
});

const submit = async () => {
  await getToken();

  const model: RegisterVM = viewModel;
  const sub = auth_api.apiAuthRegisterPost({ registerVM: model })
    .subscribe({
      next: async () => {
        reset();
        redirectToLogin();
        alert('註冊成功!');
      },
      error: async (err) => {
        errors.value = (await err).response;
      }
    });

  onUnmounted(() => {
    sub.unsubscribe();
  });

  const redirectToLogin = () => {
    router.push('/login');
  }
}

const reset = () => {
  viewModel.email = "";
  viewModel.name = "";
  viewModel.phone = "";
  viewModel.pwd = "";
  viewModel.recaptchaToken = "";
  errors.value = [];
  form.reset();
}

const getToken = async () => {
    viewModel.recaptchaToken = await recaptchaExec('register', recaptchaInstance) ?? "";
}

onUnmounted(() => {
  recaptchaInstance?.instance.value?.hideBadge();
});
</script>

<style lang="scss" scope>
@media (min-width: 1024px) {
  .word {
    display: flex;
    align-items: center;
  }
}
.re-wrap {
  margin: 20px auto;
}
:deep .grecaptcha-badge {
  display: block !important;
  visibility: visible !important;
}
.err-wrap {
  margin-bottom: 24px;
}
.err {
  color: #d13636;
  font-size: 12px;
  line-height: 1.5;
}
.card {
  @include breakpoint(sm) {
    min-width: 460px;
  }
}
</style>
