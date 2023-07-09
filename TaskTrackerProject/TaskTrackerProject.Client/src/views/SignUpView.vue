<script setup>
import Navbar from "../components/NavBar.vue"
</script>

<template>
<div class="wrapper">
<Navbar/>
  <div class="form">
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
    <div class="tab-content">
      <div id="signup">
        <h1>Login</h1>
        <form>
          <div class="field-wrap">
            <input type="email" required placeholder="Email Address"/>
          </div>
          <div class="field-wrap">
            <input type="password" required placeholder="Password" />
          </div>
          <button type="submit" class="button">Sign Up</button>
        </form>
      </div>
    </div>
  </div> 
</div>
</template>

<script>

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
.field-wrap {
	margin-bottom: 40px;
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
</style>
