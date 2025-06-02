// Глобальные переменные для отслеживания состояния
let sessionStartTime = Date.now();
let isSimulatingIssues = false;
const SESSION_THRESHOLD = 120000; // 2 минуты в миллисекундах
const RETRY_DELAY = 1000; // Задержка между попытками в мс
const MAX_RETRIES = 3; // Максимальное количество попыток

// Элементы интерфейса
const simulationIndicator = document.getElementById('simulationIndicator');
const loadingOverlay = document.getElementById('loadingOverlay');

// Функция для показа/скрытия индикатора загрузки
function showLoading(show) {
    loadingOverlay.style.display = show ? 'flex' : 'none';
}

// Функция для проверки времени сессии и активации симуляции проблем
function checkSessionTime() {
    const sessionDuration = Date.now() - sessionStartTime;
    if (sessionDuration > SESSION_THRESHOLD && !isSimulatingIssues) {
        isSimulatingIssues = true;
        simulationIndicator.style.display = 'block';
        console.log('Активирована симуляция проблем (таймауты и ошибки)');
    }
}

// Модифицированная функция для выполнения запросов с обработкой ретраев
async function makeRequestWithRetry(url, options, retries = MAX_RETRIES) {
    checkSessionTime();
    const simulateParams = isSimulatingIssues ? {
        simulateTimeout: Math.random() > 0.8,
        simulateError: Math.random() > 0.9
    } : {};
    const fullUrl = `${url}?${new URLSearchParams(simulateParams)}`;

    for (let i = 0; i < retries; i++) {
        showLoading(true);
        try {
            const response = await fetch(fullUrl, options);
            if (!response.ok) throw new Error(`HTTP ошибка! статус: ${response.status}`);
            const data = await response.json();
            showLoading(false);

            // Как только получили ответ 200, сразу выключаем эмуляцию проблем сети
            isSimulatingIssues = false;
            simulationIndicator.style.display = 'none';

            return data;
        } catch (error) {
            showLoading(false);
            if (i === retries - 1) throw error;
            alert(`Проблема с соединением. Повторная попытка ${i + 2} из ${retries}...`);
            await new Promise(resolve => setTimeout(resolve, RETRY_DELAY * (i + 1)));
        }
    }
}

// Модифицированные обработчики форм
document.getElementById('creditForm').addEventListener('submit', async function (e) {
    e.preventDefault();
    const amount = parseFloat(this.querySelector('input[type="number"]').value);

    if (isNaN(amount) || amount <= 0) {
        alert('Пожалуйста, введите корректную сумму');
        return;
    }

    try {
        const result = await makeRequestWithRetry(
            'https://localhost:44319/api/Bank/pay',
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('token-user')}`
                },
                body: JSON.stringify({ amount })
            }
        );

        alert(`Успешно погашено ${amount} ₽! Новый баланс: ${result.newBalance}`);
        this.reset();
        updateUserData();
    } catch (error) {
        console.error('Ошибка:', error);
        alert(`Ошибка при погашении кредита: ${error.message}`);
    }
});

document.getElementById('transferForm').addEventListener('submit', async function (e) {
    e.preventDefault();
    const amount = parseFloat(this.elements[0].value);
    const recipientAccount = this.elements[1].value;

    if (isNaN(amount) || amount <= 0) {
        alert('Пожалуйста, введите корректную сумму');
        return;
    }

    if (!recipientAccount) {
        alert('Пожалуйста, введите номер счета получателя');
        return;
    }

    try {
        const result = await makeRequestWithRetry(
            'https://localhost:44319/api/Bank/transfer',
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('token-user')}`
                },
                body: JSON.stringify({ amount, recipientAccount })
            }
        );

        alert(`Перевод выполнен! Новый баланс: ${result.newBalance}`);
        this.reset();
        updateUserData();
    } catch (error) {
        console.error('Ошибка:', error);
        alert(`Ошибка при переводе средств: ${error.message}`);
    }
});

// Модифицированная функция обновления данных
async function updateUserData() {
    try {
        const userData = await makeRequestWithRetry(
            'https://localhost:44319/api/View/pageBank',
            {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('user-token')}`
                }
            }
        );

        // Обновляем данные на странице
        document.querySelector('.balance-amount').textContent =
            `${userData.balance.toLocaleString('ru-RU')} ₽`;
        document.querySelector('.credit-info p').textContent =
            `Непогашенный кредит: ${userData.unpaidCredit.toLocaleString('ru-RU')} ₽`;
    } catch (error) {
        if (updateRetries++ < MAX_UPDATE_RETRIES) {
            await new Promise(resolve => setTimeout(resolve, 10000));
            updateUserData();
        } else {
            alert('Ошибка после нескольких попыток');
        }
    }
}

// Проверяем время сессии каждые 30 секунд
setInterval(checkSessionTime, 30000);

// Инициализация при загрузке страницы
document.addEventListener('DOMContentLoaded', () => {
    // Начинаем отсчет времени сессии
    sessionStartTime = Date.now();

    // Загружаем данные пользователя
    updateUserData();

    // Добавляем кнопку для ручного тестирования (только для разработки)
    if (window.location.hostname === 'localhost') {
        const testBtn = document.createElement('button');
        testBtn.textContent = 'Тест ошибок';
        testBtn.style.position = 'fixed';
        testBtn.style.bottom = '70px';
        testBtn.style.right = '20px';
        testBtn.style.padding = '10px';
        testBtn.style.zIndex = '1000';
        testBtn.onclick = () => {
            isSimulatingIssues = !isSimulatingIssues;
            simulationIndicator.style.display = isSimulatingIssues ? 'block' : 'none';
            alert(`Режим тестирования ${isSimulatingIssues ? 'включен' : 'выключен'}`);
        };
        document.body.appendChild(testBtn);
    }
});