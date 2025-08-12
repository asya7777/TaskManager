<template>
    <div class="register-container">
        <h1>Register</h1>
        <form @submit.prevent="handleRegister">
            <div>
                <label for="email">Enter email</label>
                <input type="email" v-model="email" required />
            </div>
            <div>
                <label for="firstName">Enter name</label>
                <input type="text" v-model="firstName" required />
            </div>
            <div>
                <label for="lastName">Enter surname</label>
                <input type="text" v-model="lastName" required />
            </div>
            <div>
                <label for="password">Enter password</label>
                <input type="password" v-model="password" required />
            </div>
            <button type="submit">Sign Up</button>
        </form>
        <p v-if="error" class="error">{{error}}</p>
    </div>
</template>

<script setup>
    import { ref } from 'vue';

    const email = ref('');
    const firstName = ref('');
    const lastName = ref('');
    const password = ref('');
    const error = ref('');

    const handleRegister = async () => {
        error.value = '';

        try {
            const response = await fetch('http://localhost:5082/api/User/register', {
                method: 'POST'
                , headers: {
                    'Content-Type': 'application/json'//sending JSON data
                },
                body: JSON.stringify({
                    email: email.value,
                    firstName: firstName.value,
                    lastName: lastName.value,
                    password: password.value
                })//convert to JSON
            });

            if (!response.ok) {//was the response successful?
                error.value = 'Register failed';
            }

            const result = await response.text();//if successful, get the response text
            alert(result);//display result
            router.push('/login');//redirect to login page

        }
        catch (err) {
            error.value = 'Network error';//fetch failed
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