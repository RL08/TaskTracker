<script setup>
import axios from 'axios';
import Navbar from "../components/NavBar.vue"
</script>

<template>
<div class="wrapper">
<Navbar/>
  <div class="form" v-if="!authenticated">
    <ul class="tab-group">
      <li class="tab active">
				<router-link to="/signin" class="tab-link">
          <font-awesome-icon class="icon" icon="fa-arrow-left"/> Sign in
        </router-link>
      </li>
      <li class="tab">
				<router-link to="/signup" class="tab-link" >
          <font-awesome-icon class="icon" icon="fa-arrow-left"/> Sign up
        </router-link>
			</li>
    </ul>
    <div class="tab-content">
      <div id="signup">
        <h1>Register</h1>
        <form @submit.prevent="register">
          <div class="form-group">
            <div class="form-group">
              <input type="text" required placeholder="Username" v-model="loginModel.username"/>
            </div>
          </div>
          <div class="form-group">
            <input type="email" required placeholder="Email Address" v-model="loginModel.email"/>
          </div>
          <div class="form-group">
            <input type="password" required placeholder="Password" v-model="loginModel.password"/>
          </div>
          <button type="submit" class="button">Sign Up</button>
        </form>
      </div>
    </div>
  </div> 
  <div class="form" v-else>
    <font-awesome-icon class="check" icon="fa-check"/> Successful
  </div>
</div>
</template>

<script>
export default {
  data() {
    return {
      loginModel: {
        username: "",
        email: "",
        password: "", 
      }
    };
  },
  methods: {
    async register() {
      const host =
        process.env.NODE_ENV == "production"
          ? ""
          : "https://localhost:5001";  

      const data = {
        username: this.loginModel.username,
        email: this.loginModel.email,
        password: this.loginModel.password
      }   
      console.log(data);  

      try {
        const userdata = (await axios.post(host + 'user/register', this.loginModel)).data;
        axios.defaults.headers.common['Authorization'] = `Bearer ${userdata.token}`;
        this.$store.commit('authenticate', userdata);     
        this.$router.push("/");
        console.log("success");
      } catch (e) {
        if (e.response.status == 401) {
            alert('Login failed. Invalid credentials.');
        }
      }
    },
    logout() {
      delete axios.defaults.headers.common['Authorization'];
      this.$store.commit('authenticate', null);
    },
  },
  computed: {
    authenticated() {
      return this.$store.state.user.isLoggedIn ? true : false;     
    },
    name() {
      console.log(this.$store.state.user.username);
      console.log(this.$store.state.user.isLoggedIn);
      return this.$store.state.user.username;
    },
  },
};
</script>

<style scoped>
.wrapper {
  display: flex;
}
.form {
	background: white;
	padding: 40px;
	margin: 40px auto;
	border-radius: 15px;
	box-shadow: 0 4px 10px 4px rgba(19, 35, 47, .3);
	font-size: 17px;
  width: 500px;
  height: 600px;
}
.tab-group {
	list-style: none;
	padding: 0;
}
.tab-link {
  border-radius: 15px;
	text-decoration: none;
	background: lightgray;
	color: white;
	width: 48%;
	float: left;
	text-align: center;
	margin: 0 3.5px 0 3.5px;
	padding: 15px;
}
.tab-group:after {
	content: "";
	display: table;
	clear: both;
}
.tab-link:hover, .button:hover{
	background: #01939c;
	color: #fff;
}
.tab-group .active a {
	background: #01939c;
	color: #fff;
}
h1 {
	text-align: center;
	margin-bottom: 40px;
	font-weight: lighter;
}
input, textarea {
	width: 100%;
	padding: 5px 10px;
	border: 1px solid #01939c;
	color: black;
	border-radius: 6px;
}
.form-group {
	margin-bottom: 40px;
}
.button {
	border: 0;
	border-radius: 15px;
	padding: 15px 0;
	letter-spacing: 0.1em;
	background: #01939c;
	color: #fff;
	width: 100%;
}
#check {
  width: 100%;
  background-color: greenyellow;
}
</style>
