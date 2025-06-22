document.addEventListener("DOMContentLoaded", function () {
    feather.replace();

    document.querySelectorAll('.toggle-like').forEach(button => {
        button.addEventListener('click', async function (event) {
            event.preventDefault();

            const songId = this.getAttribute('data-song-id');
            const icon = this.querySelector('i');
            const likeCountElement = this.closest('.list-item').querySelector('.likes-count');

            try {
                const response = await fetch('/Songs/ToggleLike', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
                    },
                    body: JSON.stringify({ songId: songId })
                });

                const data = await response.json();

                // Меняем состояние кнопки
                if (data.liked) {
                    icon.classList.add('liked');
                } else {
                    icon.classList.remove('liked');
                }

                // Обновляем счетчик лайков
                likeCountElement.textContent = `${data.likesCount} Likes`;
            } catch (error) {
                console.error('Error toggling like:', error);
            }
        });
    });
});