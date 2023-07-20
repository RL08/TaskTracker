<script setup>
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
import Datepicker from 'vue3-datepicker'
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
        <font-awesome-icon class="icon" id="icon" icon="fa-solid fa-battery-quarter" @click="toggleStatusBox()" />
        <font-awesome-icon class="icon" icon="fa-solid fa-triangle-exclamation" @click="togglePriorityBox()" /> 
        <font-awesome-icon class="icon" icon="fa fa-calendar" @click="toggleDateBox()"/> 
        <font-awesome-icon class="icon" icon="fa-solid fa-clock" /> 
        <font-awesome-icon class="icon" icon="fa-solid fa-repeat" /> 
      </div>
      <div class="icon-box" v-if="showStatusBox">
        <button class="bg-danger" @click="setStatus('Not Finished')"> Not Finished </button>
        <button class="bg-warning" @click="setStatus('In Progress')"> In Progress </button>
        <button class="bg-success" @click="setStatus('Completed')"> Completed </button>
      </div>
      <div class="icon-box" v-if="showPriorityBox">
        <button class="bg-danger"  @click="setPriority('Low')"> Low </button>
        <button class="bg-warning" @click="setPriority('Mid')"> Mid </button>
        <button class="bg-success" @click="setPriority('High')"> High </button>
      </div>
      <div class="icon-box" id="datepicker" v-if="showDateBox"> 
        <Datepicker v-model="this.listModel.taskdate" @keyup.enter="toggleDateBox()"/>
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
        taskstatus: "Not Finished",
        taskpriority: "Low",
        taskdate: new Date(),
			},
      showStatusBox: false,
      showPriorityBox: false,
      showDateBox: false,
		}
	},
  components: {
    Datepicker,
  },
  methods: {
    addTask() {
      const newTask = {
        id: this.list.tasks.length,
        name: this.listModel.task,
        status: this.listModel.taskstatus,
        priority: this.listModel.taskpriority,
        /**
         * if showDateBox is true -> this.listModel.taskdate 
         * if showDateBox is false -> ∞
         */
        date: this.showDateBox ? this.listModel.taskdate : "∞", 
      };
      this.$store.commit("addTask", newTask);
      this.listModel.task = ""; 
      toast.success("Task added", { autoClose: 1000, });
    },
    toggleStatusBox() {
      this.showStatusBox = !this.showStatusBox;
    },
    setStatus(status) {
      this.listModel.taskstatus = status;
      this.showStatusBox = false; 
    },
    togglePriorityBox() {
      this.showPriorityBox = !this.showPriorityBox;
    },
    setPriority(priority) {
      this.listModel.taskpriority = priority;
      this.showPriorityBox = false; 
    },
    toggleDateBox() {
      this.showDateBox = !this.showDateBox;
    },
    exitDatePicker() {
      this.showPriorityBox = false; 
    },
  }
}
</script>

<style scoped>
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
button {
  border-radius: 6px;
  margin-right: 10px;
  padding: 5px;
}
.icon-box {
  margin: 5px 0 10px 340px; 
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
#datepicker {
  height: 360px;
}
@media screen and (max-width: 1200px) {
  table, h1, input, .icon-box{
		margin: 40px 40px auto;
	}
}
</style>

