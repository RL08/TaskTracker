<script setup>
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
import Calendar from 'primevue/calendar';
import SideBar from "../components/SideBar.vue"
import axios from "axios";
</script>

<template>
  <div class="wrapper" ref="wrapper">
    <SideBar/>
    <div class="container" id="button-container">
      <button id="back" @click="redirectToHome()"> <font-awesome-icon icon="fa-solid fa-caret-left" /> Back </button> 
    </div>
    <h1> {{ lists.name }} </h1>
    <div class="container" id="table-container">
      <table class="table">
        <thead>
          <tr>
            <th> Task </th>
            <th> Status </th>
            <th> Priority </th>
            <th> Date </th> 
            <th> Options </th>
          </tr>
        </thead>
        <tbody v-for="task in tasks" :key="task.id">
          <tr>
            <td data-label="Task" @click="enableEditName(task)"> {{ task.name }} </td>
            <td data-label="Status" 
                @click="enableEditStatus(task)"
                :class="{ 'bg-red': task.status === 1, 'bg-yellow': task.status === 2, 'bg-green': task.status === 3 }"
            > 
                {{ getTaskStatus(task) }} 
            </td>
            <td 
              data-label="Priority" 
              @click="enableEditPriority(task)"
              :class="{ 'bg-red': task.priority === 1, 'bg-yellow': task.priority === 2, 'bg-green': task.priority === 3 }"
            > 
                {{ getTaskPriority(task) }} 
            </td>
            <td data-label="Date" @click="enableEditDate(task)"> {{ getTaskDate(task) }} </td>
            <td data-label="Options" > 
              <font-awesome-icon class="icon" :class="{ 'true': edit }" icon="fa-solid fa-pen" @click="enableEdit()"/> 
              <font-awesome-icon class="icon" icon="fa-solid fa-trash" @click="deleteTask(task)"/> 
              <font-awesome-icon class="icon" :class="{ 'true': isTaskFavorite(task) }" icon="fa-solid fa-star" @click="addFavoriteTask(task)"/> 
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <div class="container" id="icon-container">
      <div class="icon-box" v-if="showStatusBox">
        <button class="bg-danger"  @click="setStatus(1)"> Not Finished </button>
        <button class="bg-warning" @click="setStatus(2)"> In Progress </button>
        <button class="bg-success" @click="setStatus(3)"> Completed </button>
      </div>
    </div>
    <div class="container" id="icon-container">
      <div class="icon-box" v-if="showPriorityBox">
        <button class="bg-danger"  @click="setPriority(1)"> Low </button>
        <button class="bg-warning" @click="setPriority(2)"> Medium </button>
        <button class="bg-success" @click="setPriority(3)"> High </button>
      </div>
    </div>
    <div class="container" id="icon-container">
      <div class="icon-box" id="datepicker" v-if="showDateBox"> 
        <button class="bg-primary" @click="setDate('Unlimited')"> Unlimited </button>
        <button class="bg-primary" @click="setDate('Today')"> Today </button>
        <button class="bg-primary" @click="setDate('Tommorow')"> Tommorow </button>
        <button class="bg-primary" @click="setDate('Next Week')"> Next Week </button>
        <Calendar v-model="listModel.taskcalendardate" dateFormat="dd/mm/yy" showIcon/>
      </div>
    </div>
    <div class="container" id="fa-icon-container" v-if="!editName">
      <div class="icon-box" id="fa-icon-box" v-if="listModel.task !== ''">
        <font-awesome-icon class="icon" id="icon" icon="fa-solid fa-battery-quarter" @click="toggleStatusBox()" />
        <font-awesome-icon class="icon" icon="fa-solid fa-triangle-exclamation" @click="togglePriorityBox()" /> 
        <font-awesome-icon class="icon" icon="fa fa-calendar" @click="toggleDateBox()"/> 
      </div>
    </div>
    <div class="container" id="input-container" v-if="!editName">
      <input type="text" required placeholder="Add Task" v-model="listModel.task" @keyup.enter=addTask()> 
    </div>
    <div class="container" id="input-container" v-if="editName">
      <input id="rename" type="text" required placeholder="Rename Task" v-model="listModel.task" @keyup.enter=renameTask()> 
    </div>
    <button v-if="!change" class="scrolldown" @click="scrollToBottom()">
			<font-awesome-icon icon="fa-arrow-down"/>
    </button>
    <button v-if="change" class="scrolldown" @click="scrollToTop()">
			<font-awesome-icon icon="fa-arrow-up"/>
    </button>
    <button v-if="isDevelopment" id="testtask" class="scrolldown" @click="addDefaultTasks()">
			<font-awesome-icon icon="fa-plus"/>
    </button>
  </div>
</template>

<script>
export default {
  async mounted() {
    await this.getTask();
    await this.getUserTask();
  },
  computed: {
    listGuid() {
      return this.$store.state.user.currentListGuid;
    },
    task() {
      return this.$store.state.user.currentTask;
    },
    favtasks() {
      return this.$store.state.user.favTasks;
    },
    lists() {
      return this.$store.state.user.lists;
    },
    tasks() {
      return this.$store.state.user.tasks;
    },
    userTasks() {
      return this.$store.state.user.userTasks;
    },
    isDevelopment() {
      return process.env.NODE_ENV === 'development';
    },
  },
  data() {
		return {
			listModel: {
        task: "",
        taskstatus: 1,
        taskpriority: 1,
        taskdate: null,
        taskcalendardate: new Date(),
			},
      showStatusBox: false,
      showPriorityBox: false,
      showDateBox: false, 
      accessDateBox: false,
      edit: false,
      editName: false,
      editStatus: false,
      editPriority: false,
      editDate: false,
      favorite: false,
      change: false,
		}
	},
  components: {
    Calendar,
  },
  methods: {
    async getTask() { 
      this.$store.commit('getTask', this.userTasks);
    },
    async getUserTask() {
      try { this.$store.commit('getUserTask', (await axios.get("task")).data); } 
      catch (e) { toast.error("Error loading API") }
    },
    async addTask() {
      if(this.listModel.task !== "") {
        //ternary operator for date
        const newTask = {
          name: this.listModel.task,
          status: this.listModel.taskstatus,
          priority: this.listModel.taskpriority,
          isfavorite: false,
          date: this.accessDateBox ? this.listModel.taskdate : null, 
          listGuid: this.listGuid,
        };
        try { 
          // (await axios.post('task/addtask', newTask)).data will be executed
          const task = (await axios.post('task/addtask', newTask)).data; 
          this.$store.commit('addTask', task);
          toast.success("New Task has been created", { autoClose: 1000});
        } 
        catch (e) {
          if (e.response.status == 400) {
            toast.error("Error 400: Server couldn't understand the request")
          }
        }
        this.accessDateBox = false;
        this.listModel.task = ""; 
      }
    },
    async deleteTask(task) {
      this.$store.commit('deleteTask', task);
      await axios.delete(`task/${task.guid}`, task.guid); 
    },
    enableEditName(task) {
      if(this.edit) {
        this.$store.commit('getEditTask', task);
        this.editName = true;
      }
    },
    async renameTask() {
      this.task.name = this.listModel.task;
      await axios.put(`task/${this.task.guid}`, this.task)
      this.listModel.task = ""; 
      this.editName = false;
    },
    getTaskStatus(task) {
      if (task.status === 1) {
        return "Not Finished";
      } 
      else if (task.status === 2) {
        return "In Progress";
      } 
      else if (task.status === 3) {
        return "Completed";
      }
      else {
        return "No Status";
      }
    },
    enableEditStatus(task) {
      if(this.edit) {
        this.$store.commit('getEditTask', task);
        this.editStatus = true;
        this.showStatusBox = true;
      }
    },
    async setStatus(status) {
      this.listModel.taskstatus = status;
      if(this.editStatus) {
        this.task.status = this.listModel.taskstatus; 
        await axios.put(`task/${this.task.guid}`, this.task)
        this.editStatus = false;
      }
      this.showStatusBox = false; 
    },
    getTaskPriority(task) {
      if (task.priority === 1) {
        return "Low";
      }
      else if (task.priority === 2) {
        return "Medium";
      }
      else if (task.priority === 3) { 
        return "High";
      }
      else {
        return "No Priority";
      }
    },
    enableEditPriority(task) {
      if(this.edit) {
        this.$store.commit('getEditTask', task);
        this.editPriority = true;
        this.showPriorityBox = true;
      }
    },
    async setPriority(priority) {
      this.listModel.taskpriority = priority;
      if(this.editPriority) {
        this.task.priority = this.listModel.taskpriority; 
        await axios.put(`task/${this.task.guid}`, this.task)
        this.editPriority = false;
      }
      this.showPriorityBox = false; 
    },
    getTaskDate(task) {
      if(task.date === null) {
        return "Unlimited";
      }
      const dateFormat = new Intl.DateTimeFormat("de-de", {
        dateStyle: "short",
      })
      return dateFormat.format(new Date(task.date));
    },
    enableEditDate(task) {
      if(this.edit) { 
        this.$store.commit('getEditTask', task);
        this.editDate = true;
        this.showDateBox = true;
      }
    },
    async setDate(date) {
      const today = new Date();
      if(this.listModel.taskdate === null) { this.listModel.taskdate = today; }
      if(date === "Unlimited") { this.listModel.taskdate = null; }
      if(date === "Today") { this.listModel.taskdate.setDate(today.getDate()); }
      if(date === "Tommorow") { this.listModel.taskdate.setDate(today.getDate() + 1) }
      if(date === "Next Week") { this.listModel.taskdate.setDate(today.getDate() + 7) }
      if(this.editDate) {
        this.task.date = this.listModel.taskdate; 
        await axios.put(`task/${this.task.guid}`, this.task)
        this.editDate = false;
      }
      this.showDateBox = false;
    },  
    async addFavoriteTask(task) {
      if(!task.isFavorite) {
        task.isFavorite = true; 
        this.$store.commit("addFavTask", task);
      }
      else {
        task.isFavorite = false; 
        this.$store.commit("delFavTask", task);
      }
      await axios.put(`task/${task.guid}`, task)
      // console.log(this.list.favoriteTasks);
    },
    isTaskFavorite(task) {
      return this.favtasks.some((favoriteTask) => favoriteTask.guid === task.guid);
    },
    redirectToHome() {
      this.$router.push("/");
    },
    enableEdit() {
      this.edit = !this.edit; 
    },
    toggleStatusBox() {
      this.showStatusBox = !this.showStatusBox;
    },
    togglePriorityBox() {
      this.showPriorityBox = !this.showPriorityBox;
    },
    toggleDateBox() {
      this.showDateBox = !this.showDateBox;
      this.accessDateBox = true;
    },
    scrollToBottom() {
      const wrapper = this.$refs.wrapper;
      wrapper.scrollTop = wrapper.scrollHeight;
      this.change = true;
    },
    scrollToTop() {
      const wrapper = this.$refs.wrapper;
      wrapper.scrollTop = 0;
      this.change = false;
    },
    async addDefaultTasks() {
      for (let i = 1; i <= 10; i++) {
        this.listModel.task = "Unknown Task"
        this.addTask(); 
      } 
    }
  },
  watch: {
    "listModel.taskcalendardate": { 
      async handler () {
        if(this.listModel.taskdate === null) { this.listModel.taskdate = new Date(); }
        this.listModel.taskdate.setDate(this.listModel.taskcalendardate.getDate());
        this.showDateBox = false;
        if(this.editDate) {
          this.task.date = this.listModel.taskdate; 
          await axios.put(`task/${this.task.guid}`, this.task)
          this.editDate = false;
        }
      },
    },
  },
  updated() {
    if(this.change) {
      this.scrollToBottom();
    }
  }
}
</script>

<style scoped>
.wrapper {
  overflow-y: auto;
  overflow-x: hidden!important;;
  height: 100vh;
}
h1 {
  color: black;
  margin: 80px 0 10px 200px;
  text-align: center;
  font-size: 2em;
}
.container {
  display: flex;
  justify-content: center; 
  width: inherit;
  margin-left: 200px;
  padding: 0 10%;
}
#button-container {
  justify-content: left;
  position: relative;
}
button {
  border-radius: 6px;
  padding: 5px;
  width: 33%;
  margin-right: 5px;
}
#back{
  position: absolute;
  border-radius: 6px;
  background-color: white;
  margin: 20px;
  width: 100px;
}
#back:hover {
  background-color: lightgrey;
}
table {
  width: 70vw;
}
tr {
  background: white;
}
table, th, td {
  border:1px solid black;
  border-collapse: collapse;
}
th, td {
  width: 100vh;
  text-align: center;
}
thead tr{
  background-color: turquoise;
}
tbody {
  cursor: pointer;
}
tbody tr td:hover {
  background-color: lightgrey;
}
#input-container {
  margin-bottom: 10px;
  margin-top: 5px;
}
input {
  display: flex;
  justify-content: center;
	width: 70vw;
	padding: 5px 10px;
	border: 5px solid turquoise;
	border-radius: 6px;
  z-index: 0;
}
input::placeholder {
  color: black;
}
#rename {
  border: 5px solid grey;
  margin-top: 10px;
}
#icon-container {
  margin-top: 5px;
}
#fa-icon-container {
  margin-top: 10px;
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
  margin: 0 10px;
  padding: 5px;
}
#icon {
  margin-right: 10px;
}
.icon:hover {
  background-color: lightgrey;
}
.icon.true {
  color: turquoise;
}
#datepicker {
  justify-content: right;
}
.scrolldown {
  z-index: 2;
  display: block;
  position: absolute;
  right: 10px;
  bottom: 16px;
  border-radius: 50%;
  width: inherit;
  padding: 1px 6px;
  margin: 0;
}
#testtask {
  right: 40px;
}
.bg-red {
  background-color: rgb(242, 132, 132);
}
.bg-yellow {
  background-color: rgb(223, 244, 117);
}
.bg-green {
  background-color: rgb(117, 244, 164);
}
@media screen and (min-width: 280px) {  
  .wrapper {
    font-size: 1rem;
  }
}
@media screen and (min-height: 892px) {  
  .wrapper {
    font-size: 1.4rem;
  }
}
@media screen and (max-width: 912px) {
  h1 {
    margin: 80px auto 10px;
  }
  .container{
    margin: 0 auto;
    padding: 0;
  }
  #button-container {
    margin: 10px 0 0 40px;
  }
  input, table, .icon-box {
    width: 100vw;
  }
  table {
    border-collapse: collapse;
    border: 0;
  }
  thead {
    display: none;
  }
  table, tbody, tr, td {
    display: block;
    width: 100%;
  }
  tr {
    margin: 0 0 15px 0;
    border: 1px solid black;
  }
  tbody tr td:nth-child(even):hover {
    background-color: lightgrey;
  }
  td {
    text-align: right;
    padding-left: 50%;
    position: relative;
  }
  td:before {
    content: attr(data-label);
    position: absolute;
    left: 0;
    width: 50%;
    padding-left: 15px;
    text-align: left;
  }
}
</style>