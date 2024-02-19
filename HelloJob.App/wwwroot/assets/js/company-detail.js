var modeBtn=document.querySelector('.mode_btn_company_detail')
var mode=document.getElementById('body-company-detail')


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


const likedIcons = document.querySelectorAll('.vacancies__item__right #liked_ico');
const unlikedIcons = document.querySelectorAll('.vacancies__item__right #unliked_ico');

likedIcons.forEach(likeIcon => {
    likeIcon.addEventListener('click', function(e) {
        e.preventDefault();
        this.style.display = 'none';
        unlikedIcons[Array.from(likedIcons).indexOf(this)].style.display = 'inline-block';
    });
});

unlikedIcons.forEach(unlikeIcon => {
    unlikeIcon.addEventListener('click', function(e) {
        e.preventDefault();
        this.style.display = 'none';
        likedIcons[Array.from(unlikedIcons).indexOf(this)].style.display = 'inline-block ';
    });
});

const baglamaButonu = document.querySelector('.baglama-btn');
const nightmenuacButonu = document.querySelector('.menu_open_company_detail');
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



var tabLinks = document.querySelectorAll('.tab__link');

tabLinks.forEach(function(tabLink) {
    tabLink.addEventListener('click', function() {
        tabLinks.forEach(function(link) {
            link.classList.remove('active');
        });
        tabLink.classList.add('active');

        var tabItemId = tabLink.getAttribute('data-tab');

        var tabItems = document.querySelectorAll('.tab__item');

        tabItems.forEach(function(item) {
            if (item.getAttribute('data-tab') === tabItemId) {
                item.classList.add('active');
            } else {
                item.classList.remove('active');
            }
        });
    });
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

