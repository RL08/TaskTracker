import { createStore } from 'vuex'   // npm install vuex

export default createStore({
  state() {
    return {
      user: {
        name: "",
        guid: "",
        isLoggedIn: false,
        currentListId: null,
        lists: [{
          id: "",
          name: "",
          path: "",
          tasks: [{
            id: "",
            name: "",
            status : "",
            priority: "",
            date: "",
          }],
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
      state.user.lists.push(list);
    },
    addTask(state, task) {
      state.user.lists[state.user.currentListId].tasks.push(task);
    },    
    setCurrentListId(state, listId) {
      state.user.currentListId = listId;
    },    
  }
});