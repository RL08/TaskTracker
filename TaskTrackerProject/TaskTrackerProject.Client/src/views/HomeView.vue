<script setup>
import SideBar from "../components/SideBar.vue"
</script>

<template>
  <div class="wrapper">
    <SideBar/>
    <h1>All List</h1>
    <div class="container">
      <table class="table table-striped table-bordered">
        <thead>
          <tr>
            <th>List</th>
            <th>Status</th>
            <th>Priority</th>
            <th>Progress</th>
          </tr>
        </thead>
        <tbody v-for="list in lists" :key="list.id" @click="redirectTo(list.path, list.id)">
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
  computed: {
    lists() {
      return this.$store.state.user.lists;
    },
  },
  methods: {
    redirectTo(path, listId) {
      this.$store.commit('setCurrentListId', listId);
      this.$router.push(path);
    },
    getListStatus(list) {
      if(list.tasks.length === 0) {
        return "No Status";
      }
      else if (list.tasks.some((task) => task.status === "Not Finished")) {
        return "Not Finished";
      } 
      else if (list.tasks.some((task) => task.status === "In Progress")) {
        return "In Progress";
      } 
      else {
        return "Completed";
      }
    },
    getListPriority(list) {
      if(list.tasks.length === 0) {
        return "No Priority";
      }
      else if (list.tasks.some((task) => task.priority === "Low")) {
        return "Low";
      }
      else if (list.tasks.some((task) => task.status === "Mid")) {
        return "Mid";
      }
      else { 
        return "High";
      }
    },
    getCompletedTaskCount(list) {
      return list.tasks.filter((task) => task.status === "Completed").length;
    },
  },
};
</script>

<style scoped>
.wrapper {
  overflow-y: auto;
  height: 100vh;
}
.container {
  display: flex;
  justify-content: center; 
  margin-left: 200px;
}
h1 {
  color: white;
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
@media screen and (max-width: 1250px) {
  .container, 
  h1 {
    margin-left: auto;
  }
}
</style>


