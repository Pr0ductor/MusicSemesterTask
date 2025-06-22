document.addEventListener('DOMContentLoaded', function() {
    if (typeof feather !== 'undefined') {
        feather.replace();
    }
});

function toggleLike(songId, button) {
    $.ajax({
        url: '/Songs/Like',
        type: 'POST',
        data: { songId: songId },
        success: function(response) {
            if (response.success) {
                const icon = button.querySelector('i');
                if (response.isLiked) {
                    icon.classList.add('active-danger');
                } else {
                    icon.classList.remove('active-danger');
                }

                if (typeof feather !== 'undefined') {
                    feather.replace();
                }
            }
        },
        error: function(xhr) {
            if (xhr.status === 401) {
                window.location.href = '/AuthView/Login';
            } else {
                console.error('Error toggling like:', xhr);
            }
        }
    });
}