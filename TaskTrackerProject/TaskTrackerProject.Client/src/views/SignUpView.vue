<script setup>
import axios from 'axios';
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
</script>

<template>
<div class="wrapper">
  <div class="form" v-if="!authenticated">
  <ul class="tab-group">
    <li class="tab">
			<router-link to="/signin" class="tab-link">
        <font-awesome-icon class="icon" icon="fa-arrow-left"/> Sign in
      </router-link>
    </li>
    <li class="tab active">
			<router-link to="/signup" class="tab-link" >
        <font-awesome-icon class="icon" icon="fa-arrow-left"/> Sign up
      </router-link>
		</li>
  </ul>
  <div class="form-content">
    <div id="signup">
      <h1>Register</h1>
      <form @submit.prevent="register">
        <div class="form-group">
          <input type="text" required placeholder="Username" v-model="loginModel.username"/>
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
      // const data = {
      //   username: this.loginModel.username,
			// 	email: this.loginModel.email,
      //   password: this.loginModel.password
      // } 
			// console.log(data); 
      // uncomment to see input data  

      try {
        const userdata = (await axios.post('user/register', this.loginModel)).data;
        axios.defaults.headers.common['Authorization'] = `Bearer ${userdata.token}`;
        this.$store.commit('authenticate', userdata);     
        this.$router.push("/");
        console.log("success");
      } catch (e) {
        if(e.response === undefined) { console.error(e); }
        else if (e.response.status == 401) {
          toast.error("Login failed. Invalid credentials.");
        }
        else if (e.response.status == 400) {
          toast.error("User is already in the database.");
        }
      }
    },
  },
  computed: {
    authenticated() {
      return this.$store.state.user.isLoggedIn ? true : false;     
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
	margin: 40px 40px 40px 340px;
	border-radius: 15px;
	box-shadow: 0 4px 10px 4px rgba(19, 35, 47, .3);
	font-size: 17px;
  flex-grow: 1;
  overflow-x: auto;
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
	width: 45%;
	float: left;
	text-align: center;
	margin: 0 10px 0 10px;
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
@media screen and (max-width: 1200px) {
  .form {
		margin: 40px auto;
	}
}
@media screen and (max-height: 700px) {
  .form {
		height: 450px;
		overflow-y: auto;
	}
}
</style>
