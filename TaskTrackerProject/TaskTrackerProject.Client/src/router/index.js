import { createRouter, createWebHistory } from 'vue-router'
import store from '../store.js'
import HomeView from '../views/HomeView.vue'
import FavoriteView from '../views/FavoriteView.vue'
import NewListView from '../views/NewListView.vue'
import SignInView from '../views/SignInView.vue'
import SignUpView from '../views/SignUpView.vue'
import SignInSPGView from '../views/SignInSPGView.vue'


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
      meta: { authorize: true },
    },
    {
      path: '/favorite',
      name: 'favorite',
      component: FavoriteView,
      meta: { authorize: true },
    },
    {
      path: '/list/:listguid',
      name: 'newlist/:listguid',
      component: NewListView,
      meta: { authorize: true },
    },
    {
      path: '/signin',
      name: 'signin',
      component: SignInView
    },
    {
      path: '/signup',
      name: 'signup',
      component: SignUpView
    },
    {
      path: '/signinspg',
      name: 'signinspg',
      component: SignInSPGView
    },
  ]
})

router.beforeEach((to, from, next) => {
  const authenticated = store.state.user.isLoggedIn;
  if (to.meta.authorize && !authenticated) {
    next("/signup");
    return;
  }
// If the user wants to switch to another list while remaining on the current list, the tasks won't be refreshed.
// This code is responsible for refreshing the tasks.
  if (from.params.listguid !== undefined && to.params.listguid !== from.params.listguid) {
    store.commit('getTask', store.state.user.userTasks);
  }
  next();
  return;
});

export default router
