<script setup>
</script>

<template>
  <h1>All List</h1>
  <div class="wrapper">
    <table class="table table-striped table-bordered">
      <thead>
        <tr>
          <th>List</th>
          <th>Status</th>
          <th>Priority</th>
          <th>Progress</th>
        </tr>
      </thead>
      <tbody v-for="list in lists" :key="list.id" @click="redirectTo(list.path, list.id)">
        <tr>
          <td> {{ list.name }} </td>
          <td> Stuck </td>
          <td> Low </td>
          <td> <progress value="20" max="100"> </progress> </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  computed: {
    lists() {
      return this.$store.state.user.lists.filter(list => list.name !== "" && list.path !== "");
    },
  },
  methods: {
    redirectTo(path, listId) {
      this.$store.commit('setCurrentListId', listId);
      this.$router.push(path);
    }
  }
};
</script>

<style scoped>
.wrapper {
  overflow-y: auto;
  max-height: calc(100vh - 1px);
}
h1 {
  color: white;
  margin: 20px 0 0 340px;
}
h5 {
  margin-top: 20px;
}
table {
  margin: 40px 40px 80px 340px; 
  background-color: white;
  width: 75vw;
}
table, th, td {
  border:1px solid black;
  border-collapse: collapse;
}
th, td {
  width: 100vh;
}
progress {
  width: 100%;
  height: 20px;
}
tbody {
  cursor: pointer;
}
tbody:hover {
  background-color: lightgrey;
}
/* unnötig derzeit
.grid {
  display: grid;
  grid-template-columns: repeat(1, 1fr);
  grid-gap: 20px;
  margin-left: 340px;
}
.list {
  background: white;
  box-shadow: 0 4px 10px 4px rgba(19, 35, 47, 0.3);
  font-size: 17px;
  margin-right: 40px;
  height: 250px;
}
.status {
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: red;
  padding: 10px;
}
.list-title {
  display: flex;
  justify-content: center;
  align-items: center;
} */
@media screen and (max-width: 1200px) {
  table, h1{
		margin: 40px 40px auto;
	}
}
</style>


