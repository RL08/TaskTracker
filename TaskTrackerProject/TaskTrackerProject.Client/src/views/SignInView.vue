<script setup>
import axios from 'axios';
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
import SideBar from "../components/SideBar.vue"
</script>

<template>
<div class="wrapper">
	<SideBar/>
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
          <div class="form-group" id="bottom">
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
	display: flex;
  flex-direction: column;
  justify-content: space-between;
	background: white;
	padding: 40px;
	margin: 40px 40px 40px 240px;
	border-radius: 15px;
	box-shadow: 0 4px 10px 4px rgba(19, 35, 47, .3);
	font-size: 17px;
	width: 100vw;

}
.tab-group {
	list-style: none;
  padding: 0;
	display: flex;
	justify-content: center;
}
.tab-link {
	display: flex;
	align-items: center;
	text-decoration: none;
	border-radius: 15px;
	background: lightgray;
	color: white;
	margin: 0 2px;
	padding: 15px;
}
li {
	width: 100%;
}
.icon {
  margin-right: 20px;
}
.tab-link:hover, .button:hover{
	background: turquoise;
	color: #fff;
}
.tab-group .active a {
	background: turquoise;
	color: #fff;
}
h1 {
	text-align: center;
	font-weight: lighter;
	margin-bottom: 10px;
}
input, textarea {
	width: 100%;
	height: 5vh;
	padding: 5px 10px;
	border: 2px solid turquoise;
	color: black;
	border-radius: 6px;
}
.form-group {
	margin: 5vh 0;
}
.form-group-icon {
	display: flex;
	justify-content: center;
	align-items: center;
	font-size: 35px;
	margin-bottom: 20px;
}
.button {
	border: 0;
	border-radius: 15px;
	padding: 15px 0;
	letter-spacing: 0.1em;
	background: turquoise;
	color: #fff;
	width: 100%;
}
#ms {
	color: turquoise;
}
@media screen and (max-width: 280px) {  
	.tab-link {
		font-size: 10px;
	}
	.icon {
		display: none;
	}
}
@media screen and (max-width: 415px) {  
  .wrapper {
    font-size: 14px;
  }
	.form {
    font-size: 14px;
  }
	.tab-link {
		font-size: 14px;
	}
}
@media screen and (max-width: 1000px) {
  .form {
		margin: 15vh 10px;
	}
}
@media screen and (min-height: 740px) {  
	.wrapper, .form, .tab-link {
    font-size: 17px;
  }
	h1, .form-group-icon {
		font-size: 40px;
	} 
}
@media screen and (min-height: 1024px) {  
	.wrapper, .form, .tab-link {
    font-size: 28px;
  }
	h1, .form-group-icon {
		font-size: 70px;
	} 
}	
</style>
