import { createRouter, createWebHistory } from 'vue-router'
import store from '../store.js'
import HomeView from '../views/HomeView.vue'
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
      component: HomeView
    },
    {
      path: '/alllist',
      name: 'alllist',
      component: HomeView
    },
    {
      path: '/list/:listguid',
      name: 'newlist/:listguid',
      component: NewListView
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
    next("/");
    return;
  }
  next();
  return;
});

export default router
