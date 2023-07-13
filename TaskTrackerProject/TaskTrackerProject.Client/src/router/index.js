import { createRouter, createWebHistory } from 'vue-router'
import store from '../store.js'
import HomeView from '../views/HomeView.vue'
import NewListView from '../views/NewListView.vue'
import SignInView from '../views/SignInView.vue'
import SignUpView from '../views/SignUpView.vue'
import SignInMSView from '../views/SignInMSView.vue'


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/newlist',
      name: 'newlist',
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
      path: '/signinms',
      name: 'signinms',
      component: SignInMSView
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
