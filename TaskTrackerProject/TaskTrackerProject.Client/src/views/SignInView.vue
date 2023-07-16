<script setup>
import axios from 'axios';
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
</script>

<template>
<div class="wrapper">
  <div class="form">
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
    <div class="form-content">
      <div id="signup">
        <h1>Login</h1>
        <form @submit.prevent="login">
          <div class="form-group">
            <input type="text" required placeholder="Username" v-model="loginModel.username"/>
          </div>
					<div class="form-group">
            <input type="email" required placeholder="Email Address" v-model="loginModel.email"/>
          </div>
          <div class="form-group">
            <input type="password" required placeholder="Password" v-model="loginModel.password"/>
          </div>
					<div class="form-group-icon">
						<router-link to="/signinms">
							<font-awesome-icon id="ms" icon="fa-brands fa-microsoft"/>
						</router-link>
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
		}
	},
	methods: {
		async login() {
			// const data = {
      //   username: this.loginModel.username,
			// 	email: this.loginModel.email,
      //   password: this.loginModel.password
      // } 
			// console.log(data); 
      // uncomment to see input data

			try {
				const userdata = (await axios.post('user/login', this.loginModel)).data;
        axios.defaults.headers.common['Authorization'] = `Bearer ${userdata.token}`;
        this.$store.commit('authenticate', userdata);     
				this.$router.push("/");
				console.log("Success!");
			} catch (e) {
        if(e.response === undefined) { console.error(e); }
        else if (e.response.status == 401) {
          toast.error("Login failed! Invalid credentials!");
        }
      }
		}
	}
}
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
	overflow-y: auto;
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
.form-group-icon {
	margin-bottom: 40px;
	font-size: 35px;
	display: flex;
	justify-content: center;
	align-items: center;
}
.top-row > div {
	float: left;
	width: 48%;
	margin-right: 4%;
}
.top-row > div:last-child {
	margin: 0;
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
#ms {
	color: #01939c;
}
</style>
