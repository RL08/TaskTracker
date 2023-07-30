import { createStore } from 'vuex'   // npm install vuex

export default createStore({
  state() {
    return {
      user: {
        name: "",
        guid: "",
        isLoggedIn: false,
        lists: [{
          id: "0",
          name: "GetStarted",
          path: "/list",
          currentListId: null,
          currentTaskId: null,
          tasks: [{
            id: "0",
            name: "Run 10 km",
            status : "Not Finished",
            priority: "Low",
            date: "∞",
            favorite: false,
          }],
          favoriteTasks: [],           
        }],
      }
    }
  },
  mutations: {
    authenticate(state, userdata) {
      if (!userdata) {
        state.user = { name: "", guid: "", role: "", isLoggedIn: false };
        return;
      }
      state.user.name = userdata.username;
      state.user.guid = userdata.userGuid;
      state.user.role = userdata.role;
      state.user.isLoggedIn = true;
    },
    addList(state, list) {
      list.tasks = [];
      list.favoriteTasks = [];
      state.user.lists.push(list);
    },
    addTask(state, task) {
      state.user.lists[state.user.currentListId].tasks.push(task);
    }, 
    deleteTask(state, task) {
      state.user.lists[state.user.currentListId].tasks = state.user.lists[state.user.currentListId].tasks.filter(
        currenttask => currenttask.id !== task.id);
    }, 
    addFavoriteTask(state, task) {
      state.user.lists[state.user.currentListId].favoriteTasks.push(task);
    },       
    deleteFavoriteTask(state, task) {
      state.user.lists[state.user.currentListId].favoriteTasks = state.user.lists[state.user.currentListId].favoriteTasks.filter(
        favoritetask => favoritetask.id !== task.id);
    },   
    setCurrentListId(state, listId) {
      state.user.currentListId = listId;
    },    
  }
});