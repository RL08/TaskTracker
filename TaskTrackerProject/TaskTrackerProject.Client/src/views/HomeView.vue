<script setup>
import SideBar from "../components/SideBar.vue"
import axios from 'axios';
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
</script>

<template>
  <div class="wrapper">
    <SideBar/>
    <h1>All List</h1>
    <div class="container">
      <table class="table table-bordered">
        <thead>
          <tr>
            <th>List</th>
            <th>Status</th>
            <th>Priority</th>
            <th>Progress</th>
            <th> Options </th>
          </tr>
        </thead>
        <tbody v-for="list in lists" :key="list.id">
          <tr>
            <td v-if="edit && list.guid === this.editListGuid"> 
              <input id="rename" type="text" required placeholder="Rename List" v-model="newName" @keyup.enter=renameList(list)> 
            </td>
            <td v-else @click="redirectTo(list)"> {{ list.name }} </td>
            <td @click="redirectTo(list)"> {{ getListStatus(list) }} </td>
            <td @click="redirectTo(list)"> {{ getListPriority(list) }} </td>
            <td @click="redirectTo(list)"> <progress :value="getCompletedTaskCount(list)" :max="list.tasks.length"> </progress> </td> 
            <td> 
              <font-awesome-icon class="icon" :class="{ 'true': edit }" icon="fa-solid fa-pen" @click="enableEdit(list)"/> 
              <font-awesome-icon class="icon" icon="fa-solid fa-trash" @click="deleteList(list)"/> 
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
export default {
  async mounted() {
    await this.getList();
  },
  computed: {
    lists() { 
      return this.$store.state.user.lists; 
    },
    list() {
      return this.$store.state.user.currentList;
    },
  },
  data() {
    return {
      edit: false,
      editListGuid: null,
      newName: "",
    }
  },
  methods: {
    async getList() {
      try { this.$store.commit('getAllList', (await axios.get("list")).data); } 
      catch (e) { toast.error("Error loading API") }
    },
    async deleteList(list) {
      this.$store.commit('deleteList', list);
      await axios.delete(`list/${list.guid}`, list.guid); 
    },
    redirectTo(list) {
      this.$store.commit('setCurrentListGuid', list.guid);
      this.$router.push(`list/${list.guid}`); 
    },
    getListStatus(list) {
      if (list.tasks.some((task) => task.status === 1)) {
        return "Not Finished";
      } 
      else if (list.tasks.some((task) => task.status === 2)) {
        return "In Progress";
      } 
      else if (list.tasks.some((task) => task.status === 3)) {
        return "Completed";
      }
      else {
        return "No Status";
      }
    },
    getListPriority(list) {
      if (list.tasks.some((task) => task.priority === 1)) {
        return "Low";
      }
      else if (list.tasks.some((task) => task.priority === 2)) {
        return "Medium";
      }
      else if (list.tasks.some((task) => task.priority === 3)) { 
        return "High";
      }
      else {
        return "No Priority";
      }
    },
    getCompletedTaskCount(list) {
      return list.tasks.filter((task) => task.status === 3).length;
    },
    enableEdit(list) {
      this.edit = !this.edit; 
      this.editListGuid = list.guid;
    },
    async renameList(list) {
      list.name = this.newName;
      await axios.put(`list/${list.guid}`, list)
      this.newName = ""; 
      this.edit = false;
    },
  },
};
</script>

<style scoped>
.wrapper {
  overflow-y: auto;
  overflow-x: hidden!important;
  height: 100vh;
}
.container {
  display: flex;
  justify-content: center; 
  margin-left: 200px;
}
h1 {
  color: black;
  margin: 80px 0 10px 200px;
  text-align: center;
}
table {
  background-color: white;
  width: 70vw;
}
table, th, td {
  border:1px solid black;
  border-collapse: collapse;
}
th, td {
  width: 100vh;
  text-align: center;
}
thead {
  background-color: turquoise;
}
tbody {
  cursor: pointer;
}
progress {
  width: 100%;
  height: 20px;
}
tbody {
  cursor: pointer;
}
tbody:hover {
  background-color: lightgrey;
}
input {
  width: 100%;
  border: 0;
}
.icon {
  margin: 0 10px 0 10px;
  padding: 5px;
}
#icon {
  margin: 0 10px 0 0;
}
.icon:hover {
  background-color: lightgrey;
}
.icon.true {
  color: turquoise;
}
@media screen and (max-width: 280px) {  
  .wrapper {
    font-size: 11px;
  }
}
@media screen and (min-height: 1024px) {  
  .wrapper {
    font-size: 24px;
  }
}
@media screen and (max-width: 1000px) {
  .container, 
  h1 {
    margin-left: auto;
  }
  table {
    width: 100vw;
  }
}
</style>