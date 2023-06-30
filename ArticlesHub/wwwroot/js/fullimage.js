$(document).ready(function () {
    // Открытие изображений в полный размер по нажатию
    $('.img-thumbnail').click(function () {
        window.open($(this).attr('src'), '_blank');
    });
});