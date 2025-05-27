function toggleForm(formType) {
    const loginForm = document.getElementById('login-form');
    const registerForm = document.getElementById('register-form');
    const toggleBtns = document.querySelectorAll('.toggle-btn');

    toggleBtns.forEach(btn => btn.classList.remove('active'));

    if (formType === 'login') {
        loginForm.classList.add('active');
        registerForm.classList.remove('active');
        toggleBtns[0].classList.add('active');
    } else {
        registerForm.classList.add('active');
        loginForm.classList.remove('active');
        toggleBtns[1].classList.add('active');
    }
}

// Обработчик формы входа
document.getElementById('login-form').addEventListener('submit', async function (e) {
    e.preventDefault();

    const email = document.getElementById('login-email').value;
    const password = document.getElementById('login-password').value;
    const messageDiv = document.getElementById('login-message');

    try {
        const response = await fetch('http://localhost:53528/api/Auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                email: email,
                password: password
            })
        });

        const data = await response.json();

        if (response.ok) {
            // Успешный вход
            showMessage(messageDiv, 'Успешный вход! Перенаправление...', 'success');
            // Сохраняем токен (если есть)
            if (data.token) {
                localStorage.setItem('authToken', data.token);
            }
            // Перенаправляем пользователя
            setTimeout(() => {
                window.location.href = '/pageBank';
            }, 1500);
        } else {
            // Ошибка входа
            showMessage(messageDiv, data.message || 'Ошибка входа', 'error');
        }
    } catch (error) {
        showMessage(messageDiv, 'Ошибка сети. Попробуйте позже.', 'error');
        console.error('Login error:', error);
    }
});

// Обработчик формы регистрации
document.getElementById('register-form').addEventListener('submit', async function (e) {
    e.preventDefault();

    const name = document.getElementById('register-name').value;
    const email = document.getElementById('register-email').value;
    const password = document.getElementById('register-password').value;
    const messageDiv = document.getElementById('register-message');

    // Простая валидация пароля
    if (password.length < 6) {
        showMessage(messageDiv, 'Пароль должен содержать минимум 6 символов', 'error');
        return;
    }

    //try {
        const response = await fetch('https://localhost:53528/api/Auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                name: name,
                email: email,
                password: password
            })
        });

        const data = await response.json();

        if (response.ok) {
            // Успешная регистрация
            showMessage(messageDiv, 'Регистрация успешна! Вы можете войти.', 'success');
            // Автоматически переключаем на форму входа
            setTimeout(() => {
                toggleForm('login');
                document.getElementById('login-email').value = email;
            }, 1500);
        } else {
            // Ошибка регистрации
            showMessage(messageDiv, data.message || 'Ошибка регистрации', 'error');
        }
    //} catch (error) {
    //    showMessage(messageDiv, 'Ошибка сети. Попробуйте позже.', 'error');
    //    console.error('Registration error:', error);
    }
});

// Функция для отображения сообщений
function showMessage(element, text, type) {
    element.textContent = text;
    element.className = type + '-message';
}