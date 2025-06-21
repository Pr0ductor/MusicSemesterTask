document.addEventListener("DOMContentLoaded", function() {
    // Инициализация Feather icons
    feather.replace();

    // Добавляем обработчик события для кнопок лайка
    document.querySelectorAll('.like-button').forEach(button => {
        button.addEventListener('click', function(event) {
            event.preventDefault();

            const songId = this.getAttribute('data-song-id');
            const icon = this.querySelector('i');
            const likeCountElement = this.closest('.media-action').querySelector('.likes-count');

            // Получаем антисфальт-токен
            const token = document.querySelector('[name="__RequestVerificationToken"]').value;

            fetch('/Songs/LikeSong', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ songId: songId })
            })
                .then(response => response.json())
                .then(data => {
                    if (icon.classList.contains('liked')) {
                        icon.classList.remove('liked');
                    } else {
                        icon.classList.add('liked');
                    }

                    likeCountElement.textContent = `${data.likesCount} Likes`;
                });
        });
    });
});