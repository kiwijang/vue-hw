<template>
  <h1 class="mb-title">活動清單</h1>
  <div class="card">
    <div class="card-body">
      <!-- search -->
      <div class="form-group">
        <label>活動名稱</label>
        <input
          type="text"
          class="form-control"
          id="exampleInputName1"
          placeholder="Name"
        />
      </div>
      <div class="form-group">
        <label>活動起日</label>
        <input type="date" class="date form-control mb-3" name="birthday" />
        <label>活動迄日</label>
        <input type="date" class="date form-control" name="birthday" />
      </div>
      <button type="button" class="btn btn-primary mr-2">查詢</button>

      <hr />

      <button type="button" class="btn btn-warning" @click="toggleAddModel()">
        <span class="mdi mdi-alarm-plus"></span> 新增活動
      </button>

      <HwTable
        :table-data="tableData"
        :table-header="[
          '名稱',
          '起日',
          '迄日',
          '地點',
          '費用',
          '人數',
          '參與人員',
        ]"
        :min-width="1200"
        @open-event="getOpen"
      >
      </HwTable>

      <!-- edit modal -->
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
            <h2>修改活動資料</h2>
            <hr />
            <form class="forms-sample">
              <div class="form-group">
                <label for="exampleInputEmail3">名稱</label>
                <input
                  type="text"
                  class="form-control"
                  placeholder="name"
                  v-model="updViewModel.name"
                />
              </div>

              <div class="form-group">
                <label for="exampleInputBirth">起日</label>
                <div>
                  <input
                    type="date"
                    class="date form-control"
                    name="birthday"
                    v-model="updViewModel.startTime"
                  />
                </div>
              </div>

              <div class="form-group">
                <label for="exampleInputBirth">迄日</label>
                <div>
                  <input
                    type="date"
                    class="date form-control"
                    name="birthday"
                    v-model="updViewModel.endTime"
                  />
                </div>
              </div>

              <div class="form-group">
                <label for="exampleInputCity1">地址</label>
                <input
                  type="text"
                  class="form-control"
                  v-model="updViewModel.address"
                  placeholder="通訊地址"
                />
              </div>

              <div class="form-group">
                <label for="exampleInputCity1">費用</label>
                <input
                  type="text"
                  class="form-control"
                  v-model="updViewModel.cost"
                  placeholder="費用"
                />
              </div>

              <div class="form-group">
                <label>參與人員</label>
                <template
                  v-for="(user, i) in eventjoinusersViews"
                  :key="user.userId"
                >
                  <div class="form-check form-check-primary">
                    <label class="form-check-label">
                      <input
                        type="checkbox"
                        class="form-check-input"
                        :value="user.userId"
                        @change="addToJoinUsers($event, user)"
                        :checked="isIdInArray(user.userId)"
                      />
                      {{ user.userName }}<i class="input-helper"></i>
                    </label>
                  </div>
                </template>
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

      <!-- add modal -->
      <div
        class="backdrop"
        id="backdrop"
        :class="{ hide: !isAddOpen }"
        @click="toggleAddModel()"
      ></div>
      <Teleport to="body">
        <div class="hw-modal" v-if="isAddOpen">
          <div class="modal-top">
            <button
              class="btn btn-light btn-rounded btn-fw mdi mdi-window-close close"
              @click="toggleAddModel()"
            ></button>
          </div>
          <div>
            <h2>新增活動資料</h2>
            <hr />
            <form class="forms-sample">
              <div class="form-group">
                <label for="exampleInputEmail3">名稱</label>
                <input
                  type="text"
                  class="form-control"
                  placeholder="name"
                  v-model="addViewModel.name"
                />
              </div>

              <div class="form-group">
                <label for="exampleInputBirth">起日</label>
                <div>
                  <input
                    type="date"
                    class="date form-control"
                    name="birthday"
                    v-model="addViewModel.startTime"
                  />
                </div>
              </div>

              <div class="form-group">
                <label for="exampleInputBirth">迄日</label>
                <div>
                  <input
                    type="date"
                    class="date form-control"
                    name="birthday"
                    v-model="addViewModel.endTime"
                  />
                </div>
              </div>

              <div class="form-group">
                <label for="exampleInputCity1">地址</label>
                <input
                  type="text"
                  class="form-control"
                  v-model="addViewModel.address"
                  placeholder="通訊地址"
                />
              </div>

              <div class="form-group">
                <label for="exampleInputCity1">費用</label>
                <input
                  type="text"
                  class="form-control"
                  v-model="addViewModel.cost"
                  placeholder="費用"
                />
              </div>

              <div class="form-group">
                <label>參與人員</label>
                <template
                  v-for="(user, i) in eventjoinusersViews"
                  :key="user.userId"
                >
                  <div class="form-check form-check-primary">
                    <label class="form-check-label">
                      <input
                        type="checkbox"
                        class="form-check-input"
                        :value="user.userId"
                        @change="addToAddJoinUsers($event, user)"
                        :checked="isIdInArray(user.userId)"
                      />
                      {{ user.userName }}<i class="input-helper"></i>
                    </label>
                  </div>
                </template>
                {{addViewModel.eventjoinusersName}}
              </div>

              <button
                type="submit"
                class="btn btn-primary mr-2"
                @click.prevent="submitAdd()"
              >
                確認新增
              </button>
              <button
                class="btn btn-inverse-secondary btn-fw"
                @click="toggleAddModel()"
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
import { EventApi, MemberApi, type EventVM } from '@/api';
import HwTable from '@/components/HwTable.vue';
import router from '@/router';
import { useOpenStore } from '@/stores/isOpen';
import { event_api, getJWTfromBrowser, invailidJwtRelaodPage, member_api } from '@/utils';
import type { Observable, Subscription } from 'rxjs';
import { catchError, switchMapTo, tap } from 'rxjs/operators';
import { onMounted, ref, type Ref, reactive } from 'vue';
import { useReCaptcha } from 'vue-recaptcha-v3';


//// 新增
let isAddOpen = ref(false);
let addViewModel = ref({
  name: '',
  startTime: '',
  endTime: '',
  address: '',
  cost: 0,
  eventjoinusersName: []
});
let eventJoinuUsersViews_Add: any = ref([]);

const toggleAddModel = () => {
  isAddOpen.value = !isAddOpen.value;
  console.log(isAddOpen.value)
};

const addToAddJoinUsers = (evt: { target: { checked: any; }; }, user: any) => {
  eventJoinuUsersViews_Add.value = updateEventJoinuUsersViews(eventJoinuUsersViews_Add.value, evt.target.checked, user);
  console.log(eventJoinuUsersViews_Add.value)
}

const getEventCheckbox = () => {
  const s = api_m.apiMemberGetAllMembersPost()
    .pipe(
      tap((members: any[]) => {
        // 產生 checkbox
        if (members.length > 0) {
          eventjoinusersViews = members?.map((y: { id: any; chiName: any; }) => {
            return {
              userId: y.id,
              userName: y.chiName,
            }
          });
        }
      }),
      catchError(async (err) => invailidJwtRelaodPage(err.status)),
    )
    .subscribe();
  sub.push(s);
}


const submitAdd = () => {
  // console.log(viewModel.value.birth);
  console.log(addViewModel.value.startTime)
  console.log(new Date(addViewModel.value.startTime).toLocaleDateString())
  const addEventVM: EventVM = {
    name: addViewModel.value.name,
    startTime: addViewModel.value.startTime ? new Date(addViewModel.value.startTime).toLocaleDateString() : "",
    endTime: addViewModel.value.endTime ? new Date(addViewModel.value.endTime).toLocaleDateString() : "",
    address: addViewModel.value.address,
    cost: addViewModel.value.cost,
    eventjoinusersName: eventJoinuUsersViews_Add.value
  };
  const s = api.apiEventCreateEventPost({ eventVM: addEventVM  })
    .pipe(catchError(async (err) => invailidJwtRelaodPage(err.status))).subscribe((x: any) => {
    alert('新增成功!');

    const s = setAllMemberTable();
    sub.push(s);
  });
};
////

const openStore = useOpenStore();
let eventJoinuUsersViews: any = ref([]);

const addToJoinUsers = (evt: { target: { checked: any; }; }, user: any) => {
  updateEventJoinuUsersViews(eventJoinuUsersViews.value, evt.target.checked, user);
}

const updateEventJoinuUsersViews = (arr: any[], checked: boolean, user: any) => {
  if (checked) {
    arr.push(user);
  } else {
    const index = arr.findIndex((x: any) => x.userId === user.userId);
    if (index !== -1) {
      arr.splice(index, 1);
    }
  }
  return arr;
};

const isIdInArray: (userId: number) => boolean = (userId: number) => {
  console.log('hi')
  return updViewModel.value.eventjoinusersName.findIndex((x: { userId: number }) => x.userId === userId) !== -1;
}

const toggleModel = () => {
  openStore.toggleIsOpen();
  eventJoinuUsersViews.value = [];
};

let myDataId = ref('');
const getEvent$ = (dataId: string) => api.apiEventGetEventByIdPost({ id: dataId });
const getOpen = (dataId: string) => {
  myDataId.value = dataId;
  console.log(dataId)
  openStore.toggleIsOpen();

  // 取得 event 欄位資料並更新到畫面
  getEvent(setUpdateModel);
};

const getEvent = (cb: Function) => {
  const s = api_m.apiMemberGetAllMembersPost()
    .pipe(
      tap((members: any[]) => {
        // 產生 checkbox
        if (members.length > 0) {
          eventjoinusersViews = members?.map((y: { id: any; chiName: any; }) => {
            return {
              userId: y.id,
              userName: y.chiName,
            }
          });
        }
      }),
      catchError(async (err) => invailidJwtRelaodPage(err.status)),
      // 取得目前活動資訊
      switchMapTo(getEvent$(myDataId.value)),
    )
    .subscribe((x: EventVM) => {
      cb(x);
    });
  sub.push(s);
}

const setUpdateModel = (y: EventVM) => {
    updViewModel.value = {
      name: y.name ?? '',
      startTime: y.startTime ? y.startTime.split('T')[0] : "",
      endTime: y.endTime ? y.endTime.split('T')[0] : "",
      address: y.address ?? '',
      cost: y.cost ?? 0,
      eventjoinusersName: y.eventjoinusersName as any
    }
};

const recaptchaInstance = useReCaptcha();
const encodedJwt = ref(getJWTfromBrowser());
let sub: Subscription[] = [];
let tableData: Ref<EventVM[]> = ref([]);
let api: EventApi;
let getAllEvent$: Observable<EventVM> | any;
let api_m: MemberApi;
let eventjoinusersViews: any = reactive([]);

if (encodedJwt.value) {
  api = event_api(encodedJwt.value);
  getAllEvent$ = event_api(encodedJwt.value).apiEventGetAllEventsPost().pipe(catchError(async (err) => invailidJwtRelaodPage(err.status)));
  api_m = member_api(encodedJwt.value);
} else {
  router.push('/login');
}

const setAllEventTable = () => {
    return getAllEvent$
      .subscribe({
        next: (x: any[]) => {
        if (x) {
          tableData.value = x.map((y: { id: any; name: any; startTime: string | number | Date; endTime: string | number | Date; address: any; cost: any; eventjoinusersName: string | any[]; }) => {
            return {
              id: y.id,
              name: y.name,
              startTime: y.startTime ? new Date(y.startTime).toLocaleDateString() : "",
              endTime: y.endTime ? new Date(y.endTime).toLocaleDateString() : "",
              address: y.address,
              cost: y.cost,
              count: y.eventjoinusersName?.length,
              joinUsers: y.eventjoinusersName
            }
          });
        }
      },
      error: async (err: any) => {},
    })
};


onMounted(() => {
  recaptchaInstance?.instance.value?.hideBadge();

  const s = setAllEventTable();
  sub.push(s);

  // 生成新增頁面的 checkbox
  getEventCheckbox();
});

let updViewModel = ref({
  name: '',
  startTime: '',
  endTime: '',
  address: '',
  cost: 0,
  eventjoinusersName: []
});


const submit = () => {
  // console.log(viewModel.value.birth);
  const updateEventVM: EventVM = {
    id: myDataId.value,
    name: updViewModel.value.name,
    startTime: updViewModel.value.startTime ? new Date(updViewModel.value.startTime).toLocaleDateString() : "",
    endTime: updViewModel.value.endTime ? new Date(updViewModel.value.endTime).toLocaleDateString() : "",
    address: updViewModel.value.address,
    cost: updViewModel.value.cost,
    eventjoinusersName: eventJoinuUsersViews.value
  };
  const s = api.apiEventUpdateEventPost({ eventVM: updateEventVM })
    .pipe(catchError(async (err) => invailidJwtRelaodPage(err.status))).subscribe((x: any) => {
    alert('更新成功!');

    const s = setAllMemberTable();
    sub.push(s);
  });
};

const setAllMemberTable = () => {
  return getAllEvent$.subscribe({
    next: (x: any[]) => {
      tableData.value = x.map((y: { id: any; name: any; startTime: string | number | Date; endTime: string | number | Date; address: any; cost: any; eventjoinusersName: string | any[]; }) => {
        return {
          id: y.id,
          name: y.name,
          startTime: y.startTime ? new Date(y.startTime).toLocaleDateString() : "",
          endTime: y.endTime ? new Date(y.endTime).toLocaleDateString() : "",
          address: y.address,
          cost: y.cost,
          count: y.eventjoinusersName?.length,
          joinUsers: y.eventjoinusersName
        }
      });
    },
    error: async (err: any) => {
      const status = (await err).status;
    },
  });
}
</script>

<style lang="scss" scoped>
hr {
  border-top: 2px solid black;
  width: 100%;
  margin: 24px 0;
}
.mb-3 {
  margin-bottom: 12px;
}
.date {
  cursor: pointer;
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
