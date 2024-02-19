const filterClearButton = document.getElementById('FilterTemizle');
filterClearButton.addEventListener('click', function() {
    var selectInputs = document.querySelectorAll('.form-select');
    selectInputs.forEach(function(selectInput) {
        selectInput.selectedIndex = 0;
    });

    var checkboxes = document.querySelectorAll('.checkbox');
    checkboxes.forEach(function(checkbox) {
        checkbox.checked = false;
    });
});

const MultiSearch=document.getElementById('multisearch');
const searchbar=document.querySelector('.nsb_searchbar_filter');
const topsearchbar=document.querySelector('.nsb_searchbar_top')
const search_closebutton=document.querySelector('.nsp_close');

MultiSearch.addEventListener('click',function(){
searchbar.style.display="block";
topsearchbar.style.display="none";

})

search_closebutton.addEventListener('click',function(){
    searchbar.style.display="none";
topsearchbar.style.display="flex";
})


const baglamaButonu = document.querySelector('.baglama-btn');
const menuacButonu = document.querySelector('.menu_open');
const sidebar = document.querySelector('.sidebar');

  menuacButonu.addEventListener('click', function (e) {
   console.log(e);
    e.preventDefault()
    sidebar.style.display = 'block';
}
  )
baglamaButonu.addEventListener('click', function (e) {
    e.preventDefault()
    sidebar.style.display = 'none';
});


$('.new-categories-slider').slick({
    dots: false,
    infinite: true,
    speed: 300,
    slidesToShow: 5,
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
          slidesToShow: 3,
          slidesToScroll: 1,
          arrows:false


        }
      }
      // You can unslick at a given breakpoint now by adding:
      // settings: "unslick"
      // instead of a settings object
    ]
  });


    $('.new-companies').slick({
      dots: false,
      infinite: true,
      speed: 300,
      slidesToShow: 5,
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
            slidesToShow: 3,
            slidesToScroll: 1,
            arrows:false
  
  
          }
        }
        // You can unslick at a given breakpoint now by adding:
        // settings: "unslick"
        // instead of a settings object
      ]
    });
  

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
        console.log("333");
      }
      else  {
        mode.className='light'
        modeBtn.className="evening mode_btn"
        localStorage.setItem('mode','light')
        console.log("444");
        

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


    //  Modal start

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

