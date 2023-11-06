<template>
  <div class="table-responsive">
    <table
      class="table table-striped"
      :style="{ 'min-width': minWidth + 'px' }"
    >
      <thead>
        <tr>
          <th></th>
          <th v-for="(row, i) in tableHeader" :key="i">
            {{ row }}
          </th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(row, i) in tableData" :key="i">
          <td>
            <button
              type="button"
              class="btn btn-inverse-primary btn-rounded btn-icon"
              @click="edit(i)"
            >
              <i class="mdi mdi-pencil"></i>
            </button>
          </td>
          <template v-for="(val, key, index) in row" :key="index">
            <td v-if="key === 'name'" class="max-w-250">{{ val }}</td>
            <td v-else-if="key === 'joinUsers'">
              <span class="join-users" v-for="(user, i) in val" :key="i">{{ user.userName }}</span>
            </td>
            <td
              v-else-if="key !== 'id' && key !== 'name' && key !== 'joinUsers'"
            >
              {{ val }}
            </td>
            <input v-else type="hidden" :data-attr="val" :id="'qazwsx' + i" />
          </template>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
export default {
  props: {
    tableData: {
      type: Array,
      required: true,
    },
    tableHeader: {
      type: Array,
      required: true,
    },
    minWidth: {
      type: Number,
    },
  },
  setup(_, ctx) {
    const edit = (index: number) => {
      const val = document
        .getElementById('qazwsx' + index)
        ?.getAttribute('data-attr');
      ctx.emit('open-event', val);
    };

    return {
      edit,
    };
  },
};
</script>

<style lang="scss">
.modal {
  background-color: #fff;
  width: 100%;
  height: 100%;
}
.max-w-250 {
  max-width: 150px;
}
.join-users {
  background-color: darkgoldenrod;
  color: white;
  padding: 4px 8px;
  margin-right: 4px;
  border-radius: 20px;
}
</style>
