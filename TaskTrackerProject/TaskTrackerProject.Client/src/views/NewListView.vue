<script setup>
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
import Calendar from 'primevue/calendar';
import SideBar from "../components/SideBar.vue"
</script>

<template>
  <div class="wrapper" ref="wrapper">
    <SideBar/>
    <div class="container" id="button-container">
      <button id="back" @click="redirectToHome()"> <font-awesome-icon icon="fa-solid fa-caret-left" /> Back </button> 
    </div>
    <h1> {{ list.name }} </h1>
    <div class="container" id="table-container">
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
    </div>
    <div class="container" id="icon-container">
      <div class="icon-box" v-if="showStatusBox">
        <button class="bg-danger" @click="setStatus('Not Finished')"> Not Finished </button>
        <button class="bg-warning" @click="setStatus('In Progress')"> In Progress </button>
        <button class="bg-success" @click="setStatus('Completed')"> Completed </button>
      </div>
    </div>
    <div class="container" id="icon-container">
      <div class="icon-box" v-if="showPriorityBox">
        <button class="bg-danger"  @click="setPriority('Low')"> Low </button>
        <button class="bg-warning" @click="setPriority('Mid')"> Mid </button>
        <button class="bg-success" @click="setPriority('High')"> High </button>
      </div>
    </div>
    <div class="container" id="icon-container">
      <div class="icon-box" id="datepicker" v-if="showDateBox"> 
        <button class="bg-primary" @click="setDate('Today')"> Today </button>
        <button class="bg-primary" @click="setDate('Tommorow')"> Tommorow </button>
        <button class="bg-primary" @click="setDate('Next Week')"> Next Week </button>
        <Calendar v-model="this.listModel.taskdate" dateFormat="dd/mm/yy" showIcon showButtonBar/>
      </div>
    </div>
    <div class="container" id="fa-icon-container">
      <div class="icon-box" id="fa-icon-box" v-if="this.listModel.task !== ''">
        <font-awesome-icon class="icon" id="icon" icon="fa-solid fa-battery-quarter" @click="toggleStatusBox()" />
        <font-awesome-icon class="icon" icon="fa-solid fa-triangle-exclamation" @click="togglePriorityBox()" /> 
        <font-awesome-icon class="icon" icon="fa fa-calendar" @click="toggleDateBox()"/> 
        <font-awesome-icon class="icon" icon="fa-solid fa-clock" /> 
        <font-awesome-icon class="icon" icon="fa-solid fa-repeat" /> 
      </div>
    </div>
    <div class="container" id="input-container">
      <input type="text" required placeholder="Add Task" v-model="listModel.task" @keyup.enter=addTask()> 
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
      accessDateBox: false,
		}
	},
  components: {
    Calendar,
  },
  methods: {
    addTask() {
      if(this.listModel.task !== "") {
        const newTask = {
        id: this.list.tasks.length,
        name: this.listModel.task,
        status: this.listModel.taskstatus,
        priority: this.listModel.taskpriority,
        /**
         * if accessDateBox is true -> this.listModel.taskdate.toDateString()
         * if accessDateBox is false -> ∞ 
         */
        date: this.accessDateBox ? this.listModel.taskdate.toDateString() : "∞", 
      };
      this.accessDateBox = false;
      this.$store.commit("addTask", newTask);
      this.listModel.task = ""; 
      toast.success("Task added", { autoClose: 1000, });
      }
    },
    scrollToBottom() {
      const wrapper = this.$refs.wrapper;
      wrapper.scrollTop = wrapper.scrollHeight;
    },
    redirectToHome() {
      this.$router.push("/");
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
      this.accessDateBox = true;
    },
    setDate(date) {
      // today is the default value of this.listModel.taskdate
      const today = new Date();
      if(date === "Tommorow") 
      { this.listModel.taskdate.setDate(today.getDate() + 1) }
      if(date === "Next Week") 
      { this.listModel.taskdate.setDate(today.getDate() + 7) }
      this.showDateBox = false;
    }
  },
  watch: {
    'listModel.taskdate': { 
      handler () {
        this.showDateBox = !this.showDateBox;
      },
    },
  },
  updated() {
    this.scrollToBottom();
  }
}
</script>

<style scoped>
h1 {
  color: white;
  margin: 80px 0 10px 200px;
  text-align: center;
}
.container {
  display: flex;
  justify-content: center; 
}
#button-container {
  margin-left: 200px;
  justify-content: left;
}
button {
  border-radius: 6px;
  padding: 5px;
  width: 33%;
  margin-right: 5px;
}
#back {
  position: absolute;
  border-radius: 6px;
  margin: 20px;
  padding: 5px;
  width: 100px;
}
.wrapper {
  overflow-y: auto;
  height: 100vh;
}
#table-container {
  margin: 0 0 10px 200px;
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
tbody {
  cursor: pointer;
}
tbody:hover {
  background-color: lightgrey;
}
#input-container {
  margin: 5px 0 10px 200px;
}
input {
  display: flex;
  justify-content: center;
	width: 70vw;
	padding: 5px 10px;
	border: 1px solid black;
	border-radius: 6px;
}
#icon-container {
  margin: 5px 0 0 200px;
}
#fa-icon-container {
  margin: 10px 0 0 200px;
}
.icon-box {
  display: flex;
  justify-content: center;
  width: 70vw;
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
  justify-content: right;
}
@media screen and (max-width: 1250px) {
  #input-container, 
  #table-container, 
  #icon-container, 
  #fa-icon-container, 
  .container,
  h1 {
    margin-left: auto;
    margin-right: auto;
  }
  #button-container {
    margin: 10px 0 0 40px;
  }
}
</style>