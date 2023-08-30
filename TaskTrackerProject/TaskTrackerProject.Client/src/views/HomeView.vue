<script setup>
import SideBar from "../components/SideBar.vue"
import axios from 'axios';
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
</script>

<template>
  <div class="wrapper" ref="wrapper">
    <SideBar/>
    <h1>All List</h1>
    <div class="container">
      <table class="table">
        <thead>
          <tr>
            <th> List </th>
            <th> Status </th>
            <th> Priority </th>
            <th> Progress </th>
            <th> Options </th>
          </tr>
        </thead>
        <tbody v-for="list in lists" :key="list.id">
          <tr
            :id="'edit-' + list.guid"
            :style="{ 'background-color': edit && list.guid === editListGuid ? 'lightgrey' : '' }"
          >
            <td data-label="" v-if="edit && list.guid === editListGuid"> 
              <input id="rename" type="text" required placeholder="Rename List" v-model="newName" @keyup.enter=renameList(list)> 
            </td>
            <td data-label="List" v-else @click="redirectTo(list)"> {{ list.name }} </td>
            <td 
              data-label="Status" 
              @click="redirectTo(list)"
              id="status"
              :class="{ 'bg-red': list.tasks.some((task) => task.status === 1),
                        'bg-yellow': list.tasks.some((task) => task.status === 2),
                        'bg-green': list.tasks.some((task) => task.status === 3)}"
            > 
              {{ getListStatus(list) }} 
            </td>
            <td 
              data-label="Priority" 
              @click="redirectTo(list)"
              :class="{ 'bg-red': list.tasks.some((task) => task.status === 1),
                        'bg-yellow': list.tasks.some((task) => task.status === 2),
                        'bg-green': list.tasks.some((task) => task.status === 3)}"
            > 
              {{ getListPriority(list) }} 
            </td>
            <td data-label="Progress" @click="redirectTo(list)"> 
              <progress :value="getCompletedTaskCount(list)" :max="list.tasks.length"> </progress> 
            </td> 
            <td data-label="Options"> 
              <font-awesome-icon class="icon" :class="{ 'true': edit }" icon="fa-solid fa-pen" @click="enableEdit(list)"/> 
              <font-awesome-icon class="icon" icon="fa-solid fa-trash" @click="deleteList(list)"/> 
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <button class="scrolldown" @click="scrollToBottom()">
			<font-awesome-icon id="bar" icon="fa-arrow-down"/>
    </button>
    <button v-if="change" class="scrolldown" @click="scrollToTop()">
			<font-awesome-icon id="bar" icon="fa-arrow-up"/>
    </button>
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
  //note: don't use 'this.' in v-if because it will be undefined in production
  data() {
    return {
      edit: false,
      editListGuid: null,
      newName: "",
      change: false,
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
      if(list.name === "Favorite") { this.$router.push("/favorite"); }
      else {
        this.$store.commit('setCurrentListGuid', list.guid);
        this.$router.push(`/list/${list.guid}`); 
      }
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
      if (this.editListGuid) {
        var previousElement = document.getElementById("edit-" + this.editListGuid);
        previousElement.style.backgroundColor = '';
      }
      var element = document.getElementById("edit-" + list.guid);
      element.style.backgroundColor = 'lightgrey';

      if(this.editListGuid === list.guid) {  this.edit = !this.edit; }
      else { this.edit = true; }
      this.editListGuid = list.guid;
    },
    async renameList(list) {
      list.name = this.newName;
      await axios.put(`list/${list.guid}`, list)
      this.newName = ""; 
      this.edit = false;
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
    }
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
  width: inherit;
  margin-left: 200px;
  padding: 0 10%;
}
h1 {
  color: black;
  margin: 80px 0 10px 200px;
  text-align: center;
  font-size: 2em;
}onsole
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
tbody tr td:hover {
  background-color: lightgrey;
}
progress {
  width: 100%;
  height: 20px;
}
tbody {
  cursor: pointer;
}
input {
  width: 100%;
  border: 0;
}
#rename::placeholder {
  text-align: center;
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
.scrolldown {
  z-index: 2;
  display: block;
  position: absolute;
  right: 10px;
  bottom: 10px;
  border-radius: 50%;
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
  progress {
    width: 50%;
    vertical-align: middle;
    padding: 15px;
  }
}
</style>