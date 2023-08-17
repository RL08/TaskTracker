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
      <table class="table table-bordered">
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
            <td @click="enableEditName(task)"> {{ task.name }} </td>
            <td @click="enableEditStatus(task)"> {{ getTaskStatus(task) }} </td>
            <td @click="enableEditPriority(task)"> {{ getTaskPriority(task) }} </td>
            <td @click="enableEditDate(task)"> {{ getTaskDate(task) }} </td>
            <td> 
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
        <Calendar v-model="this.listModel.taskcalendardate" dateFormat="dd/mm/yy" showIcon/>
      </div>
    </div>
    <div class="container" id="fa-icon-container" v-if="!this.editName">
      <div class="icon-box" id="fa-icon-box" v-if="this.listModel.task !== ''">
        <font-awesome-icon class="icon" id="icon" icon="fa-solid fa-battery-quarter" @click="toggleStatusBox()" />
        <font-awesome-icon class="icon" icon="fa-solid fa-triangle-exclamation" @click="togglePriorityBox()" /> 
        <font-awesome-icon class="icon" icon="fa fa-calendar" @click="toggleDateBox()"/> 
        <font-awesome-icon class="icon" icon="fa-solid fa-clock" /> 
        <!-- <font-awesome-icon class="icon" icon="fa-solid fa-repeat" />  -->
      </div>
    </div>
    <div class="container" id="input-container" v-if="!editName">
      <input type="text" required placeholder="Add Task" v-model="listModel.task" @keyup.enter=addTask()> 
    </div>
    <div class="container" id="input-container" v-if="editName">
      <input id="rename" type="text" required placeholder="Rename Task" v-model="listModel.task" @keyup.enter=renameTask()> 
    </div>
  </div>
</template>

<script>
export default {
  async mounted() {
    await this.getTask();
  },
  computed: {
    listGuid() {
      return this.$store.state.user.currentListGuid;
    },
    task() {
      return this.$store.state.user.currentTask;
    },
    lists() {
      return this.$store.state.user.lists;
    },
    tasks() {
      return this.$store.state.user.tasks;
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
		}
	},
  components: {
    Calendar,
  },
  methods: {
    async getTask() {
      try { this.$store.commit('getTask', (await axios.get("task")).data); } 
      catch (e) { toast.error("Error loading API") }
    },
    async addTask() {
      if(this.listModel.task !== "") {
        // if accessDateBox is true -> this.listModel.taskdate
        // if accessDateBox is false -> null
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
          console.log(e)
          if (e.response.status == 400) {
            toast.error("BadRequest 400")
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
      console.log(this.listModel.taskstatus)
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
        this.getTask();
        this.editDate = false;
      }
      this.showDateBox = false;
    },  
    addFavoriteTask(task) {
      const favoriteTask = {
        id: task.id,
        name: task.name,
        status: task.status,
        priority: task.priority,
        date: task.date, 
        favorite: task.favorite,
      };
      if(!task.favorite) {
        this.$store.commit("addFavoriteTask", favoriteTask);
        task.favorite = true;
        toast.info("Favorite Task marked", { autoClose: 1000, });
      }
      else {
        this.$store.commit("deleteFavoriteTask", favoriteTask);
        task.favorite = false;
        toast.info("Favorite Task unmarked", { autoClose: 1000, });
      }
      // console.log(this.list.favoriteTasks);
    },
    isTaskFavorite(task) {
      // return this.list.favoriteTasks.some((favoriteTask) => favoriteTask.id === task.id);
      return
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
    },
  },
  watch: {
    "listModel.taskcalendardate": { 
      async handler () {
        this.listModel.taskdate = this.listModel.taskcalendardate;
        this.showDateBox = false;
        if(this.editDate) {
          this.task.date = this.listModel.taskdate; 
          await axios.put(`task/${this.task.guid}`, this.task)
          this.editDate = false;
        }
      },
    },
    "listModel.task": {
      handler () {
        if(this.listModel.task !== "") {
          this.scrollToBottom();
        }
      }
    }
  },
  // user view will be at the bottom when he click
  // updated() {
  //   this.scrollToBottom();
  // }
}
</script>

<style scoped>
h1 {
  color: black;
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
  background-color: white;
  margin: 20px;
  padding: 5px;
  width: 100px;
}
#back:hover {
  background-color: lightgrey;
}
.wrapper {
  overflow-y: auto;
  overflow-x: hidden!important;;
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
thead {
  background-color: turquoise;
}
tbody {
  cursor: pointer;
}
td:hover {
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
	border: 5px solid turquoise;
	border-radius: 6px;
}
input::placeholder {
  color: black;
}
#rename {
  border: 5px solid grey;
  margin-top: 10px;
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
.icon.true {
  color: turquoise;
}
#datepicker {
  justify-content: right;
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
  input, table, .icon-box {
    width: 100vw;
  }
}
</style>