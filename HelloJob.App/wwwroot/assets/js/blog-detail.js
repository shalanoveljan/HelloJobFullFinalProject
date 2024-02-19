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