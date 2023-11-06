<template>
  <h1 class="mb-title">個人資料</h1>
  <div class="card">
    <div class="card-body">
      <form class="forms-sample">
        <div class="form-group">
          <label for="exampleInputEmail3">Email</label>
          <input
            type="email"
            class="form-control"
            placeholder="Email"
            v-model="viewModel.email"
            readonly
          />
        </div>
        <div class="form-group">
          <label for="exampleInputName1">加入會員時間</label>
          <input
            type="text"
            class="form-control"
            id="exampleInputCreateTime"
            placeholder="加入會員時間"
            v-model="viewModel.createTime"
            readonly
          />
        </div>
        <div class="form-group">
          <label>大頭貼</label>
          <input
            type="file"
            name="img"
            accept="image/*"
            class="file-upload-default"
            @change="getFile($event)"
            ref="browseFileBtn"
          />
          <div class="input-group col-xs-12">
            <input
              type="text"
              class="form-control file-upload-info"
              disabled
              placeholder="上傳圖片"
              v-model="imgSrcPath"
            />
            <span class="input-group-append">
              <button
                class="file-upload-browse btn btn-primary"
                type="button"
                @click="upload()"
              >
                上傳
              </button>
            </span>
          </div>
          <img :src="myImgSrc" class="pic" />
        </div>
        <div class="form-group">
          <label for="exampleInputName1">姓名</label>
          <input
            type="text"
            class="form-control"
            v-model="viewModel.chiName"
            placeholder="中文姓名"
          />
        </div>
        <div class="form-group">
          <label for="exampleInputEngName">英文姓名</label>
          <input
            type="text"
            class="form-control"
            v-model="viewModel.engName"
            placeholder="英文姓名"
          />
        </div>
        <div class="form-group">
          <label for="exampleInputCity1">手機</label>
          <input
            type="text"
            class="form-control"
            v-model="viewModel.phone"
            placeholder="手機"
          />
        </div>

        <div class="form-group">
          <label for="exampleSelectGender">性別</label>
          <div class="row">
            <div class="col-sm-4">
              <div class="form-check">
                <label class="form-check-label">
                  <input
                    type="radio"
                    class="form-check-input"
                    name="exampleSelectGender"
                    v-model="viewModel.gender"
                    value="male"
                    checked
                  />
                  男士<i class="input-helper"></i>
                </label>
              </div>
            </div>
            <div class="col-sm-5">
              <div class="form-check">
                <label class="form-check-label">
                  <input
                    type="radio"
                    class="form-check-input"
                    name="exampleSelectGender"
                    v-model="viewModel.gender"
                    value="female"
                  />
                  女士<i class="input-helper"></i>
                </label>
              </div>
            </div>
          </div>
        </div>

        <div class="form-group">
          <label for="exampleInputBirth">生日</label>
          <div>
            <input
              type="date"
              class="date form-control"
              name="birthday"
              v-model="viewModel.birth"
            />
          </div>
        </div>
        <div class="form-group">
          <label for="exampleInputCity1">地址</label>
          <input
            type="text"
            class="form-control"
            v-model="viewModel.address"
            placeholder="地址"
          />
        </div>

        <div class="form-group">
          <label for="exampleInputSub">是否訂閱電子報</label>
          <div class="row">
            <div class="col-sm-4">
              <div class="form-check">
                <label class="form-check-label">
                  <input
                    type="radio"
                    class="form-check-input"
                    name="exampleInputSub"
                    v-model="viewModel.isSubscribe"
                    :value="true"
                    checked
                  />
                  是，訂閱
                  <i class="input-helper"></i>
                </label>
              </div>
            </div>
            <div class="col-sm-5">
              <div class="form-check">
                <label class="form-check-label">
                  <input
                    type="radio"
                    class="form-check-input"
                    name="exampleInputSub"
                    v-model="viewModel.isSubscribe"
                    :value="false"
                  />
                  否
                  <i class="input-helper"></i>
                </label>
              </div>
            </div>
          </div>
        </div>
        <button
          type="submit"
          class="btn btn-primary mr-2"
          @click.prevent="submit()"
        >
          確認修改
        </button>
        <button class="btn btn-inverse-secondary btn-fw">取消</button>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { UserApi, type UpdateUserVM } from '@/api';
import { configurationJWT, getJWTfromBrowser, invailidJwtRelaodPage, user_api } from '@/utils';
import { onMounted, onUnmounted, ref } from 'vue';
import jwtDecode from 'jwt-decode';
import type { Subscription } from 'rxjs';
import { useReCaptcha } from 'vue-recaptcha-v3';
import router from '@/router';
import { catchError } from 'rxjs/operators';

let sub: Subscription[] = [];

let viewModel = ref({
  email: '',
  createTime: '',
  img: null as any,
  chiName: '',
  engName: '',
  phone: '',
  gender: 'male',
  birth: null as any,
  address: '',
  isSubscribe: false,
});
let browseFileBtn: HTMLInputElement;
let imgSrcPath = ref('');
let myImgSrc = ref('');

const encodedJwt = ref(getJWTfromBrowser());
let email = '';
if (encodedJwt.value) {
  const jwt: any = jwtDecode(encodedJwt.value);
  email = jwt?.sub;
}

let api: { apiUserGetUserPost: (arg0: { email: string; }) => { (): any; new(): any; subscribe: { (arg0: { next: (x: any) => void; error: (err: any) => Promise<void>; }): any; new(): any; }; }; apiUserUpdateUserPost: (arg0: { updateUserVM: UpdateUserVM; }) => { (): any; new(): any; subscribe: { (arg0: () => void): any; new(): any; }; }; };
if (encodedJwt.value) {
  api = user_api(encodedJwt.value);
} else {
  router.push('/login');
}

const recaptchaInstance = useReCaptcha();
onMounted(() => {
  recaptchaInstance?.instance.value?.hideBadge();
  const s = api
    .apiUserGetUserPost({ email })    
    .pipe(catchError(async (err) => invailidJwtRelaodPage(err.status)))
    .subscribe({
      next: (x) => {
        viewModel.value = {
          createTime: x.createTime ? new Date(x.createTime).toLocaleString() : '',
          email: x.email ?? '',
          img: x.img ?? '',
          chiName: x.chiName ?? '',
          engName: x.engName ?? '',
          phone: x.phone ?? '',
          gender: x.gender ?? '',
          birth: x.birthDay ? x.birthDay.split('T')[0] : null,
          address: x.address ?? '',
          isSubscribe: x.isSubscribe ?? false,
        };

        myImgSrc.value = x.img ?? '';
      },
      error: async (err) => {const status = (await err).status;}
    });
  sub.push(s);
});

const submit = () => {
  console.log(viewModel.value.birth)
  const updateUserVM: UpdateUserVM = {
    email: viewModel.value.email,
    img: viewModel.value.img,
    chiName: viewModel.value.chiName,
    engName: viewModel.value.engName,
    phone: viewModel.value.phone,
    gender: viewModel.value.gender,
    birth: new Date(viewModel.value.birth).toLocaleDateString(),
    address: viewModel.value.address,
    isSubscribe: viewModel.value.isSubscribe,
  };
  const s = api.apiUserUpdateUserPost({ updateUserVM })
    .pipe(catchError(async (err) => invailidJwtRelaodPage(err.status)))
    .subscribe(() => {
    alert('更新成功!');
  });
  sub.push(s);
};

const upload = () => {
  browseFileBtn.click();
}

const getFile = (e: Event) => {
  const img = (e.target as any).files[0];

  // 25600 bytes (25kb)
  if (img.size <= 25600) {
    const reader = new FileReader();

    reader.onload = function (event: any) {
      const arrayBuffer = event.target.result;
      const uint8Array = new Uint8Array(arrayBuffer);
      const binaryString = String.fromCharCode.apply(null, uint8Array as unknown as number[]);
      const base64String = btoa(binaryString);


      viewModel.value.img = `data:${img.type};base64,` + base64String;
    };

    imgSrcPath.value = img.name;
    myImgSrc.value = URL.createObjectURL(img);

    // 執行這段才會觸發 reader.onload
    reader.readAsArrayBuffer(img);
  } else {
    alert('圖片不能超過 25kb');
  }
}

onUnmounted(() => {
  if (sub.length > 0) {
    sub.forEach((x) => x.unsubscribe());
  }
});
</script>

<style lang="scss" scoped>
.pic {
  max-width: 250px;
  height: 250px;
  object-fit: contain;
  overflow: hidden;
}

input[type='date'] {
  cursor: pointer;
}
.form-check-label {
  cursor: pointer;
}
</style>
