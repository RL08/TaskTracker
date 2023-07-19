<script setup>
import axios from 'axios';
import SideBar from "../components/SideBar.vue"
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
</script>

<template>
  <h1> {{ list.name }} </h1>
  <div class="wrapper">
    <table class="table table-striped table-bordered">
      <thead>
        <tr>
          <th>Task</th>
          <th>Status</th>
          <th>Priority</th>
          <th>Date</th>
        </tr>
      </thead>
      <tbody v-for="task in list.tasks" :key="task.id">
        <tr>
          <td> {{ task.name }} </td>
          <td> {{ task.status }} </td>
          <td> {{ task.priority }} </td>
          <td> {{ task.date }} </td>
        </tr>
      </tbody>
    </table>
    <div class="input-field">
      <input type="text" required placeholder="Add Task" v-model="listModel.task" @keyup.enter=addTask()>
      <div class="icon-box" v-if="this.listModel.task !== ''">
        <font-awesome-icon class="icon" id="icon" icon="fa-solid fa-battery-quarter" />
        <font-awesome-icon class="icon" icon="fa-solid fa-triangle-exclamation" /> 
        <font-awesome-icon class="icon" icon="fa fa-calendar" @click="setStatus()" /> 
        <font-awesome-icon class="icon" icon="fa-solid fa-clock" /> 
        <font-awesome-icon class="icon" icon="fa-solid fa-repeat" /> 
      </div>
    </div>
  </div>
</template>

<script>
export default {
  computed: {
    list() {
      return this.$store.state.user.lists[this.$store.state.user.currentListId];
    },
  },
  data() {
		return {
			listModel: {
				task: "",
			}
		}
	},
  methods: {
    addTask() {
      const newTask = {
        id: this.list.tasks.length,
        name: this.listModel.task,
        status: "Not Finished",
        priority: "Low",
        date: "∞",
      };
      this.$store.commit("addTask", newTask);
      this.listModel.task = ""; 
      toast.success("Task added", { autoClose: 1000, });
    },
    setStatus() {
      console.log("Something happend !!!")
    }
  }
}
</script>

<style scoped>
.wrapper {
  overflow-y: auto;
  max-height: calc(100vh - 1px);
}
h1 {
  color: white;
  margin: 20px 0 0 340px;
}
h5 {
  margin-top: 20px;
}
table {
  margin: 40px 40px 80px 340px; 
  background-color: white;
  width: 75vw;
}
table, th, td {
  border:1px solid black;
  border-collapse: collapse;
}
th, td {
  width: 100vh;
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
  margin: 40px 0 0 340px; 
	width: 75vw;
	padding: 5px 10px;
	border: 1px solid black;
	color: black;
	border-radius: 6px;
}
.icon-box {
  margin: 5px 0 0 340px; 
  width: 75vw;
  background-color: white;
  padding: 5px 10px;
	border: 1px solid black;
	border-radius: 6px;
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
@media screen and (max-width: 1200px) {
  table, h1, input, .icon-box{
		margin: 40px 40px auto;
	}
}
</style>

