<script setup>
import axios from 'axios';
import SideBar from "../components/SideBar.vue"
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
</script>

<template>
<div class="wrapper">
  <section class="todo">
    <header class="header">
      <h1>todos</h1>
      <input class="new-todo" placeholder="Add List">
    </header>
    <!-- <section class="main" v-show="todos.length">
      <input
        id="toggle-all"
        class="toggle-all"
        type="checkbox"
        :checked="remaining === 0"
        @change="toggleAll"
      >
      <label for="toggle-all">Mark all as complete</label>
      <ul class="todo-list">
        <li
          v-for="todo in filteredTodos"
          class="todo"
          :key="todo.id"
          :class="{ completed: todo.completed, editing: todo === editedTodo }"
        >
          <div class="view">
            <input class="toggle" type="checkbox" v-model="todo.completed">
            <label @dblclick="editTodo(todo)">{{ todo.title }}</label>
            <button class="destroy" @click="removeTodo(todo)"></button>
          </div>
          <input
            v-if="todo === editedTodo"
            class="edit"
            type="text"
            v-model="todo.title"
            @vue:mounted="({ el }) => el.focus()"
            @blur="doneEdit(todo)"
            @keyup.enter="doneEdit(todo)"
            @keyup.escape="cancelEdit(todo)"
          >
        </li>
      </ul>
    </section> -->
    <!-- <footer class="footer" v-show="todos.length">
      <span class="todo-count">
        <strong>{{ remaining }}</strong>
        <span>{{ remaining === 1 ? ' item' : ' items' }} left</span>
      </span>
      <ul class="filters">
        <li>
          <a href="#/all" :class="{ selected: visibility === 'all' }">All</a>
        </li>
        <li>
          <a href="#/active" :class="{ selected: visibility === 'active' }">Active</a>
        </li>
        <li>
          <a href="#/completed" :class="{ selected: visibility === 'completed' }">Completed</a>
        </li>
      </ul>
      <button class="clear-completed" @click="removeCompleted" v-show="todos.length > remaining">
        Clear completed
      </button> 
    </footer> -->
  </section>
</div>
</template>

<script>
export default {

}
</script>

<style scoped>
.wrapper {
  display: flex;
}
.todo {
  padding: 40px;
  margin: 40px;
  width: 100%;
  background-color: white;
}
h1 {
	text-align: left;
	margin-bottom: 40px;
	font-weight: bold;
}
input, textarea {
	width: 100%;
	padding: 5px 10px;
	color: black;
	border-radius: 6px;
}
</style>

