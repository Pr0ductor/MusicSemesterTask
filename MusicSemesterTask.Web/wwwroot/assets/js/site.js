document.addEventListener('DOMContentLoaded', function() {
    if (typeof feather !== 'undefined') {
        feather.replace();
    }
});

function toggleLike(songId, button) {
    const antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();
    
    $.ajax({
        url: '/Songs/Like',
        type: 'POST',
        data: { songId: songId },
        headers: {
            'RequestVerificationToken': antiForgeryToken
        },
        success: function(response) {
            if (response.success) {
                const icon = button.querySelector('i');
                if (response.isLiked) {
                    icon.classList.add('active-danger');
                } else {
                    icon.classList.remove('active-danger');
                }
                // Re-render Feather icons
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