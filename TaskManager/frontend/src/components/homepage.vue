<template>
    <div class="homepage">
        <main class="main-content">
            <h1 class="title">TASK MANAGER</h1>
            <button class="add-task" @click="$router.push('/create-task')">+</button>
        </main>

        <nav class="task-nav">
            <h2>To do:</h2>
            <ul>
                <li v-for="task in assignedTasks" :key="task.taskId"><!--for loop to display tasks-->
                    {{task.taskName}}
                </li>
            </ul>

        </nav>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import CreateTask from './CreateTask.vue';

    const assignedTasks = ref([]);

    onMounted(async () => {
        const userId = localStorage.getItem('userId'); // Get userId from local storage(login.vue de koyduk)
        const response = await fetch(`http://localhost:5082/api/Task/get_tasks/${userId}`);//string var diye backtick kullandýk
        assignedTasks.value = await response.json();
    })

</script>

<style scoped>
    .homepage {
        display: flex;
    }

    .main-content {
        flex: 1;
        padding: 20px;
        color: bisque;
    }

    .title {
        text-align: center;
        color: darkseagreen;
        font-size: 2rem;
        margin-bottom: 20px;
    }


    .add-task {
        background-color: darkseagreen;
        color: bisque;
        border: none;
        border-radius: 50%;
        width: 50px;
        height: 50px;
        font-size: 24px;
        cursor: pointer;
    }

    .task-nav {
        background-color: darkseagreen;
        padding: 20px;
        width: 300px;
        color: bisque;
    }
</style>