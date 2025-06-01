// Обработка погашения кредита
document.getElementById('creditForm').addEventListener('submit', async function (e) {
    e.preventDefault();

    const formData = {
        amount: parseFloat(this.querySelector('input[type="number"]').value)
    };

    try {
        const response = await fetch(`http://localhost:5272/api/Bank/transfer`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                amount: formData.amount  // Исправлено: используем formData.amount вместо amount
            })
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || 'Ошибка при погашении кредита');
        }

        const result = await response.json();
        alert(`Успешно погашено ${formData.amount} ₽! Новый баланс: ${result.newBalance}`);
        this.reset();

        // Обновляем данные на странице
        updateUserData(result);

    } catch (error) {
        console.error('Ошибка:', error);
        alert(error.message);
    }
});

// Обработка перевода средств
document.getElementById('transferForm').addEventListener('submit', async function (e) {
    e.preventDefault();

    const formData = {
        amount: parseFloat(this.elements[0].value),
        recipientAccount: this.elements[1].value
    };

    try {
        const response = await fetch(`http://localhost:5272/api/Bank/pay`, {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(formData)
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || 'Ошибка при переводе средств');
        }

        const result = await response.json();
        alert(`Перевод выполнен! ID операции: ${result.transactionId}`);
        this.reset();

        // Обновляем данные на странице
        updateUserData(result);

    } catch (error) {
        console.error('Ошибка:', error);
        alert(error.message);
    }
});

// Функция обновления данных пользователя
async function updateUserData() {
    try {
        const response = await fetch(`http://localhost:8086/getBank`, {
            headers: headers
        });

        const userData = await response.json();

        // Обновляем баланс и кредит
        document.querySelector('.balance-amount').textContent =
            `${userData.balance.toLocaleString()} ₽`;
        document.querySelector('.credit-info p').textContent =
            `Непогашенный кредит: ${userData.creditDebt.toLocaleString()} ₽`;

    } catch (error) {
        console.error('Ошибка обновления данных:', error);
    }
}