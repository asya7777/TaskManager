<template>
    <div class="create-task">
        <h2>Create New Task</h2>
        <form @submit.prevent="handleCreate">
            <div>
                <label>Task Name</label>
                <input type="text" v-model="taskName" required />
            </div>
            <div>
                <label>Description</label>
                <textarea type="text" v-model="taskDescription" required></textarea>
            </div>
            <div>
                <label>Deadline</label>
                <input type="text" v-model="taskDeadline" required />
            </div>

            <div>
                <label>Choose a user to assign:</label>
                <select v-model="selectedUserId" required><!--if the user picks the user with id=3, selectedUserId.value=3-->
                    <option disabled value="">Select a user</option><!--selectedUserId = "", daha seçim yapýlmadý -->
                    <option v-for="user in users" :key="user.usrId" :value="user.usrId">
                        {{ user.firstName }} {{ user.lastName }}
                    </option>
                </select>
            </div>

            <button type="submit">Create Task</button>
            <p v-if="error" class="error">{{error}}</p>
        </form>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import { useRouter } from 'vue-router';

    const router = useRouter();

    const taskName = ref('');
    const taskDescription = ref('');
    const taskDeadline = ref('');
    const selectedUserId = ref('');
    const users = ref([]);
    const error = ref('');

    onMounted(async () => {//userlarý fetch lemek için
        try {
            const res = await fetch('http://localhost:5082/api/User/get_all');
            users.value = await res.json();
        } catch (err) {
            console.error('Error fetching users:');
        }
    });

    const handleCreate = async () => {
        error.value = '';

        try {
            const response = await fetch('http://localhost:5082/api/Task', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                }, body: JSON.stringify({
                    taskName: taskName.value,
                    taskDescription: taskDescription.value,
                    taskDeadline: taskDeadline.value,
                    assignedUserId: selectedUserId.value, // Use the selected user ID
                })
            });

            if (!response.ok) {
                throw new Error('Failed to create task');
            }

            alert('Task created successfully!');

            //clear refs
            taskName.value = '';
            taskDescription.value = '';
            taskDeadline.value = '';
            selectedUserId.value = '';

            router.push('/homepage');

        } catch (err) {
            error.value = err.message || 'Network error';
        }
    }
</script>

<style scoped>

</style>
