import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import { checkJwtIsLogin, checkRoleEqual } from '@/utils'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/about',
      name: 'about',
      component: () => import('../views/AboutView.vue')
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/LoginView.vue')
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('../views/RegisterView.vue')
    },
    {
      path: '/backstage',
      name: 'backstage',
      redirect: '/user',
      beforeEnter: (to, from, next) => {
        if (checkJwtIsLogin()) {
          next()
        } else {
          next('/login')
        }
      },
      component: () => import('../views/backstage/BackstageView.vue'),
      children: [
        // 個人資料
        {
          path: '/user',
          name: 'user',
          component: () => import('../views/backstage/UserView.vue')
        },
        // 會員清單
        {
          path: '/memberList',
          name: 'memberList',
          component: () => import('../views/backstage/MemberListView.vue'),
          beforeEnter: (to, from, next) => {
            if (checkRoleEqual('Admin')) {
              next()
            } else {
              next('/user')
            }
          },
        },
        // 活動清單
        {
          path: '/eventList',
          name: 'eventList',
          component: () => import('../views/backstage/EventListView.vue'),
          beforeEnter: (to, from, next) => {
            if (checkRoleEqual('Admin')) {
              next()
            } else {
              next('/user')
            }
          },
        }
      ]
    },
    {
      path: '/404',
      name: '404',
      component: () => import('../views/404View.vue')
    },
    {
      path: '/:pathMatch(.*)*',
      redirect: '/404'
    }
  ]
})

export default router
