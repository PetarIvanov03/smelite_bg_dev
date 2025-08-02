// Слайдшоу бекграунд (hero-section)
const bgImages = [
    "/MainPage/img1.png",
    "/MainPage/img2.png",
    "/MainPage/img3.png",
    "/MainPage/img4.png"
];
let bgIndex = 0;
const currentDiv = document.getElementById('hero-bg-img-current');
const nextDiv = document.getElementById('hero-bg-img-next');

currentDiv.style.backgroundImage = `url('${bgImages[0]}')`;
currentDiv.style.transform = 'translateX(0%)';
currentDiv.style.opacity = '1';
nextDiv.style.opacity = '1';
nextDiv.style.transform = 'translateX(100%)';

function slideHeroBg(nextIdx) {
    nextDiv.style.backgroundImage = `url('${bgImages[nextIdx]}')`;
    nextDiv.style.transition = "none";
    nextDiv.style.transform = "translateX(100%)";
    nextDiv.style.opacity = '1';
    setTimeout(() => {
        currentDiv.style.transition = "transform 1.55s cubic-bezier(.74,.11,.31,.98)";
        nextDiv.style.transition = "transform 1.55s cubic-bezier(.74,.11,.31,.98)";
        currentDiv.style.transform = "translateX(-100%)";
        nextDiv.style.transform = "translateX(0%)";
        setTimeout(() => {
            currentDiv.style.transition = "none";
            nextDiv.style.transition = "none";
            currentDiv.style.transform = "translateX(0%)";
            nextDiv.style.transform = "translateX(100%)";
            currentDiv.style.backgroundImage = nextDiv.style.backgroundImage;
        }, 1580);
    }, 16);
}
setInterval(() => {
    let nextIdx = (bgIndex + 1) % bgImages.length;
    slideHeroBg(nextIdx);
    bgIndex = nextIdx;
}, 7000);

// --------- AOS INIT ---------
AOS.init({ duration: 900, once: true, offset: 90 });

// ----------- Infinite Scroll (обща логика) -----------
function makeInfiniteScroll(selector, cardSelector, rowClass) {
    const scroll = document.getElementById(selector);
    if (!scroll) return;
    const cards = Array.from(scroll.querySelectorAll(cardSelector));
    const row = document.createElement('div');
    row.className = rowClass;
    cards.concat(cards).forEach(c => row.appendChild(c.cloneNode(true)));
    scroll.querySelectorAll(cardSelector).forEach(c => c.remove());
    scroll.appendChild(row);

    // Pause loop on hover
    scroll.addEventListener('mouseenter', () => scroll.classList.add('paused'));
    scroll.addEventListener('mouseleave', () => scroll.classList.remove('paused'));

    // Drag to scroll
    let startX, startScroll, dragging = false;
    row.addEventListener('mousedown', e => {
        scroll.classList.add('paused');
        dragging = true;
        startX = e.clientX;
        startScroll = parseInt(row.style.transform.replace('translateX(', '')) || 0;
    });
    document.addEventListener('mousemove', e => {
        if (!dragging) return;
        let diff = e.clientX - startX;
        row.style.transform = `translateX(${startScroll + diff}px)`;
    });
    document.addEventListener('mouseup', () => { dragging = false; });

    // Touch events for mobile
    row.addEventListener('touchstart', e => {
        scroll.classList.add('paused');
        dragging = true;
        startX = e.touches[0].clientX;
        startScroll = parseInt(row.style.transform.replace('translateX(', '')) || 0;
    });
    document.addEventListener('touchmove', e => {
        if (!dragging) return;
        let diff = e.touches[0].clientX - startX;
        row.style.transform = `translateX(${startScroll + diff}px)`;
    });
    document.addEventListener('touchend', () => { dragging = false; });
}
makeInfiniteScroll('infinite-crafts-scroll', '.craft-card', 'infinite-crafts-row');
makeInfiniteScroll('infinite-sponsors-scroll', '.sponsor-card', 'infinite-sponsors-row');

// --------- Reveal ефекти (обща логика за всички reveal секции) ---------
document.querySelectorAll('.reveal-section').forEach(section => {
    function checkReveal() {
        const rect = section.getBoundingClientRect();
        if (rect.top < window.innerHeight * 0.85) section.classList.add('visible');
        else section.classList.remove('visible');
    }
    window.addEventListener('scroll', checkReveal);
    window.addEventListener('resize', checkReveal);
    window.addEventListener('DOMContentLoaded', checkReveal);
});

// --------- BURGER MENU for mobile ---------
const burgerBtn = document.getElementById('burger-btn');
const nav = document.getElementById('main-nav');
burgerBtn.addEventListener('click', function() {
    nav.classList.toggle('open');
    burgerBtn.classList.toggle('open');
    document.body.classList.toggle('menu-open');
});
nav.querySelectorAll('a').forEach(a =>
    a.addEventListener('click', () => {
        nav.classList.remove('open');
        burgerBtn.classList.remove('open');
        document.body.classList.remove('menu-open');
    })
);
document.addEventListener('click', function(e){
    if(nav.classList.contains('open') && !nav.contains(e.target) && e.target!==burgerBtn) {
        nav.classList.remove('open');
        burgerBtn.classList.remove('open');
        document.body.classList.remove('menu-open');
    }
}, true);

// --------- Sticky header само докато е hero-section ---------
const header = document.querySelector('header');
const hero = document.querySelector('.hero-section');
let lastSticky = false, fadeTimeout = null;
function handleStickyHeader() {
    const heroBottom = hero.getBoundingClientRect().bottom;
    if (heroBottom <= 0) {
        if (lastSticky) {
            header.classList.add('fade-out');
            document.body.classList.remove('sticky-header');
            clearTimeout(fadeTimeout);
            fadeTimeout = setTimeout(() => {
                header.classList.remove('sticky', 'fade-out');
            }, 460);
            lastSticky = false;
        }
    } else {
        if (!lastSticky) {
            header.classList.add('sticky');
            header.classList.remove('fade-out');
            document.body.classList.add('sticky-header');
            lastSticky = true;
        }
    }
}
window.addEventListener('scroll', handleStickyHeader);
window.addEventListener('resize', handleStickyHeader);
window.addEventListener('DOMContentLoaded', handleStickyHeader);


// Newsletter subscribe demo (може да вържеш с бекенд, тук е само визуално)
document.querySelectorAll('.newsletter-form').forEach(form => {
    form.addEventListener('submit', function(e) {
        e.preventDefault();
        form.querySelector('.newsletter-success').style.display = "block";
        setTimeout(() => {
            form.querySelector('.newsletter-success').style.display = "none";
            form.reset();
        }, 4000);
    });
});
