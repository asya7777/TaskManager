<template>
    <div class="login-container">
        <h1>Login</h1>
        <form @submit.prevent="handleLogin">
            <div>
                <label for="email">Enter email</label>
                <input type="email" v-model="email" required />
            </div>
            <div>
                <label for="password">Enter password</label>
                <input type="password" v-model="password" required />
            </div>
            <button type="submit">Login</button>
        </form>
        <p v-if="error" class="error">{{error}}</p>
    </div>
</template>

<script setup>
    import { ref } from 'vue';//reactive variable

    const email = ref('');
    const password = ref('');
    const error = ref('');

    const handleLogin = async () => {
        error.value = '';//reset error message

        if (email.value == 'admin@example.com' && password.value == '123') {
            alert('Login successful!');
        } else {
            error.value = 'Invalid email or password';
        }
    }
</script>

<style scoped>
    .login-container {
        .login-container

    {
        max-width: 400px;
        margin: 80px auto;
        padding: 2rem;
        border: 1px solid #ccc;
        border-radius: 8px;
    }

    input {
        width: 100%;
        padding: 0.5rem;
        margin-top: 0.25rem;
        margin-bottom: 1rem;
        border: 1px solid #aaa;
        border-radius: 4px;
    }

    button {
        width: 100%;
        padding: 0.5rem;
        background-color: #42b983;
        border: none;
        color: white;
        font-weight: bold;
        cursor: pointer;
    }

    .error {
        color: red;
        margin-top: 1rem;
    }
    }
</style>