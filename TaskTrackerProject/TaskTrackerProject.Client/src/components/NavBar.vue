<script setup>
import ProfileAvatar from "vue-profile-avatar"
</script>

<template>
<div class="wrapper">
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
        <li>
          <router-link to="/newlist" class="nav-link">
            <font-awesome-icon class="icon" icon="fa-plus"/> New List
          </router-link>
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
    }
  },
  data() {
    return {
      username: "Dummy",
      showSidebar: false,
    };
  }, 
  components: {
    ProfileAvatar,
  },  
  methods: {
    toggleSidebar() {
      this.showSidebar = !this.showSidebar; 
    },
  },
}
</script>

<style scoped>
.wrapper {
  display: flex;
}
#sidebar {
  width: 150px;
  height: 100vh;
  background-color: white;
  overflow-y: auto
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
/** responsive */
.navbar-toggler {
  display: none;
  padding: 12px 0 0 12px;
}
@media screen and (max-width: 912px) {
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
  }
}
.sidebar-header.active {
  display: block;
}
#sidebar.active {
  background-color: white;
}
</style>