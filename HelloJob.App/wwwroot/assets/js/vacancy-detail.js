
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


var modeBtn=document.querySelector('.mode_btn')
var mode=document.getElementById('body')


if(localStorage.getItem("mode")===null){
 localStorage.setItem("mode","light")
}

else{
 modeBtn.addEventListener('click',function(e){
 e.preventDefault()

 if(modeBtn.className==="evening mode_btn" ){
  
   mode.className='dark'
   modeBtn.className = "morning mode_btn"
   localStorage.setItem('mode','dark')
 }
 else  {
   mode.className='light'
   modeBtn.className="evening mode_btn"
   localStorage.setItem('mode','light')
   

 }
 })

 if(localStorage.getItem('mode')==='light'){
   mode.className='light'
   modeBtn.className="evening mode_btn"
 }

 else{
   mode.className='dark'
   modeBtn.className = "morning mode_btn"
 }
}


const baglamaButonu = document.querySelector('.baglama-btn');
const nightmenuacButonu = document.querySelector('.menu_open');
const sidebar = document.querySelector('.sidebar');


  nightmenuacButonu.addEventListener('click', function (e) {
   console.log(e);
    e.preventDefault()
    sidebar.style.display = 'block';

}
  )
baglamaButonu.addEventListener('click', function (e) {
    e.preventDefault()
    sidebar.style.display = 'none';
});

var registermodal=document.querySelector('.modal-content-register')
var loginmodal=document.querySelector('.modal-content')
var registerbtn=document.querySelector('.register-btn')
var loginbtn=document.querySelector('.loginac')

registerbtn.addEventListener("click",function (e) {
  e.preventDefault()
  registermodal.style.display="flex"
  loginmodal.style.display="none"
})
loginbtn.addEventListener("click",function (e) {
  e.preventDefault()
  registermodal.style.display="none"
  loginmodal.style.display="flex"
})


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