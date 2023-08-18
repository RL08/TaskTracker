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
        favTasks: [],
        currentListGuid: null,
        currentTask: null,
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
    async getAllList(state, listData) {
      const userListArray = [];
      for(let listId in listData) {
        if (listData[listId].userUsername === state.user.name) {
          userListArray.push(listData[listId]);
        }    
      }   
      state.user.lists = userListArray; 
    },
    deleteList(state, list) {
      state.user.lists = state.user.lists.filter(dellist => dellist.guid !== list.guid);
    },
    async getTask(state, taskData) {
      const userTaskArray = [];
      for(let taskId in taskData) {
        if (taskData[taskId].listGuid === state.user.currentListGuid) {
          userTaskArray.push(taskData[taskId]);
        }    
      }  
      state.user.tasks = userTaskArray;
    },
    addTask(state, task) {
      state.user.tasks.push(task); 
    },
    deleteTask(state, task) {
      state.user.tasks = state.user.tasks.filter(deltask => deltask.guid !== task.guid);
    },
    setCurrentListGuid(state, listGuid) {
      state.user.currentListGuid = listGuid;
    },    
    getEditTask(state, task) {
      state.user.currentTask = task;
    },
    addFavTask(state, task) {
      state.user.favTasks.push(task); 
    },
    delFavTask(state, task) {
      state.user.favTasks = state.user.favTasks.filter(deltask => deltask.guid !== task.guid); 
    },
  }
});