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
          </tr>
        </thead>
        <tbody v-for="list in lists" :key="list.id" @click="redirectTo(list)">
          <tr>
            <td> {{ list.name }} </td>
            <td> {{ getListStatus(list) }} </td>
            <td> {{ getListPriority(list) }} </td>
            <td> <progress :value="getCompletedTaskCount(list)" :max="list.tasks.length"> </progress> </td> 
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
  },
  methods: {
    async getList() {
      try { this.$store.commit('getList', (await axios.get("list")).data); } 
      catch (e) { toast.error("Error loading API") }
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


