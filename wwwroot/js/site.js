// Site-wide JavaScript

$(document).ready(function() {
    // Fade out alerts after 5 seconds
    setTimeout(function() {
        $('.alert').fadeOut('slow');
    }, 5000);

    console.log("ECS GYM loaded successfully.");
});
