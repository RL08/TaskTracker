<script setup>
import SideBar from "./components/SideBar.vue"
</script>

<template id="app">
    <SideBar/>
    <router-view/>
</template>

<style>
#app {
    overflow-y: auto;
    max-height: calc(100vh - 1px);
}
</style>

