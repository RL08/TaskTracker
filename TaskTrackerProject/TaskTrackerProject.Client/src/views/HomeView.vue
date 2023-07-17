<script setup>
</script>

<template>
  <div class="wrapper">
    <h1>All List</h1>
    <div class="grid">
      <div class="card" v-for="list in lists" :key="list.name">
        <div class="card-body">
          <h5 class="card-title"> {{ list.name }} </h5>
          <h5 class="status"> Stuck </h5>
          <progress value="20" max="100"> </progress>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  computed: {
    lists() {
      return this.$store.state.user.lists;
    },
  },
};
</script>

<style scoped>
.wrapper {
  overflow-y: auto;
  max-height: calc(100vh - 50px);
}
h1 {
  margin: 40px 40px 40px 340px;
}
h5 {
  margin-top: 20px;
}
progress {
  height:35%;
  width:100%;
  margin-bottom: 10px;
}
.grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  grid-gap: 20px;
  margin-left: 340px;
}
.card {
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
  border-radius: 15px;
  background-color: red;
  padding: 10px;
}
.card-title {
  display: flex;
  justify-content: center;
  align-items: center;
}
@media screen and (max-width: 992px) {
  .grid {
    grid-template-columns: repeat(2, 1fr);
  }
}
@media screen and (max-width: 576px) {
  .grid {
    grid-template-columns: repeat(1, 1fr);
  }
}
</style>


