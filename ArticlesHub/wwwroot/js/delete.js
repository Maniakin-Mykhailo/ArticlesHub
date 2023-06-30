$(document).ready(function () {
    $('.remove-image').click(function (e) {
        e.preventDefault();
        var imageId = $(this).data('image-id');
        $('.remove-image-checkbox[value="' + imageId + '"]').prop('checked', true);
        // Выполните дополнительные действия здесь, если необходимо
    });
});