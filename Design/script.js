document.addEventListener('DOMContentLoaded', () => {
  const toggle = document.querySelector('.menu-toggle');
  const nav = document.querySelector('.nav');
  if (toggle && nav) {
    toggle.addEventListener('click', () => {
      nav.classList.toggle('open');
    });
  }
});

// Optional helper for future enhancements
// function makeInfiniteScroll(selector, speed = 0.7) {
//   const container = document.querySelector(selector);
//   if (!container) return;
//   const items = Array.from(container.children);
//   items.forEach(item => {
//     const clone = item.cloneNode(true);
//     clone.setAttribute('aria-hidden', 'true');
//     container.appendChild(clone);
//   });
//   let scrollPos = 0;
//   let currentSpeed = speed;
//   function animate() {
//     scrollPos += currentSpeed;
//     if (scrollPos >= container.scrollWidth / 2) {
//       scrollPos = 0;
//     }
//     container.scrollLeft = scrollPos;
//     requestAnimationFrame(animate);
//   }
//   animate();
//   container.addEventListener('mouseenter', () => { currentSpeed = 0; });
//   container.addEventListener('mouseleave', () => { currentSpeed = speed; });
// }
// document.addEventListener('DOMContentLoaded', () => {
//   makeInfiniteScroll('.crafts-list-scroll', 0.7);
//   makeInfiniteScroll('.sponsors-list-scroll', 0.4);
// });
