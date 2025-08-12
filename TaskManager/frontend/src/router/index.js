import { createRouter, createWebHistory } from 'vue-router'

import LoginPage from '../components/login.vue'
import RegisterPage from '../components/register.vue'
import HomePage from '../components/homepage.vue'
import CreateTask from '../components/CreateTask.vue'

const routes = [
    {
        path: '/login',
        component: LoginPage
    }
    ,{
        path: '/register',
        component: RegisterPage
    }
    , {
        path: '/homepage',
        component: HomePage
    }
    , {
        path: '/create-task', 
        component: CreateTask
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router;