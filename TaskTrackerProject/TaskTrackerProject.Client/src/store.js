import { createStore } from 'vuex'   // npm install vuex

export default createStore({
  state() {
    return {
      user: {
        name: "",
        guid: "",
        isLoggedIn: false,
        lists: [{
          name: "",
          path: "",
          tasks: [{
            status : "Stuck",
            priority: "Low",
          }]
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
      state.user.lists.push(list);
    },
    addTask(state, { listId, task }) {
      state.user.lists[listId].tasks.push(task);
    },
  }
});