$('.blog-items').slick({
    dots: false,
    infinite: true,
    speed: 300,
    slidesToShow: 3,
    slidesToScroll: 1,
    
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 3,
          infinite: true,
          dots: true
        }
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2
        }
      },
      {
        breakpoint: 560,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          arrows:false


        }
      }
      // You can unslick at a given breakpoint now by adding:
      // settings: "unslick"
      // instead of a settings object
    ]
  });

document.addEventListener('DOMContentLoaded', function() {
  const likeIcons = document.querySelectorAll('.card_like');

  likeIcons.forEach(likeIcon => {
      likeIcon.addEventListener('click', function(e) {
          e.preventDefault();
          const wishlistItem = this.closest('.vacancies__item');
          if (wishlistItem) {
              wishlistItem.remove();
          }
      });
  });
});
