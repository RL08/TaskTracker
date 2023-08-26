<script setup>
import ProfileAvatar from "vue-profile-avatar";
import axios from "axios";
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
</script>

<template>
  <button class="navbar-toggler" id="first-navbar-toggler" :class="{ 'true': showNavbar }" @click="toggleSidebar()">
    <font-awesome-icon id="bar" icon="fa-bars"/>
  </button>
  <nav id="sidebar" :class="{ 'true': showSidebar }">
    <button class="navbar-toggler" :class="{ 'true': showNavbar }" @click="toggleSidebar()">
      <font-awesome-icon id="bar" icon="fa-bars"/>
    </button>
    <div class="sidebar-header" :class="{ 'true': showSidebar }">
      <div class="authentication-container" v-if="!authenticated">
        <router-link to="/signin" class="nav-link" >
          <font-awesome-icon class="icon" icon="fa-arrow-left"/> Sign in
        </router-link>
        <router-link to="/signup" class="nav-link">
          <font-awesome-icon class="icon" icon="fa-arrow-right"/> Sign up
        </router-link>
      </div> 
      <div class="profile-info">
        <div class="profile" v-if="authenticated">
          <ProfileAvatar id="profile" :username="username" size=m></ProfileAvatar> 
          <div id="profile-name">{{ username }}</div> 
        </div>
        <form class="form-inline">
          <input class="form-control" type="search" placeholder="Search" aria-label="Search" v-model="searchQuery">
        </form>
      </div>
      <ul class="list-unstyled" v-if="!searchQuery && authenticated">
        <li>
          <router-link to="/" class="nav-link" @click="toggleSidebar()"> 
            <font-awesome-icon class="icon" icon="fa-list"/> All List
          </router-link>
        </li>
        <li class="nav-link" @click="showinputfield()">
          <font-awesome-icon class="icon" icon="fa-plus"/> New List
        </li>
        <li class="nav-link" v-if="isDevelopment" @click="addDefaultLists()">
          <font-awesome-icon class="icon" icon="fa-plus"/> Add 10 Lists
        </li>
        <li id="list-name" class="nav-link" :class="{ 'true': showInput }" >
          <font-awesome-icon class="icon" icon="fa-plus"/> 
          <input type="text" placeholder="new list" id="list-input" v-model="listname" @keyup.enter="addList()">
        </li>
        <li class="nav-link" v-for="list in lists" :key="list.id" @click="redirectTo(list)">
          <font-awesome-icon class="icon" icon="fa-solid fa-list-check" /> {{ list.name }}
        </li>
        <li>
          <div class="nav-link" id="logout" @click="logout()">
            <font-awesome-icon class="icon" icon="fa-right-from-bracket"/> Logout
          </div>
        </li>
      </ul>
      <ul class="list-unstyled" v-else-if="searchQuery">
        <label>List:</label> 
        <li class="nav-link" v-for="list in filteredLists" :key="list.id" @click="redirectTo(list)">
          <font-awesome-icon class="icon" icon="fa-solid fa-list-check" /> {{ list.name }}
        </li>
        <label>Task:</label> 
        <li class="nav-link" v-for="task in filteredTasks" :key="task.id" @click="redirectToTask(task)">
          <font-awesome-icon class="icon" icon="fa-solid fa-list-check" /> {{ task.name }}
        </li>
      </ul>
    </div>
  </nav>
</template>

<script>
export default {
  async mounted() {
    await this.getList();
    await this.getTask();
  },
  computed: {
    authenticated() {
      return this.$store.state.user.isLoggedIn;
    },
    username() {
      return this.$store.state.user.name;
    },
    lists() {
      return this.$store.state.user.lists;
    },
    listGuid() {
      return this.$store.state.user.currentListGuid;
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
    filteredLists() {
      return this.lists.filter((list) => list.name.toLowerCase().includes(this.searchQuery.toLowerCase()));
    },
    filteredTasks() {
      return this.userTasks.filter((task) => task.name.toLowerCase().includes(this.searchQuery.toLowerCase()));
    },
  },
  data() {
    return {
      loginModel: {
        username: "",
      }, 
      listname: "", 
      showSidebar: false,
      showNavbar: false,
      showInput: false,
      searchQuery: null,
    };
  }, 
  components: {
    ProfileAvatar,
  },  
  methods: {
    async getList() {
      try { this.$store.commit('getAllList', (await axios.get("list")).data); } 
      catch (e) { toast.error("Error loading API") }
    },
    async getTask() {
      try { this.$store.commit('getUserTask', (await axios.get("task")).data); } 
      catch (e) { toast.error("Error loading API") }
    },
    toggleSidebar() {
      this.showSidebar = !this.showSidebar; 
      this.showNavbar = !this.showNavbar;
    },
    showinputfield() {
      this.showInput = !this.showInput; 
    },
    redirectTo(list) {
      this.toggleSidebar();
      this.$store.commit('setCurrentListGuid', list.guid);
      this.$router.push(`/list/${list.guid}`);
    },
    redirectToTask(task) {
      this.toggleSidebar();
      this.$store.commit('setCurrentListGuid', task.listGuid);
      this.$router.push(`/list/${task.listGuid}`);
    },
    async addList() {
      if (this.authenticated) {
        //ternary operator for default list
        const newList = {
          name: this.listname ? this.listname : "Unknown List",
          userguid: this.$store.state.user.guid
        };
        try { 
          await axios.post('list/addlist', newList); 
          this.getList()
          toast.success("New list has been created", { autoClose: 1000});
        } 
        catch (e) {
          if (e.response.status == 400) {
            toast.error("BadRequest 400")
          }
        }
        this.listname = "";
        this.showInput = false; 
      } 
      else {
        toast.error("You are not logged in")
        return;
      }
    },
    async addDefaultLists() { 
      for (let i = 1; i <= 10; i++) {
        this.addlist(); 
      } 
    },
    logout() {
      if(this.$store.state.user.isLoggedIn) {
        delete axios.defaults.headers.common['Authorization'];
        this.$store.commit('authenticate', null);
        toast.info("You have logged out", { autoClose: 1000, });
        this.$router.push("/");
      } 
      else {
        toast.warning("You are not logged in!", { autoClose: 1000, });
      }
    },
    scrollToBottom() {
      window.scrollTo(0,0);
    }
  },
}
</script>

<style scoped>
#sidebar {
  width: 200px;
  height: 100vh;
  background-color: white;
  overflow-y: auto;
  position: fixed;
}
.list-unstyled {
  padding-left: 0;
}
.icon {
  margin-right: 20px;
}
.nav-link, label {
  padding: 10px;
  display: flex;
  justify-content: left;
  align-items: center;
  width: 100%;
}
.nav-link:hover, .navbar-brand:hover, #bar:hover {
  background-color: turquoise;
}
.form-inline {
  padding: 10px;
  margin-bottom: 10px;
}
input {
  width: 100%;
	height: 5vh;
  font-size: 100%;
}
.profile {
  display: flex;
  justify-content: left; 
  padding: 10px;
}
#profile-name {
  padding-left: 10px;
  font-size: medium;
  font-weight: bold;
  display: flex;
  justify-content: center;
  align-items: center;
}
.authentication-container {
  display: flex;
  flex-direction: column;
  align-items: center;
}
#logout {
  cursor: pointer;
}
/** new list */
#list-name {
  display: none;
}
#list-name.true {
  display: flex;
}
#list-input {
  width: 100%;
  padding: 5px 10px;
}
label {
  font-weight: bold;
}
/** responsive */
#first-navbar-toggler {
  display: none;
}
.navbar-toggler {
  margin: 30px 0 0 10px;
  display: none;
  z-index: 2;
}
@media screen and (max-width: 912px) {
  #first-navbar-toggler {
    display: block;
    position: absolute;
  }
  .navbar-toggler {
    display: block;
  }
  .scrolldown {
    display: block;
    position: absolute;
    right: 10px;
    top: 30px;
    background-color: transparent;
    border: 0;
  }
  #bar {
    border: 2px solid black;
    border-radius: 50%;
    padding: 10px;
    background: lightgrey;
  }
  .sidebar-header {
    display: none;
  }
  #sidebar {
    background-color: transparent;
    display: none;
    width: 100vw;
    z-index: 1;
  }
}
.sidebar-header.true {
  display: block;
}
#sidebar.true {
  background-color: white;
  display: block;
}
#first-navbar-toggler.true {
  display: none;
}
</style>