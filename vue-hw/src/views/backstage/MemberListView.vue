<template>
  <h1 class="mb-title">會員清單</h1>

  <div class="card">
    <div class="card-body">
      <!-- search -->
      <div class="form-group">
        <label for="exampleInputName1">姓名</label>
        <input
          type="text"
          class="form-control"
          id="exampleInputName1"
          placeholder="Name"
        />
      </div>

      <div class="form-group">
        <label for="exampleInputName1">出生年份</label>
        <input
          type="text"
          class="form-control"
          id="exampleInputName1"
          placeholder="Name"
        />
      </div>
      <button type="button" class="btn btn-primary mr-2">查詢</button>

      <hr />

      <HwTable
        :table-data="tableData"
        :table-header="['姓名', '年紀', '生日', '手機', 'E-mail']"
        :min-width="650"
        @open-event="getOpen"
      ></HwTable>

      <!-- modal -->
      <div
        class="backdrop"
        id="backdrop"
        :class="{ hide: !openStore.isOpen }"
        @click="toggleModel()"
      ></div>
      <Teleport to="body">
        <div class="hw-modal" v-if="openStore.isOpen">
          <div class="modal-top">
            <button
              class="btn btn-light btn-rounded btn-fw mdi mdi-window-close close"
              @click="toggleModel()"
            ></button>
          </div>
          <div>
            <h2>修改會員資料</h2>
            <hr />
            <form class="forms-sample">
              <div class="form-group">
                <label for="exampleInputEmail3">姓名</label>
                <input
                  type="text"
                  class="form-control"
                  placeholder="name"
                  v-model="updViewModel.name"
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
                          v-model="updViewModel.gender"
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
                          v-model="updViewModel.gender"
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
                    v-model="updViewModel.birth"
                  />
                </div>
              </div>

              <div class="form-group">
                <label for="exampleInputEmail3">身分證字號</label>
                <input
                  type="text"
                  class="form-control"
                  placeholder="idNumber"
                  v-model="updViewModel.idNumber"
                />
              </div>

              <div class="form-group">
                <label for="exampleInputEmail3">Email</label>
                <input
                  type="email"
                  class="form-control"
                  placeholder="Email"
                  v-model="updViewModel.email"
                  readonly
                />
              </div>

              <div class="form-group">
                <label for="exampleInputCity1">手機</label>
                <input
                  type="text"
                  class="form-control"
                  v-model="updViewModel.phone"
                  placeholder="手機"
                />
              </div>

              <div class="form-group">
                <label for="exampleInputCity1">通訊地址</label>
                <input
                  type="text"
                  class="form-control"
                  v-model="updViewModel.address"
                  placeholder="通訊地址"
                />
              </div>

              <div class="form-group">
                <label for="exampleInputCity1">學校</label>
                <input
                  type="text"
                  class="form-control"
                  v-model="updViewModel.school"
                  placeholder="學校"
                />
              </div>

              <div class="form-group">
                <label for="exampleInputCity1">科系</label>
                <input
                  type="text"
                  class="form-control"
                  v-model="updViewModel.department"
                  placeholder="科系"
                />
              </div>

              <button
                type="submit"
                class="btn btn-primary mr-2"
                @click.prevent="submit()"
              >
                確認修改
              </button>
              <button
                class="btn btn-inverse-secondary btn-fw"
                @click="toggleModel()"
              >
                取消
              </button>
            </form>
          </div>
        </div>
      </Teleport>
    </div>
  </div>
</template>

<script setup lang="ts">
import { AuthApi, MemberApi, type MemberVM } from '@/api';
import HwTable from '@/components/HwTable.vue';
import router from '@/router';
import { useLoginStore } from '@/stores/isLogin';
import { useOpenStore } from '@/stores/isOpen';
import { configuration, configurationJWT, getJWTfromBrowser, invailidJwtRelaodPage, member_api, removeBrowserStorage } from '@/utils';
import type { Subscription } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { onMounted, onUnmounted, ref, type Ref } from 'vue';
import { useReCaptcha } from 'vue-recaptcha-v3';

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

const loginStore = useLoginStore();

let sub: Subscription[] = [];
let tableData: Ref<MemberVM[]> = ref([]);
let updViewModel = ref({
  name: '',
  gender: 'male',
  birth: '',
  idNumber: '',
  email: '',
  phone: '',
  address: '',
  school: '',
  department: '',
});

let api: { apiMemberGetMemberByUserIdPost: (arg0: { id: string; }) => any; apiMemberGetAllMembersPost: () => any; apiMemberUpdateMemberPost: (arg0: { memberVM: MemberVM; }) => { (): any; new(): any; subscribe: { (arg0: (x: any) => void): any; new(): any; }; }; };
const encodedJwt = ref(getJWTfromBrowser());
if (encodedJwt.value) {
  api = member_api(encodedJwt.value);
} else {
  router.push('/login');
}


const openStore = useOpenStore();

const toggleModel = () => {
  openStore.toggleIsOpen();
};

let myDataId = ref('');
const getMember$ = (dataId: string) => api.apiMemberGetMemberByUserIdPost({ id: dataId});

const getOpen = (dataId: string) => {
  myDataId.value = dataId;
  console.log(dataId)
  openStore.toggleIsOpen();

  getMember();
};


const getMember = () => {
  const s = getMember$(myDataId.value)
    .pipe(catchError(async (err) => invailidJwtRelaodPage(err.status)))
    .subscribe((x: MemberVM) => {
    setUpdateModel(x);
  });
  sub.push(s);
}

const setUpdateModel = (x: MemberVM) => {
    updViewModel.value = {
      name: x.chiName ?? '',
      gender: x.gender ?? 'male',
      birth: x.birth ? x.birth.split('T')[0] : new Date().toLocaleDateString(),
      idNumber: x.idNumber ?? '',
      email: x.email ?? '',
      phone: x.phone ?? '',
      address: x.address ?? '',
      school: x.school ?? '',
      department: x.department ?? '',
    }
};

const recaptchaInstance = useReCaptcha();

onMounted(() => {
  recaptchaInstance?.instance.value?.hideBadge();

  const s = setAllMemberTable();
  sub.push(s);
});

const setAllMemberTable = () => {
  return api.apiMemberGetAllMembersPost()
    .pipe(catchError(async (err) => invailidJwtRelaodPage(err.status)))
    .subscribe({
    next: (x: any[]) => {
      tableData.value = x.map((y: { id: any; chiName: any; birth: string | number | Date; phone: any; email: any; }) => {
        return {
          id: y.id,
          name: y.chiName,
          age: y.birth ? new Date().getFullYear() - new Date(y.birth).getFullYear() : '',
          birth: y.birth ? new Date(y.birth).toLocaleDateString() : '',
          phone: y.phone,
          email: y.email
        }
      });
    },
    error: async (err: any) => {},
  });
}


const submit = () => {
  // console.log(viewModel.value.birth);
  const updateMemberVM: MemberVM = {
    id: Number(myDataId.value),
    chiName: updViewModel.value.name ?? '',
    gender: updViewModel.value.gender ?? 'male',
    birth: updViewModel.value.birth,
    idNumber: updViewModel.value.idNumber ?? '',
    email: updViewModel.value.email ?? '',
    phone: updViewModel.value.phone ?? '',
    address: updViewModel.value.address ?? '',
    school: updViewModel.value.school ?? '',
    department: updViewModel.value.department ?? '',
  };
  const s = api.apiMemberUpdateMemberPost({ memberVM: updateMemberVM })
    .pipe(catchError(async (err) => invailidJwtRelaodPage(err.status)))
    .subscribe((x) => {
    alert('更新成功!');

    const s = setAllMemberTable();
    sub.push(s);
  });
  sub.push(s);
};

onUnmounted(() => {
  if (sub.length > 0) {
    sub.forEach((x) => x.unsubscribe());
  }
});
</script>

<style lang="scss" scoped>
hr {
  border-top: 2px solid black;
  width: 100%;
  margin: 24px 0;
}

///// modal /////////////
.hw-modal {
  background-color: #fff;
  width: 100%;
  margin: 20px;
  display: block;
  position: absolute;
  top: 80px;
  left: 0;
  width: calc(100% - 40px);
  height: calc(100% - 150px);
  overflow-y: auto;
  padding: 24px 40px;
  border-radius: 20px;
  @include box-shadow;
}
.backdrop {
  background-color: black;
  opacity: 0.5;
  position: fixed;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  display: flex;
  justify-content: center;
  align-items: center;
}
.hide {
  display: none;
}
.close {
  color: black;
  font-size: 24px;
  width: 50px;
  height: 50px;
  &:hover {
    color: black;
  }
}

.modal-top {
  display: flex;
  justify-content: flex-end;
}
///////////////////////////
</style>
