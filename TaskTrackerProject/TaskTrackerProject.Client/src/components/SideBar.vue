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
          <input class="form-control" type="search" placeholder="Search" aria-label="Search">
        </form>
      </div>
      <ul class="list-unstyled">
        <li>
          <router-link to="/" class="nav-link"> 
            <font-awesome-icon class="icon" icon="fa-list"/> All List
          </router-link>
        </li>
        <li class="nav-link" @click="showinputfield()">
          <font-awesome-icon class="icon" icon="fa-plus"/> New List
        </li>
        <div class="nav-link" v-if="isDevelopment">
          <font-awesome-icon class="icon" icon="fa-plus"/> 
          <button class="btn" @click="addDefaultLists">Add 10 Lists </button>
        </div>
        <li id="list-name" class="nav-link" :class="{ 'true': showInput }">
          <font-awesome-icon class="icon" icon="fa-plus"/> 
          <input type="text" placeholder="new list" id="list-input" v-model="loginModel.listname" @keyup.enter="addlist()">
        </li>
        <li class="nav-link" v-for="list in lists" :key="list.name" @click="redirectTo(list.path, list.id)">
          <font-awesome-icon class="icon" icon="fa-solid fa-list-check" /> {{ list.name }}
        </li>
        <li>
          <div class="nav-link" id="logout" @click="logout()">
            <font-awesome-icon class="icon" icon="fa-right-from-bracket"/> Logout
          </div>
        </li>
      </ul>
    </div>
  </nav>
</template>

<script>
export default {
  computed: {
    authenticated() {
      return this.$store.state.user.isLoggedIn;
    },
    username: {
      get() {
        return this.$store.state.user.name;
      }
    },
    lists() {
      return this.$store.state.user.lists;
    },
    isDevelopment() {
      return process.env.NODE_ENV === 'development';
    }
  },
  data() {
    return {
      loginModel: {
        username: "",
        listname: "",
      },  
      showSidebar: false,
      showNavbar: false,
      showInput: false,
    };
  }, 
  components: {
    ProfileAvatar,
  },  
  methods: {
    toggleSidebar() {
      this.showSidebar = !this.showSidebar; 
      this.showNavbar = !this.showNavbar;
    },
    showinputfield() {
      this.showInput = !this.showInput; 
    },
    redirectTo(path, listId) {
      this.$store.commit('setCurrentListId', listId);
      this.$router.push(path);
    },
    addlist() {
      if (this.showInput) {
        const newList = {
          id: this.$store.state.user.lists.length,
          name: this.loginModel.listname,
          path: "/list",
          icon: "fa-solid fa-list-check",
        };
        this.$store.commit("addList", newList);
        this.loginModel.listname = "";
        this.showInput = !this.showInput; 
        toast.success("New list has been created", { autoClose: 1000});
        // console.log(this.$store.state.user.links);
      }
    },
    logout() {
      if(this.$store.state.user.isLoggedIn) {
        delete axios.defaults.headers.common['Authorization'];
        this.$store.commit('authenticate', null);
        toast.info("You have logged out", { autoClose: 1000, });
      } 
      else {
        toast.warning("You are not logged in!", { autoClose: 1000, });
      }
    },
    addDefaultLists() {
      for (let i = 1; i <= 10; i++) {
        const list = {
          name: "Unknown list (" + this.$store.state.user.lists.length + ")",
          path: "/list",
          icon: "fa-solid fa-list-check",
          id: this.$store.state.user.lists.length,
        };
        this.$store.commit('addList', list);
      } 
      toast.success("Test list has been created", { autoClose: 1000});
    },
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
.nav-link {
  padding: 10px;
  display: flex;
  justify-content: left;
  align-items: center;
  width: 100%;
}
.nav-link:hover, .navbar-brand:hover, #bar:hover {
  background-color: lightgrey;
}
.form-inline {
  padding: 10px;
  margin-bottom: 10px;
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
.btn {
  padding: 0;
}
/** responsive */
#first-navbar-toggler {
  display: none;
}
.navbar-toggler {
  margin: 30px 0 0 10px;
  display: none;
}
@media screen and (max-width: 1250px) {
  #first-navbar-toggler {
    display: block;
    position: absolute;
  }
  .navbar-toggler {
    display: block;
  }
  #bar {
    border: 2px solid black;
    border-radius: 50%;
    padding: 10px;
  }
  .sidebar-header {
    display: none;
  }
  #sidebar {
    background-color: transparent;
    display: none;
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