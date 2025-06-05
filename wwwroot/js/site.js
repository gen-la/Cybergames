// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Glitch effect for headings
document.addEventListener('DOMContentLoaded', function() {
    const glitchElements = document.querySelectorAll('.glitch');

    glitchElements.forEach(element => {
        setInterval(() => {
            element.classList.add('active');
            setTimeout(() => {
                element.classList.remove('active');
            }, 200);
        }, 3000);
    });

    // Animate cyber decorations
    const decorations = document.querySelectorAll('.cyber-decoration');
    decorations.forEach(decoration => {
        setInterval(() => {
            decoration.classList.add('pulse');
            setTimeout(() => {
                decoration.classList.remove('pulse');
            }, 1000);
        }, 5000);
    });

    // Card hover effects
    const cards = document.querySelectorAll('.cyber-game-card, .cyber-game-grid-card');
    cards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.querySelector('.cyber-card-glitch').classList.add('active');
        });
        card.addEventListener('mouseleave', function() {
            this.querySelector('.cyber-card-glitch').classList.remove('active');
        });
    });
});