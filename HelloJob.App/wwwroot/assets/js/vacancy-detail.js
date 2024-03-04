
const likeIcons = document.querySelectorAll('#like');
const unlikeIcons = document.querySelectorAll('#unlike');

likeIcons.forEach(likeIcon => {
    likeIcon.addEventListener('click', function(e) {
        e.preventDefault();
        this.style.display = 'none';
        unlikeIcons[Array.from(likeIcons).indexOf(this)].style.display = 'inline-block';
    });
});

unlikeIcons.forEach(unlikeIcon => {
    unlikeIcon.addEventListener('click', function(e) {
        e.preventDefault();
        this.style.display = 'none';
        likeIcons[Array.from(unlikeIcons).indexOf(this)].style.display = 'inline-block';
    });
});


//swiper js
var swiper = new Swiper(".mySwiper", {
  slidesPerView: 1,
  spaceBetween: 10,
  pagination: {
    el: ".swiper-pagination",
    clickable: true,
  },
  breakpoints: {
    
    350: {
      slidesPerView:2,
      spaceBetween: 5,
    },
    420: {
      slidesPerView: 4,
      spaceBetween: 5,
    },
    768: {
      slidesPerView: 5,
      spaceBetween: 10,
    },
    1024: {
      slidesPerView: 7,
      spaceBetween: 30,
    },
  },
});