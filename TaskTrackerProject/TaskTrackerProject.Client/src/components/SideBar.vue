<script setup>
import ProfileAvatar from "vue-profile-avatar";
import axios from "axios";
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
</script>

<template>
  <div class="wrapper">
    <button class="navbar-toggler" @click="toggleSidebar()">
    <font-awesome-icon id="bar" icon="fa-bars"/>
  </button>
  <nav id="sidebar" :class="{ 'active': showSidebar }">
    <button class="navbar-toggler" @click="toggleSidebar()">
      <font-awesome-icon id="bar" icon="fa-bars"/>
    </button>
    <div class="sidebar-header" :class="{ 'active': showSidebar }">
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
          <!-- 14 char max -->
          <router-link to="/" class="nav-link"> 
            <font-awesome-icon class="icon" icon="fa-list"/> All List
          </router-link>
        </li>
        <li class="nav-link" @click="showinputfield()">
          <font-awesome-icon class="icon" icon="fa-plus"/> New List
        </li>
        <li v-for="link in links" :key="link.name">
          <router-link :to="link.path" class="nav-link">
            <font-awesome-icon class="icon" icon="fa-solid fa-list-check" /> {{ link.name }}
          </router-link>
        </li>
        <li id="list-name" class="nav-link" :class="{ 'active': showInput }">
          <font-awesome-icon class="icon" icon="fa-plus"/> 
          <input type="text" placeholder="new list" id="list-input" v-model="loginModel.listname" @keyup.enter="addlist()">
        </li>
        <li>
          <div class="nav-link" id="logout" @click="logout()">
            <font-awesome-icon class="icon" icon="fa-right-from-bracket"/> Logout
          </div>
        </li>
      </ul>
    </div>
  </nav>
  </div>
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
    links() {
      return this.$store.state.user.links;
    },
  },
  data() {
    return {
      loginModel: {
        username: "",
        listname: "",
      },  
      showSidebar: false,
      showInput: false,
    };
  }, 
  components: {
    ProfileAvatar,
  },  
  methods: {
    toggleSidebar() {
      this.showSidebar = !this.showSidebar; 
    },
    showinputfield() {
      this.showInput = !this.showInput; 
    },
    addlist() {
      if (this.showInput) {
        const newLink = {
          name: this.loginModel.listname,
          path: "/list",
          icon: "fa-solid fa-list-check",
        };
        this.$store.commit("addLink", newLink);
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
  },
}
</script>

<style scoped>
.wrapper {
  display: flex;
}
#sidebar {
  width: 300px;
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
#list-name.active {
  display: flex;
}
#list-input {
  width: 100%;
  padding: 5px 10px;
}
/** responsive */
.navbar-toggler {
  display: none;
  padding: 12px 0 0 12px;
}
@media screen and (max-width: 1200px) {
  .navbar-toggler {
    display: flex;
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
.sidebar-header.active {
  display: block;
}
#sidebar.active {
  background-color: white;
  display: block;
}
</style>