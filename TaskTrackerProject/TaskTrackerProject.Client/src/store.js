import { createStore } from 'vuex'   

export default createStore({
  state() {
    return {
      user: {
        name: "",
        guid: "",
        isLoggedIn: false,
        lists: [],
        tasks: [],
        currentListGuid: null,
        currentTaskId: null,
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
    async getList(state, listData) {
      const userListArray = [];
      const userTaskArray = [];
      for(let list in listData) {
        if (listData[list].userUsername === state.user.name) {
          userListArray.push(listData[list]);
          if(listData[list].id === listData[list].tasks.listId){
            userTaskArray.push(listData[list].tasks);
          }
        }    
      }   
      state.user.lists = userListArray; 
      state.user.tasks = userTaskArray; 
      console.log(state.user.lists)
      console.log(state.user.tasks)
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
    setCurrentListGuid(state, listGuid) {
      state.user.currentListGuid = listGuid;
    },    
  }
});