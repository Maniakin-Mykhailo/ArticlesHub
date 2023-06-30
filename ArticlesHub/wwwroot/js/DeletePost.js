// Найти все ссылки с классом "delete-image"
var deleteLinks = document.getElementsByClassName('delete-image');
for (var i = 0; i < deleteLinks.length; i++) {
    // Добавить обработчик клика для каждой ссылки
    deleteLinks[i].addEventListener('click', function (event) {
        event.preventDefault();
        var imageId = this.getAttribute('data-id');
        var deleteUrl = this.getAttribute('data-url');

        if (confirm('Are you sure you want to delete this image?')) {
            fetch(deleteUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ id: imageId })
            })
                .then(function (response) {
                    if (response.ok) {
                        // Изображение успешно удалено, перезагрузить страницу
                        location.reload();
                    } else {
                        alert('Failed to delete the image.');
                    }
                })
                .catch(function () {
                    alert('Failed to delete the image.');
                });
        }
    });
}
